using UnityEngine;

namespace CodeLibrary24.EditorUtilities
{
    public class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        public Node node;

        public NodeView(Node node)
        {
            this.node = node;
            this.title = node.nodeName;
            this.viewDataKey = node.guid;
            SetNodePositionInGraph();
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
    }
}