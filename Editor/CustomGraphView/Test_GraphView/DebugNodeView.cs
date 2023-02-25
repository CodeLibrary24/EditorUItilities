using CodeLibrary24.Utilities;

namespace CodeLibrary24.EditorUtilities
{
    [NodeType(typeof(DebugCustomNode))]
    public class DebugNodeView : NodeView
    {
        public override bool HasInputPort => true;
        public override bool HasOutputPort => true;
        public override PortCapacityType InputPortCapacityType => PortCapacityType.Single;
        public override PortCapacityType OutputPortCapacityType => PortCapacityType.Single;

        public DebugNodeView(CustomNode customNode) : base(customNode)
        {
        }
    }
} 