using System;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace CodeLibrary24.EditorUtilities
{
    public class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        public Node node;
        public Port inputPort;
        public Port outputPort;

        public Action<NodeView> OnNodeViewSelected;

        public NodeView(Node node)
        {
            if (node == null)
            {
                return;
            }

            this.node = node;
            title = node.nodeName;
            viewDataKey = node.guid;
            SetNodePositionInGraph();

            CreateInputPorts();
            CreateOutputPorts();
        }

        private void CreateInputPorts()
        {
            if (!node.hasInputPort)
            {
                return;
            }

            inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, GetPortCapacity(node.inputPortCapacityType), typeof(bool));
            if (inputPort != null)
            {
                inputPort.portName = String.Empty;
                inputContainer.Add(inputPort);
            }
        }

        private void CreateOutputPorts()
        {
            if (!node.hasOutputPort)
            {
                return;
            }

            outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, GetPortCapacity(node.outputPortCapacityType), typeof(bool));
            if (outputPort != null)
            {
                outputPort.portName = String.Empty;
                outputContainer.Add(outputPort);
            }
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
            style.left = node.graphPosition.x;
            style.top = node.graphPosition.y;
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            Undo.RecordObject(node, "Node Position Record");
            node.graphPosition.x = newPos.xMin;
            node.graphPosition.y = newPos.yMin;
            EditorUtility.SetDirty(node);
        }

        public override void OnSelected()
        {
            base.OnSelected();
            OnNodeViewSelected?.Invoke(this);
        }
    }
}