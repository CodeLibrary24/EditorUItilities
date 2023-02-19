using System;

namespace CodeLibrary24.EditorUtilities
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class NodeTypeAttribute : Attribute
    {
        public Type NodeType { get; set; }

        public NodeTypeAttribute(Type nodeType)
        {
            NodeType = nodeType;
        }
    }
}