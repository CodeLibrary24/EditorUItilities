namespace CodeLibrary24.EditorUtilities
{
    [NodeType(typeof(DebugNode))]
    public class DebugNodeView : NodeView
    {
        public override bool HasInputPort => true;
        public override bool HasOutputPort => false;
        public override PortCapacityType InputPortCapacityType => PortCapacityType.Single;
        public override PortCapacityType OutputPortCapacityType => PortCapacityType.Single;

        public DebugNodeView(Node node) : base(node)
        {
        }
    }
}