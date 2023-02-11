using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
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

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();
        }

        public void ResetView()
        {
            graphViewChanged -= OnGraphViewChanged;

            DeleteElements(graphElements);

            graphViewChanged += OnGraphViewChanged;
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphviewchange)
        {
            ClearGraphView(graphviewchange);
            AddEgdesAsChildren(graphviewchange);

            return graphviewchange;
        }

        private void ClearGraphView(GraphViewChange graphviewchange)
        {
            List<GraphElement> elementsToRemove = graphviewchange.elementsToRemove;

            if (elementsToRemove == null)
            {
                return;
            }

            foreach (GraphElement element in elementsToRemove)
            {
                if (element is NodeView nodeView)
                {
                    _nodeHub.DeleteNode(nodeView.node);
                }
            }
        }

        private void AddEgdesAsChildren(GraphViewChange graphviewchange)
        {
            if (graphviewchange.edgesToCreate != null)
            {
                graphviewchange.edgesToCreate.ForEach(edge =>
                {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;
                    AddChild(parentView.node, childView.node);
                });
            }
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

        public void AddChild(Node parent, Node child)
        {
            // TODO: Check if this type of node accepts children and only then add child
            parent.childrenNodes.Add(child);
        }

        private void RemoveChild(Node parent, Node child)
        {
            parent.childrenNodes.Remove(child);
        }

        private List<Node> GetChildren(Node parent)
        {
            return parent.childrenNodes;
        }
    }
}