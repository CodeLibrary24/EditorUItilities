using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeLibrary24.EditorUtilities
{
    public abstract class Node : ScriptableObject
    {
        [Header("Node Properties")]
        public string guid;

        public abstract bool HasInputPort { get; }
        public abstract bool HasOutputPort { get; }
        public abstract PortCapacityType InputPortCapacityType { get; }
        public abstract PortCapacityType OutputPortCapacityType { get; }

        public Vector2 graphPosition;

        [Space]
        [Header("Data")]
        public string nodeName;

        public string description;
        public List<Node> childrenNodes;

        public Node()
        {
            childrenNodes = new List<Node>();
        }

        public virtual Node Clone()
        {
            Node node = Instantiate(this);
            node.childrenNodes = childrenNodes.ConvertAll(n => n.Clone()); // TODO: This needs testing and use case - keeping it here for now
            return node;
        }
    }
}