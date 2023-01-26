using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class CustomEditorInspector : Editor
{
    private readonly Color _green = new Color(0.07f, 0.28f, 0.03f);

    protected VisualElement RootVisualElement;

    protected abstract string UxmlPath { get; }


    private void OnEnable()
    {
        RootVisualElement = new VisualElement();
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UxmlPath);
        visualTree.CloneTree(RootVisualElement);
        Initialize();
    }

    public override VisualElement CreateInspectorGUI()
    {
        DrawEditor();
        return RootVisualElement;
    }

    protected virtual void Initialize()
    {
    }

    protected abstract void DrawEditor();

    protected void DrawSaveButton()
    {
        Button saveButton = new Button(() => { Save(); });
        saveButton.text = "Save";
        saveButton.style.backgroundColor = _green;
        RootVisualElement.Add(saveButton);
    }

    protected virtual void Save()
    {
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}