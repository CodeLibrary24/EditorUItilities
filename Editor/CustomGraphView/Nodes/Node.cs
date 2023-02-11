using UnityEngine;

namespace CodeLibrary24.EditorUtilities
{
    public abstract class Node : ScriptableObject
    {
        public string nodeName;
        public string guid;
        public Vector2 graphPosition;  
    }
}