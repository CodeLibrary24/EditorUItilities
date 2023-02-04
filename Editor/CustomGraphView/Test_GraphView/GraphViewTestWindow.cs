using System.Collections;
using System.Collections.Generic;
using CodeLibrary24.EditorUtilities;
using CodeLibrary24.EditorUtilities.Popups;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphViewTestWindow : EditorWindow
{
    private static readonly Vector2 MinWindowSize = new Vector2(1400, 800);
    private CustomGraphView _graphView;
    private CustomInspectorView _inspectorView;
    private ToolbarMenu _nodeHubDropdown;
    private NodeHub _selectedNodeHub;

    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("CodeLibrary24/EditorUtilities/Tests/Graph View")]
    public static void ShowExample()
    {
        GraphViewTestWindow window = GetWindow<GraphViewTestWindow>();
        window.minSize = MinWindowSize;
        window.titleContent = new GUIContent("GraphViewTestWindow");
    }

    public void CreateGUI()
    {
        m_VisualTreeAsset.CloneTree(rootVisualElement);
        _graphView = rootVisualElement.Q<CustomGraphView>();
        _inspectorView = rootVisualElement.Q<CustomInspectorView>();
        InitializeNodeHubToolbar();
    }

    private List<NodeHub> GetNodeHubs()
    {
        return EditorUtils.GetAllInstances<NodeHub>();
    }

    private void InitializeNodeHubToolbar()
    {
        _nodeHubDropdown = rootVisualElement.Q<ToolbarMenu>("NodeHubToolbarMenu");

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
            PopupManager.ShowGenericNotificationPopup(rootVisualElement.localBound, "", "No node hubs found. Please create one", null);
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

    private IEnumerator ShowNodeHubNotFoundPopup()
    {
        var waitForSeconds = new EditorWaitForSeconds(1.0f);

        yield return waitForSeconds;
        PopupManager.ShowGenericNotificationPopup(focusedWindow.rootVisualElement.contentRect, "Alert!", "No node hubs found. Please create one", null);
    }

    // TODO: Add option to create and delete node hubs from the node hub bar

    private void OnNodeHubSelected(string nodeHubName)
    {
        List<NodeHub> nodeHubs = GetNodeHubs();

        _selectedNodeHub = nodeHubs.Find(x => x.name == nodeHubName);
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