using UnityEditor;
using UnityEngine.UIElements;

namespace CodeLibrary24.EditorUtilities
{
    public abstract class CustomEditorInspector : Editor
    {

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
            Button saveButton = new Button(Save)
            {
                text = "Save", style =
                {
                    backgroundColor = EditorConstants.Green
                }
            };
            RootVisualElement.Add(saveButton);
        }

        protected virtual void Save()
        {
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}