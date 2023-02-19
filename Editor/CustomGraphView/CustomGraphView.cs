using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CodeLibrary24.Utilities;
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

        public Action<NodeView> OnNodeViewSelected;

        private CustomNodeHub _customNodeHub;

        public CustomGraphView()
        {
            Insert(0, new GridBackground());
            LoadStyleSheet();
            AddManipulators();

            Undo.undoRedoPerformed += Refresh;
        }


        public void Refresh()
        {
            PopulateView(_customNodeHub);
            AssetDatabase.SaveAssets();
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

        public void PopulateView(CustomNodeHub customNodeHub)
        {
            if (customNodeHub == null)
            {
                return;
            }

            _customNodeHub = customNodeHub;

            ResetView();
            CreateNodeViews(customNodeHub);
            CreateEdges(customNodeHub);
        }

        private void CreateNodeViews(CustomNodeHub customNodeHub)
        {
            foreach (CustomNode node in customNodeHub.nodes)
            {
                CreateNodeView(node);
            }
        }

        private void CreateEdges(CustomNodeHub customNodeHub)
        {
            foreach (CustomNode node in customNodeHub.nodes)
            {
                List<CustomNode> children = customNodeHub.GetChildrenNodes(node);
                foreach (CustomNode childNode in children)
                {
                    NodeView parentNodeView = FindNodeView(node);
                    NodeView childNodeView = FindNodeView(childNode);

                    Edge edge = parentNodeView.outputPort.ConnectTo(childNodeView.inputPort);
                    AddElement(edge);
                }
            }
        }

        private NodeView FindNodeView(CustomNode customNode)
        {
            return GetNodeByGuid(customNode.guid) as NodeView;
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
                    _customNodeHub.DeleteNode(nodeView.customNode);
                }

                if (element is Edge edge)
                {
                    RemoveNode(edge);
                }
            }
        }

        private void RemoveNode(Edge edge)
        {
            NodeView parentView = edge.output.node as NodeView;
            NodeView childView = edge.input.node as NodeView;
            _customNodeHub.RemoveChildNode(parentView.customNode, childView.customNode);
        }

        private void AddEgdesAsChildren(GraphViewChange graphviewchange)
        {
            if (graphviewchange.edgesToCreate != null)
            {
                graphviewchange.edgesToCreate.ForEach(edge =>
                {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;
                    _customNodeHub.AddChildNode(parentView.customNode, childView.customNode);
                });
            }
        }

        private void CreateNodeView(CustomNode customNode)
        {
            Type viewType = null;

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(typeof(NodeView)) &&
                        type.GetCustomAttributes(typeof(NodeTypeAttribute), false)
                            .Cast<NodeTypeAttribute>()
                            .Any(attr => attr.NodeType == customNode.GetType()))
                    {
                        viewType = type;
                        break;
                    }
                }

                if (viewType != null)
                {
                    break;
                }
            }

            if (viewType != null)
            {
                NodeView nodeView = (NodeView) Activator.CreateInstance(viewType, new object[] {customNode}); // TODO: Pass constructor arguments to Node View base class here
                AddElement(nodeView);
                nodeView.OnNodeViewSelected = OnNodeViewSelected;
                OnNodeViewSelected?.Invoke(nodeView);
            }
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            if (_customNodeHub == null)
            {
                return;
            }

            var types = TypeCache.GetTypesDerivedFrom<CustomNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction("Create Node/" + type.Name, (a) => CreateNode(type));
            }
        }

        private void CreateNode(Type type)
        {
            CustomNode customNode = _customNodeHub.CreateNode(type);
            CreateNodeView(customNode);
        }
    }
}