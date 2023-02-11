using CodeLibrary24.EditorUtilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class GraphViewTestWindow : EditorWindow
{
    private static readonly Vector2 MinWindowSize = new Vector2(1400, 800);
    private CustomGraphView _graphView;
    private CustomInspectorView _inspectorView;
    private NodeHub _selectedNodeHub;
    private NodeHubBarController _nodeHubBarController;

    [FormerlySerializedAs("m_VisualTreeAsset")]
    [SerializeField]
    private VisualTreeAsset mVisualTreeAsset = default;

    [MenuItem("CodeLibrary24/EditorUtilities/Tests/Graph View")]
    public static void ShowExample()
    {
        GraphViewTestWindow window = GetWindow<GraphViewTestWindow>();
        window.minSize = MinWindowSize;
        window.titleContent = new GUIContent("GraphViewTestWindow");
    }

    private void OnEnable()
    {
        CustomGraphEventChannel.Instance.onClearViewsRequested += ClearViews;
        CustomGraphEventChannel.Instance.onChangeDetected += OnChangeDetected;
    }

    private void OnDisable()
    {
        CustomGraphEventChannel.Instance.onClearViewsRequested -= ClearViews;
        CustomGraphEventChannel.Instance.onChangeDetected -= OnChangeDetected;
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
        mVisualTreeAsset.CloneTree(rootVisualElement);
        _graphView = rootVisualElement.Q<CustomGraphView>();
        _inspectorView = rootVisualElement.Q<CustomInspectorView>();
        _nodeHubBarController = new NodeHubBarController(rootVisualElement, OnNodeHubSelected);

        _graphView.OnNodeViewSelected = OnNodeSelectionChanged;
    }

    private void OnNodeHubSelected(NodeHub selectedNodeHub)
    {
        _graphView.PopulateView(selectedNodeHub);
    }

    private void OnNodeSelectionChanged(NodeView nodeView)
    {
        EditorGUIUtility.PingObject(nodeView.node);
        _inspectorView.InspectTargetObject<Editor>(nodeView.node);
    }
}