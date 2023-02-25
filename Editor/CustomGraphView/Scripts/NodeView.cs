using System;
using CodeLibrary24.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace CodeLibrary24.EditorUtilities
{
    public abstract class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        public abstract bool HasInputPort { get; }
        public abstract bool HasOutputPort { get; }
        public abstract PortCapacityType InputPortCapacityType { get; }
        public abstract PortCapacityType OutputPortCapacityType { get; }

        public abstract Color TitleBackgroundColor { get; }

        public CustomNode customNode;
        public Port inputPort;
        public Port outputPort;

        public Action<NodeView> OnNodeViewSelected;

        private VisualElement _customDataContainer;

        public NodeView(CustomNode customNode) : base(EditorPaths.NodeViewUxml)
        {
            if (customNode == null)
            {
                return;
            }

            this.customNode = customNode;
            title = customNode.nodeName;
            viewDataKey = customNode.guid;
            SetNodePositionInGraph();

            CreateInputPorts();
            CreateOutputPorts();
            SetTitleColor();

            _customDataContainer = mainContainer.Q<VisualElement>("CustomDataContainer");
        }

        private void CreateInputPorts()
        {
            if (!HasInputPort)
            {
                return;
            }

            inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, GetPortCapacity(InputPortCapacityType), typeof(bool));
            if (inputPort != null)
            {
                inputPort.portName = String.Empty;
                inputContainer.Add(inputPort);
            }
        }

        private void CreateOutputPorts()
        {
            if (!HasOutputPort)
            {
                return;
            }

            outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, GetPortCapacity(OutputPortCapacityType), typeof(bool));
            if (outputPort != null)
            {
                outputPort.portName = String.Empty;
                outputContainer.Add(outputPort);
            }
        }

        private void SetTitleColor()
        {
            titleContainer.style.backgroundColor = TitleBackgroundColor;
        }

        public void Draw()
        {
            DrawCustomData(_customDataContainer);
        }

        protected virtual void DrawCustomData(VisualElement customDataContainer)
        {
            Label descriptionLabel = new Label(customNode.description);
            descriptionLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
            descriptionLabel.style.whiteSpace = WhiteSpace.Normal;
            customDataContainer.Add(descriptionLabel);
        }

        private Port.Capacity GetPortCapacity(PortCapacityType portCapacityType)
        {
            switch (portCapacityType)
            {
                case PortCapacityType.Single:
                    return Port.Capacity.Single;
                case PortCapacityType.Multi:
                    return Port.Capacity.Multi;
            }

            return Port.Capacity.Single;
        }

        private void SetNodePositionInGraph()
        {
            style.left = customNode.graphPosition.x;
            style.top = customNode.graphPosition.y;
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            Undo.RecordObject(customNode, "Node Position Record");
            customNode.graphPosition.x = newPos.xMin;
            customNode.graphPosition.y = newPos.yMin;
            EditorUtility.SetDirty(customNode);
        }

        public override void OnSelected()
        {
            base.OnSelected();
            OnNodeViewSelected?.Invoke(this);
        }
    }
}