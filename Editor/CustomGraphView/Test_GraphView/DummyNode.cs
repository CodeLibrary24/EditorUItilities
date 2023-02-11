
namespace CodeLibrary24.EditorUtilities
{
    public class DummyNode : Node
    {
        public override bool HasInputPort => true;
        public override bool HasOutputPort => true;
        public override PortCapacityType InputPortCapacityType => PortCapacityType.Multi;
        public override PortCapacityType OutputPortCapacityType => PortCapacityType.Multi;
    }
}