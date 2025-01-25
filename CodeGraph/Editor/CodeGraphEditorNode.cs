using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Experimental.GraphView;

namespace CodeGraph.Editor
{
    public class CodeGraphEditorNode : Node
    {
        private CodeGraphNode m_graphNode;
        public CodeGraphNode GraphNode => m_graphNode;

        private List<PortEditorData> m_fieldPorts;
        public List<PortEditorData> FieldPorts => m_fieldPorts;

        public CodeGraphEditorNode(CodeGraphNode node)
        {
            AddToClassList("code-graph-node");

            m_graphNode = node;
            var typeInfo = node.GetType();
            var info = typeInfo.GetCustomAttribute<NodeInfoAttribute>();
            title = info.Title;

            var depths = info.MenuItem.Split('/');
            foreach (var depth in depths)
                AddToClassList(depth.ToLower().Replace(' ', '-'));

            name = typeInfo.Name;

            m_fieldPorts = new();
        }

        public void SavePosition()
        {
            m_graphNode.SetPosition(GetPosition());
        }

        public void InsertPort(string fieldName, Port port)
        {
            FieldPorts.Add(new PortEditorData 
            { 
                fieldName = fieldName, 
                port = port
            });
        }
    }

    public struct PortEditorData
    {
        public string fieldName;
        public Port port;
    }
}
