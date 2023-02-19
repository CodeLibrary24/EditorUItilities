using CodeLibrary24.Utilities;

namespace CodeLibrary24.EditorUtilities
{
    [NodeType(typeof(DummyCustomNode))]
    public class DummyNodeView : NodeView
    {
        public override bool HasInputPort => true;
        public override bool HasOutputPort => true;
        public override PortCapacityType InputPortCapacityType => PortCapacityType.Multi;
        public override PortCapacityType OutputPortCapacityType => PortCapacityType.Multi;

        public DummyNodeView(CustomNode customNode) : base(customNode)
        {
        }
    }
}