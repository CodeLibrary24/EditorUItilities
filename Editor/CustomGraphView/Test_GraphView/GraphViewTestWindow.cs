using CodeLibrary24.EditorUtilities;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphViewTestWindow : EditorWindow
{
    private static readonly Vector2 MinWindowSize = new Vector2(1400, 800);
    private static CustomGraphView _graphView;
    private CustomInspectorView _inspectorView;
    private static NodeHub _selectedNodeHub;
    private static NodeHubBarController _nodeHubBarController;

    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("CodeLibrary24/EditorUtilities/Tests/Graph View")]
    public static void OpenWindow()
    {
        GraphViewTestWindow window = GetWindow<GraphViewTestWindow>();
        window.minSize = MinWindowSize;
        window.titleContent = new GUIContent("GraphViewTestWindow");
    }

    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        NodeHub nodeHub = Selection.activeObject as NodeHub;
        
        if (nodeHub)
        {
            OpenWindow();
            OpenToolFromNodeHub(nodeHub);
            return true;
        }

        return false;
    }

    private static void OpenToolFromNodeHub(NodeHub nodeHub)
    {
        _selectedNodeHub = nodeHub;
        _nodeHubBarController.ForceSelectNodeHub(nodeHub);
        OnNodeHubSelected(nodeHub);
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
        if (m_VisualTreeAsset == null)
        {
            return;
        }
        m_VisualTreeAsset.CloneTree(rootVisualElement);
        _graphView = rootVisualElement.Q<CustomGraphView>();
        _inspectorView = rootVisualElement.Q<CustomInspectorView>();
        _nodeHubBarController = new NodeHubBarController(rootVisualElement, OnNodeHubSelected, Refresh);

        _graphView.OnNodeViewSelected = OnNodeSelectionChanged;
    }

    private void Refresh()
    {
        _graphView.Refresh();
    }

    private static void OnNodeHubSelected(NodeHub selectedNodeHub)
    {
        _graphView.PopulateView(selectedNodeHub);
    }

    private void OnNodeSelectionChanged(NodeView nodeView)
    {
        EditorGUIUtility.PingObject(nodeView.node);
        _inspectorView.InspectTargetObject<Editor>(nodeView.node);
    }
}