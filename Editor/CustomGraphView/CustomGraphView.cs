using System;
using System.Collections.Generic;
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

        private NodeHub _nodeHub;

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
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
        }

        public void PopulateView(NodeHub nodeHub)
        {
            if (nodeHub == null)
            {
                return;
            }
            _nodeHub = nodeHub;

            ResetView();

            foreach (Node node in nodeHub.nodes)
            {
                CreateNodeView(node);
            }
        }

        public void ResetView()
        {
            graphViewChanged -= OnGraphViewChanged;

            DeleteElements(graphElements);

            graphViewChanged += OnGraphViewChanged;
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphviewchange)
        {
            List<GraphElement> elementsToRemove = graphviewchange.elementsToRemove;

            if (elementsToRemove == null) return graphviewchange;

            foreach (GraphElement element in elementsToRemove)
            {
                if (element is NodeView nodeView)
                {
                    _nodeHub.DeleteNode(nodeView.node);
                }
            }

            return graphviewchange;
        }

        private void CreateNodeView(Node node)
        {
            NodeView nodeView = new NodeView(node);
            AddElement(nodeView);
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            if (_nodeHub == null)
            {
                return;
            }
            var types = TypeCache.GetTypesDerivedFrom<Node>();
            foreach (var type in types)
            {
                evt.menu.AppendAction("Create Node/" + type.Name, (a) => CreateNode(type));
            }
        }

        private void CreateNode(Type type)
        {
            Node node = _nodeHub.CreateNode(type);
            CreateNodeView(node);
        }
    }
}