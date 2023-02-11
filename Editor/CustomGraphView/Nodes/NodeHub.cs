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
    }
}