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
                
                Undo.RecordObject(this, "Add node to list Record");
                nodes.Add(node);

                AssetDatabase.AddObjectToAsset(node, this);
                Undo.RegisterCreatedObjectUndo(node,"Create new node Record");
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
            Undo.RecordObject(this, "Delete node from list Record");
            nodes.Remove(node);
            Undo.DestroyObjectImmediate(node);
            // AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
        }
        
        public void AddChild(Node parent, Node child)
        {
            // TODO: Check if this type of node accepts children and only then add child
            Undo.RecordObject(parent,"Node Edge Addition Record");
            parent.childrenNodes.Add(child);
            EditorUtility.SetDirty(parent);
        }

        public void RemoveChild(Node parent, Node child)
        {
            Undo.RecordObject(parent,"Node Edge Removal Record");
            parent.childrenNodes.Remove(child);
            EditorUtility.SetDirty(parent);
        }

        public List<Node> GetChildren(Node parent)
        {
            return parent.childrenNodes;
        }
    }
}