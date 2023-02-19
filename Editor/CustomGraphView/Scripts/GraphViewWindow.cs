using CodeLibrary24.EditorUtilities;
using CodeLibrary24.Utilities;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphViewWindow : EditorWindow
{
    private static readonly Vector2 MinWindowSize = new Vector2(1400, 800);
    private static CustomGraphView _graphView;
    private CustomInspectorView _inspectorView;
    private static CustomNodeHub _selectedCustomNodeHub;
    private static NodeHubBarController _nodeHubBarController;

    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("CodeLibrary24/EditorUtilities/Tests/Graph View")]
    public static void OpenWindow()
    {
        GraphViewWindow window = GetWindow<GraphViewWindow>();
        window.minSize = MinWindowSize;
        window.titleContent = new GUIContent("GraphViewTestWindow");
    }   

    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        CustomNodeHub customNodeHub = Selection.activeObject as CustomNodeHub;
        
        if (customNodeHub)
        {
            OpenWindow();
            OpenToolFromNodeHub(customNodeHub);
            return true;
        }

        return false;
    }

    private static void OpenToolFromNodeHub(CustomNodeHub customNodeHub)
    {
        _selectedCustomNodeHub = customNodeHub;
        _nodeHubBarController.ForceSelectNodeHub(customNodeHub);
        OnNodeHubSelected(customNodeHub);
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

    private static void OnNodeHubSelected(CustomNodeHub selectedCustomNodeHub)
    {
        _graphView.PopulateView(selectedCustomNodeHub);
    }

    private void OnNodeSelectionChanged(NodeView nodeView)
    {
        EditorGUIUtility.PingObject(nodeView.customNode);
        _inspectorView.InspectTargetObject<Editor>(nodeView.customNode);
    }
}