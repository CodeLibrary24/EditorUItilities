using System.Collections.Generic;
using UnityEngine;

namespace CodeLibrary24.EditorUtilities
{
    public abstract class Node : ScriptableObject
    {
        [Header("Node Properties")]
        public string guid;

        public bool hasInputPort;
        public bool hasOutputPort;
        public PortCapacityType inputPortCapacityType;
        public PortCapacityType outputPortCapacityType;

        public Vector2 graphPosition;
        public List<Node> childrenNodes;

        [Space]
        [Header("Data")]
        public string nodeName;
        public string description;
        
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

    public enum PortCapacityType
    {
        Single
        , Multi
    }
}