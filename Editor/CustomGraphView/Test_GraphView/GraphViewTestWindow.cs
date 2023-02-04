using System.Collections.Generic;
using CodeLibrary24.EditorUtilities;
using CodeLibrary24.EditorUtilities.Popups;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphViewTestWindow : EditorWindow
{
    private static Vector2 _minWindowSize = new Vector2(1400, 800);
    private CustomGraphView _graphView;
    private CustomInspectorView _inspectorView;
    private ToolbarMenu _nodeHubDropdown;
    private NodeHub _selectedNodeHub;

    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("CodeLibrary24/Tests/Graph View")]
    public static void ShowExample()
    {
        GraphViewTestWindow window = GetWindow<GraphViewTestWindow>();
        window.minSize = _minWindowSize;
        window.titleContent = new GUIContent("GraphViewTestWindow");
    }

    public void CreateGUI()
    {
        m_VisualTreeAsset.CloneTree(rootVisualElement);
        _graphView = rootVisualElement.Q<CustomGraphView>();
        _inspectorView = rootVisualElement.Q<CustomInspectorView>();
        InitializeNodeHubToolbar();
    }


    private void InitializeNodeHubToolbar()
    {
        List<NodeHub> nodeHubs = EditorUtils.GetAllInstances<NodeHub>();
        _nodeHubDropdown = rootVisualElement.Q<ToolbarMenu>("NodeHubToolbarMenu");

        if (nodeHubs == null)
        {
            Debug.LogError("No node hubs found. Please create one");
            PopupManager.ShowGenericNotificationPopup(focusedWindow.rootVisualElement.contentRect, "", "No node hubs found. Please create one", null);
            return;
        }

        if (nodeHubs.Count == 0)
        {
            Debug.LogError("No node hubs found. Please create one");
            PopupManager.ShowGenericNotificationPopup(focusedWindow.rootVisualElement.contentRect, "", "No node hubs found. Please create one", null);
            return;
        }

        // _nodeHubDropdown.RegisterValueChangedCallback((e) => { OnNodeHubSelected(nodeHubs.Find(x => x.name == e.newValue)); });
    }
    

    private void SelectFirstNodeHub()
    {
    }

    // TODO: Add option to create and delete node hubs from the node hub bar

    private void OnNodeHubSelected(NodeHub nodeHub)
    {
        _selectedNodeHub = nodeHub;
        if (_selectedNodeHub)
        {
            _graphView.PopulateView(_selectedNodeHub);
        }
        else
        {
            Debug.LogError("Node hub not found");
        }
    }
}