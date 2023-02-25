using CodeLibrary24.Utilities;
using UnityEngine;

namespace CodeLibrary24.EditorUtilities
{
    [NodeType(typeof(DebugCustomNode))]
    public class DebugNodeView : NodeView
    {
        public override bool HasInputPort => true;
        public override bool HasOutputPort => true;
        public override PortCapacityType InputPortCapacityType => PortCapacityType.Single;
        public override PortCapacityType OutputPortCapacityType => PortCapacityType.Single;
        public override Color TitleBackgroundColor => new(0.76f, 0.38f, 0f);

        public DebugNodeView(CustomNode customNode) : base(customNode)
        {
        }
    }
} 