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
    private NodeHub _selectedNodeHub;
    private NodeHubBarController _nodeHubBarController;

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
        if (Selection.activeObject is NodeHub)
        {
            OpenWindow();
            OnNodeHubSelected(Selection.activeObject as NodeHub);
            return true;
        }

        return false;
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
        _nodeHubBarController = new NodeHubBarController(rootVisualElement, OnNodeHubSelected);

        _graphView.OnNodeViewSelected = OnNodeSelectionChanged;
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