using System;
using System.Collections;
using System.Collections.Generic;
using CodeLibrary24.EditorUtilities.Popups;
using CodeLibrary24.Utilities;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CodeLibrary24.EditorUtilities
{
    public class NodeHubBarController
    {
        private ToolbarMenu _nodeHubDropdown;

        private readonly VisualElement _rootVisualElement;
        private readonly Action<CustomNodeHub> _onNodeHubSelected;
        private readonly Action _onRefreshRequested;

        private CustomNodeHub _selectedCustomNodeHub;

        public NodeHubBarController(VisualElement rootVisualElement, Action<CustomNodeHub> onNodeHubSelected, Action onRefreshRequested)
        {
            _rootVisualElement = rootVisualElement;
            _onNodeHubSelected = onNodeHubSelected;
            _onRefreshRequested = onRefreshRequested;
            InitializeNodeHubToolbar();
        }

        private void InitializeNodeHubToolbar()
        {
            InitializeDropdown();
            InitializeButtons();
        }

        private void InitializeButtons()
        {
            Button refreshButton = _rootVisualElement.Q<ToolbarButton>("RefreshButton");
            refreshButton.clicked += OnRefreshButtonClicked;
            
            Button createButton = _rootVisualElement.Q<Button>("CreateNodeHubButton");
            createButton.clicked += CreateNodeHub;

            Button deleteButton = _rootVisualElement.Q<Button>("DeleteNodeHubButton");
            deleteButton.clicked += DeleteSelectedNodeHub;
            
            
        }

        private void OnRefreshButtonClicked()
        {
            _onRefreshRequested?.Invoke();
        }

        private void CreateNodeHub()
        {
            CustomNodeHub customNodeHub = ScriptableObject.CreateInstance<CustomNodeHub>();
            string assetPath =
                EditorUtility.SaveFilePanelInProject("Please select path", "New Node Hub", "asset", "Enter name");
            try
            {
                AssetDatabase.CreateAsset(customNodeHub, assetPath);
            }
            catch
            {
                PopupManager.ShowGenericNotificationPopup(_rootVisualElement.worldBound, "Asset Not Created!", "Asset creation cancelled by user", null);
                throw;
            }

            EditorUtils.SaveScriptableObject(customNodeHub); 

            RefreshDropdown();
            _selectedCustomNodeHub = customNodeHub;
            SetDropdownText(customNodeHub.name);
            _onNodeHubSelected(_selectedCustomNodeHub);
        }

        private void DeleteSelectedNodeHub()
        {
            if (_selectedCustomNodeHub == null)
            {
                PopupManager.ShowGenericNotificationPopup(_rootVisualElement.worldBound, "Can't Delete!", "No Node Hub is selected.", null);
                return;
            }

            PopupManager.ShowGenericConfirmationPopup(_rootVisualElement.worldBound, "Do you want to delete the node hub: " + _selectedCustomNodeHub.name, () =>
            {
                List<CustomNodeHub> nodeHubs = GetNodeHubs();
                nodeHubs.Remove(_selectedCustomNodeHub);
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(_selectedCustomNodeHub));
                _selectedCustomNodeHub = null;
                CustomGraphEventChannel.Instance.onClearViewsRequested?.Invoke();
                SetDropdownText();
                RefreshDropdown();
            }, null);
        }

        private void InitializeDropdown()
        {
            _nodeHubDropdown = _rootVisualElement.Q<ToolbarMenu>("NodeHubToolbarMenu");
            SetDropdownText();
            RefreshDropdown();
        }

        private void RefreshDropdown()
        {
            _nodeHubDropdown.menu.MenuItems().Clear();
            List<CustomNodeHub> nodeHubs = GetNodeHubs();

            if (nodeHubs == null)
            {
                EditorCoroutineUtility.StartCoroutine(ShowNodeHubNotFoundPopup(), this);
                return;
            }

            for (var i = 0; i < nodeHubs.Count; i++)
            {
                AddOptionToDropdown(i, nodeHubs[i]);
            }
        }

        private void AddOptionToDropdown(int i, CustomNodeHub customNodeHub)
        {
            _nodeHubDropdown.menu.InsertAction(i, customNodeHub.name, OnNodeHubDropdownValueChanged);
        }

        private void OnNodeHubDropdownValueChanged(DropdownMenuAction obj)
        {
            OnNodeHubSelected(obj.name);
        }

        private List<CustomNodeHub> GetNodeHubs()
        {
            return EditorUtils.GetAllInstances<CustomNodeHub>();
        }

        private void OnNodeHubSelected(string nodeHubName)
        {
            List<CustomNodeHub> nodeHubs = GetNodeHubs();

            _selectedCustomNodeHub = nodeHubs.Find(x => x.name == nodeHubName);
            if (_selectedCustomNodeHub)
            {
                SetDropdownText(nodeHubName);
                _onNodeHubSelected(_selectedCustomNodeHub);
            }
            else
            {
                Debug.LogError("Node hub not found");
            }
        }

        private void SetDropdownText(string text = "Select Node Hub")
        {
            _nodeHubDropdown.text = text;
        }

        private IEnumerator ShowNodeHubNotFoundPopup()
        {
            var waitForSeconds = new EditorWaitForSeconds(1.0f);
            yield return waitForSeconds;
            PopupManager.ShowGenericNotificationPopup(_rootVisualElement.localBound, "Alert!", "Node hubs list is null!!", null);
        }

        public void ForceSelectNodeHub(CustomNodeHub customNodeHub)
        {
            _selectedCustomNodeHub = customNodeHub;
            SetDropdownText(customNodeHub.name);
        }
    }
}