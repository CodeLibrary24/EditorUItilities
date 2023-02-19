using System.Collections.Generic;
using UnityEngine;

namespace CodeLibrary24.Utilities
{
    public abstract class CustomNode : ScriptableObject
    {
        public string guid;
        public Vector2 graphPosition;
        public string nodeName;

        public string description;
        public List<CustomNode> childrenNodes;

        public CustomNode()
        {
            childrenNodes = new List<CustomNode>();
        }

        public virtual CustomNode Clone()
        {
            CustomNode customNode = Instantiate(this);
            customNode.childrenNodes = childrenNodes.ConvertAll(n => n.Clone()); // TODO: This needs testing and use case - keeping it here for now
            return customNode;
        }
    }
}