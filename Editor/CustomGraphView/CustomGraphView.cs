using CodeLibrary24.EditorUtilities;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace CodeLibrary24.EditorUtilities
{
    public class CustomGraphView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<CustomGraphView, UxmlTraits>
        {
        }

        public CustomGraphView()
        {
            Insert(0, new GridBackground());
            LoadStyleSheet();
            AddManipulators();
        }

        private void LoadStyleSheet()
        {
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(EditorPaths.GraphViewStyleSheet);
            styleSheets.Add(styleSheet);
        }

        private void AddManipulators()
        {
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new SelectionDragger());
        }
    }
}