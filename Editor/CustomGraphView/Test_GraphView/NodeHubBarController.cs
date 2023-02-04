using System;
using System.Collections;
using System.Collections.Generic;
using CodeLibrary24.EditorUtilities.Popups;
using Unity.EditorCoroutines.Editor;
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

        public NodeHubBarController(VisualElement rootVisualElement, Action<NodeHub> onNodeHubSelected)
        {
            _rootVisualElement = rootVisualElement;
            _onNodeHubSelected = onNodeHubSelected;
            InitializeNodeHubToolbar();
        }

        private void InitializeNodeHubToolbar()
        {
            _nodeHubDropdown = _rootVisualElement.Q<ToolbarMenu>("NodeHubToolbarMenu");

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
                var hub = nodeHubs[i];
                _nodeHubDropdown.menu.InsertAction(i, nodeHubs[i].name, OnNodeHubDropdownValueChanged);
            }
        }

        private void OnNodeHubDropdownValueChanged(DropdownMenuAction obj)
        {
            OnNodeHubSelected(obj.name);
            _nodeHubDropdown.text = obj.name;
        }

        private List<NodeHub> GetNodeHubs()
        {
            return EditorUtils.GetAllInstances<NodeHub>();
        }

        private void OnNodeHubSelected(string nodeHubName)
        {
            List<NodeHub> nodeHubs = GetNodeHubs();

            NodeHub selectedNodeHub = nodeHubs.Find(x => x.name == nodeHubName);
            if (selectedNodeHub)
            {
                _onNodeHubSelected(selectedNodeHub);
            }
            else
            {
                Debug.LogError("Node hub not found");
            }
        }

        private IEnumerator ShowNodeHubNotFoundPopup()
        {
            var waitForSeconds = new EditorWaitForSeconds(1.0f);
            yield return waitForSeconds;
            PopupManager.ShowGenericNotificationPopup(_rootVisualElement.localBound, "Alert!", "No node hubs found. Please create one", null);
        }
    }
}