using System.Collections.Generic;
using UnityEngine;

namespace CodeLibrary24.EditorUtilities
{
    public abstract class Node : ScriptableObject
    {
        public string nodeName;
        public string guid;
        public Vector2 graphPosition;
        public List<Node> childrenNodes;

        public Node()
        {
            childrenNodes = new List<Node>();
        }
    }
}