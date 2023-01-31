using CodeLibrary24.EditorUtilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphViewTestWindow : EditorWindow
{
    private CustomGraphView _graphView;
    private CustomInspectorView _inspectorView;

    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("CodeLibrary24/Tests/Graph View")]
    public static void ShowExample()
    {
        GraphViewTestWindow wnd = GetWindow<GraphViewTestWindow>();
        wnd.titleContent = new GUIContent("GraphViewTestWindow");
    }

    public void CreateGUI()
    {
        m_VisualTreeAsset.CloneTree(rootVisualElement);
        _graphView = rootVisualElement.Q<CustomGraphView>();
        _inspectorView = rootVisualElement.Q<CustomInspectorView>();
        OnSelectionChange();
    }

    private void OnSelectionChange()
    {
        NodeHub nodeHub = Selection.activeObject as NodeHub;
        if (nodeHub)
        {
            _graphView.PopulateView(nodeHub);
        }
    }
}