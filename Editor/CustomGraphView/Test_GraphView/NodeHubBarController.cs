using System;
using System.Collections;
using System.Collections.Generic;
using CodeLibrary24.EditorUtilities.Popups;
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
        private readonly Action<NodeHub> _onNodeHubSelected;

        private NodeHub _selectedNodeHub;

        public NodeHubBarController(VisualElement rootVisualElement, Action<NodeHub> onNodeHubSelected)
        {
            _rootVisualElement = rootVisualElement;
            _onNodeHubSelected = onNodeHubSelected;
            InitializeNodeHubToolbar();
        }

        private void InitializeNodeHubToolbar()
        {
            InitializeDropdown();
            InitializeButtons();
        }

        private void InitializeButtons()
        {
            Button createButton = _rootVisualElement.Q<Button>("CreateNodeHubButton");
            createButton.clicked += CreateNodeHub;

            Button deleteButton = _rootVisualElement.Q<Button>("DeleteNodeHubButton");
            deleteButton.clicked += DeleteSelectedNodeHub;
        }

        private void CreateNodeHub()
        {
            NodeHub nodeHub = ScriptableObject.CreateInstance<NodeHub>();
            string assetPath =
                EditorUtility.SaveFilePanelInProject("Please select path", "New Node Hub", "asset", "Enter name");
            try
            {
                AssetDatabase.CreateAsset(nodeHub, assetPath);
            }
            catch
            {
                PopupManager.ShowGenericNotificationPopup(_rootVisualElement.worldBound, "Asset Not Created!", "Asset creation cancelled by user", null);
                throw;
            }

            EditorUtils.SaveScriptableObject(nodeHub); 

            RefreshDropdown();
            _selectedNodeHub = nodeHub;
            SetDropdownText(nodeHub.name);
            _onNodeHubSelected(_selectedNodeHub);
        }

        private void DeleteSelectedNodeHub()
        {
            if (_selectedNodeHub == null)
            {
                PopupManager.ShowGenericNotificationPopup(_rootVisualElement.worldBound, "Can't Delete!", "No Node Hub is selected.", null);
                return;
            }

            PopupManager.ShowGenericConfirmationPopup(_rootVisualElement.worldBound, "Do you want to delete the node hub: " + _selectedNodeHub.name, () =>
            {
                List<NodeHub> nodeHubs = GetNodeHubs();
                nodeHubs.Remove(_selectedNodeHub);
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(_selectedNodeHub));
                _selectedNodeHub = null;
                RefreshDropdown();
                CustomGraphEventChannel.Instance.OnClearViewsRequested?.Invoke();
            }, null);
        }

        private void InitializeDropdown()
        {
            _nodeHubDropdown = _rootVisualElement.Q<ToolbarMenu>("NodeHubToolbarMenu");
            RefreshDropdown();
        }

        private void RefreshDropdown()
        {
            _nodeHubDropdown.menu.MenuItems().Clear();
            List<NodeHub> nodeHubs = GetNodeHubs();

            if (nodeHubs == null)
            {
                EditorCoroutineUtility.StartCoroutine(ShowNodeHubNotFoundPopup(), this);
                Debug.LogError("No node hubs found. Please create one");
                return;
            }

            if (nodeHubs.Count == 0)
            {
                EditorCoroutineUtility.StartCoroutine(ShowNodeHubNotFoundPopup(), this);
                PopupManager.ShowGenericNotificationPopup(_rootVisualElement.localBound, "", "No node hubs found. Please create one", null);
                return;
            }

            for (var i = 0; i < nodeHubs.Count; i++)
            {
                AddOptionToDropdown(i, nodeHubs[i]);
            }
        }

        private void AddOptionToDropdown(int i, NodeHub nodeHub)
        {
            _nodeHubDropdown.menu.InsertAction(i, nodeHub.name, OnNodeHubDropdownValueChanged);
        }

        private void OnNodeHubDropdownValueChanged(DropdownMenuAction obj)
        {
            OnNodeHubSelected(obj.name);
        }

        private List<NodeHub> GetNodeHubs()
        {
            return EditorUtils.GetAllInstances<NodeHub>();
        }

        private void OnNodeHubSelected(string nodeHubName)
        {
            List<NodeHub> nodeHubs = GetNodeHubs();

            _selectedNodeHub = nodeHubs.Find(x => x.name == nodeHubName);
            if (_selectedNodeHub)
            {
                SetDropdownText(nodeHubName);
                _onNodeHubSelected(_selectedNodeHub);
            }
            else
            {
                Debug.LogError("Node hub not found");
            }
        }

        private void SetDropdownText(string text)
        {
            _nodeHubDropdown.text = text;
        }

        private IEnumerator ShowNodeHubNotFoundPopup()
        {
            var waitForSeconds = new EditorWaitForSeconds(1.0f);
            yield return waitForSeconds;
            PopupManager.ShowGenericNotificationPopup(_rootVisualElement.localBound, "Alert!", "No node hubs found. Please create one", null);
        }
    }
}