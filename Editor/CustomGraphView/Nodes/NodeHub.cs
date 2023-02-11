using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CodeLibrary24.EditorUtilities
{
    public class NodeHub : ScriptableObject
    {
        public List<Node> nodes = new List<Node>();

        public Node CreateNode(Type type)
        {
            Node node = ScriptableObject.CreateInstance(type) as Node;
            if (node != null)
            {
                node.nodeName = type.Name;
                node.name = type.Name;
                node.guid = GUID.Generate().ToString();
                nodes.Add(node);

                AssetDatabase.AddObjectToAsset(node, this);
                AssetDatabase.SaveAssets();
                return node;
            }
            else
            {
                Debug.LogError("ERROR: Node not created");
            }

            return null;
        }

        public void DeleteNode(Node node)
        {
            nodes.Remove(node);
            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
        }
        
        public void AddChild(Node parent, Node child)
        {
            // TODO: Check if this type of node accepts children and only then add child
            parent.childrenNodes.Add(child);
        }

        public void RemoveChild(Node parent, Node child)
        {
            parent.childrenNodes.Remove(child);
        }

        public List<Node> GetChildren(Node parent)
        {
            return parent.childrenNodes;
        }
    }
}