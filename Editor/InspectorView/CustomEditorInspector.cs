using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class CustomEditorInspector : Editor
{
    private Color green = new Color(0.07f, 0.28f, 0.03f);

    protected VisualElement rootVisualElement;

    protected abstract string UXMLPath { get; }


    private void OnEnable()
    {
        rootVisualElement = new VisualElement();
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UXMLPath);
        visualTree.CloneTree(rootVisualElement);
        Initialize();
    }

    public override VisualElement CreateInspectorGUI()
    {
        DrawEditor();
        return rootVisualElement;
    }

    protected virtual void Initialize()
    {
    }

    protected abstract void DrawEditor();

    protected void DrawSaveButton()
    {
        Button saveButton = new Button(() => { Save(); });
        saveButton.text = "Save";
        saveButton.style.backgroundColor = green;
        rootVisualElement.Add(saveButton);
    }

    protected virtual void Save()
    {
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}