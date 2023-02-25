using System;
using System.Collections.Generic;
using CodeLibrary24.Utilities;
using UnityEditor;
using UnityEngine;

namespace CodeLibrary24.EditorUtilities
{
    public class CustomNodeHubController
    {
        private CustomNodeHub _customNodeHub;

        public CustomNodeHubController(CustomNodeHub customNodeHub)
        {
            _customNodeHub = customNodeHub;
        }

        public CustomNode CreateNode(Type type)
        {
            CustomNode customNode = ScriptableObject.CreateInstance(type) as CustomNode;
            if (customNode != null)
            {
                customNode.nodeName = type.Name;
                customNode.name = type.Name;
                customNode.guid = GUID.Generate().ToString();

                Undo.RecordObject(_customNodeHub, "Add node to list Record");
                _customNodeHub.nodes.Add(customNode);

                AssetDatabase.AddObjectToAsset(customNode, _customNodeHub);
                Undo.RegisterCreatedObjectUndo(customNode, "Create new node Record");
                AssetDatabase.SaveAssets();
                return customNode;
            }
            else
            {
                Debug.LogError("ERROR: Node not created");
            }

            return null;
        }

        public void DeleteNode(CustomNode customNode)
        {
            Undo.RecordObject(_customNodeHub, "Delete node from list Record");
            _customNodeHub.nodes.Remove(customNode);
            Undo.DestroyObjectImmediate(customNode);
            // AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
        }

        public void AddChildNode(CustomNode parent, CustomNode child)
        {
            // TODO: Check if this type of node accepts children and only then add child
            Undo.RecordObject(parent, "Node Edge Addition Record");
            parent.childrenNodes.Add(child);
            EditorUtility.SetDirty(parent);
        }

        public void RemoveChildNode(CustomNode parent, CustomNode child)
        {
            Undo.RecordObject(parent, "Node Edge Removal Record");
            parent.childrenNodes.Remove(child);
            EditorUtility.SetDirty(parent);
        }

        public List<CustomNode> GetChildrenNodes(CustomNode parent)
        {
            return parent.childrenNodes;
        }

        public CustomNodeHub GetNodeHub()
        {
            return _customNodeHub;
        }
    }
}