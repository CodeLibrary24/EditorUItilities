using System;
using CodeLibrary24.EditorUtilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphViewTestWindow : EditorWindow
{
    private static readonly Vector2 MinWindowSize = new Vector2(1400, 800);
    private CustomGraphView _graphView;
    private CustomInspectorView _inspectorView;
    private NodeHub _selectedNodeHub;
    private NodeHubBarController _nodeHubBarController;
    
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("CodeLibrary24/EditorUtilities/Tests/Graph View")]
    public static void ShowExample()
    {
        GraphViewTestWindow window = GetWindow<GraphViewTestWindow>();
        window.minSize = MinWindowSize;
        window.titleContent = new GUIContent("GraphViewTestWindow");
    }

    private void OnEnable()
    {
        CustomGraphEventChannel.Instance.OnClearViewsRequested += ClearViews;
        CustomGraphEventChannel.Instance.OnChangeDetected += OnChangeDetected;

    }
    
    private void OnDisable()
    {
        CustomGraphEventChannel.Instance.OnClearViewsRequested -= ClearViews;
        CustomGraphEventChannel.Instance.OnChangeDetected -= OnChangeDetected;
    }

    private void OnChangeDetected()
    {
        
    }

    private void ClearViews()
    {
        _graphView.ResetView();
    }

    public void CreateGUI()
    {
        m_VisualTreeAsset.CloneTree(rootVisualElement);
        _graphView = rootVisualElement.Q<CustomGraphView>();
        _inspectorView = rootVisualElement.Q<CustomInspectorView>();
        _nodeHubBarController = new NodeHubBarController(rootVisualElement, OnNodeHubSelected);
    }

    private void OnNodeHubSelected(NodeHub selectedNodeHub)
    {
        _graphView.PopulateView(selectedNodeHub);
        
    }
}