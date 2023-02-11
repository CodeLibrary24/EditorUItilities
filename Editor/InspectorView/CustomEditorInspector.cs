using UnityEditor;
using UnityEngine.UIElements;

namespace CodeLibrary24.EditorUtilities
{
    public abstract class CustomEditorInspector : Editor
    {

        protected VisualElement rootVisualElement;

        protected abstract string UxmlPath { get; }

        private void OnEnable()
        {
            rootVisualElement = new VisualElement();
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UxmlPath);
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
            Button saveButton = new Button(Save)
            {
                text = "Save", style =
                {
                    backgroundColor = EditorConstants.Green
                }
            };
            rootVisualElement.Add(saveButton);
        }

        protected virtual void Save()
        {
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}