namespace CodeLibrary24.EditorUtilities
{
    [NodeType(typeof(DummyNode))]
    public class DummyNodeView : NodeView
    {
        public override bool HasInputPort => true;
        public override bool HasOutputPort => true;
        public override PortCapacityType InputPortCapacityType => PortCapacityType.Multi;
        public override PortCapacityType OutputPortCapacityType => PortCapacityType.Multi;

        public DummyNodeView(Node node) : base(node)
        {
        }
    }
}