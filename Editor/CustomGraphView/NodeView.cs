using System;
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
            this.node = node;
            title = node.nodeName;
            viewDataKey = node.guid;
            SetNodePositionInGraph();

            CreateInputPorts();
            CreateOutputPorts();
        }

        private void CreateInputPorts()
        {
            inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
            if (inputPort != null)
            {
                inputPort.portName = String.Empty;
                inputContainer.Add(inputPort);
            }
        }

        private void CreateOutputPorts()
        {       
            outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            if (outputPort != null)
            {
                outputPort.portName = String.Empty;
                outputContainer.Add(outputPort);
            }
        }

        private void SetNodePositionInGraph()
        {
            style.left = node.graphPosition.x;
            style.top = node.graphPosition.y;
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            node.graphPosition.x = newPos.xMin;
            node.graphPosition.y = newPos.yMin;
        }

        public override void OnSelected()
        {
            base.OnSelected();
            OnNodeViewSelected?.Invoke(this);
        }
    }
}