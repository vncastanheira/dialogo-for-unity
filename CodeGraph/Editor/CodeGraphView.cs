using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace CodeGraph.Editor
{
    public class CodeGraphView : GraphView
    {
        private CodeGraphAsset m_codeGraph;
        private SerializedObject m_serializedObject;
        private CodeGraphEditorWindow m_window;

        public CodeGraphEditorWindow Window => m_window;
        public List<CodeGraphEditorNode> GraphEditorNodes => m_graphNodes;

        private List<CodeGraphEditorNode> m_graphNodes { get; set; }
        public Dictionary<string, CodeGraphEditorNode> m_nodeDictionary;

        private CodeGraphWindowSearchProvider m_searchProvider;

        public CodeGraphView(SerializedObject serializedObject, CodeGraphEditorWindow window)
        {
            m_serializedObject = serializedObject;
            m_codeGraph = serializedObject.targetObject as CodeGraphAsset;
            m_window = window;

            m_graphNodes = new List<CodeGraphEditorNode>();
            m_nodeDictionary = new Dictionary<string, CodeGraphEditorNode>();

            m_searchProvider = ScriptableObject.CreateInstance<CodeGraphWindowSearchProvider>();
            m_searchProvider.graph = this;
            nodeCreationRequest = ShowSearchWindow;

            var styleAsset = Resources.Load<StyleSheet>("RpgGraphViewUSS");
            styleSheets.Add(styleAsset);
            var background = new GridBackground
            {
                name = "Grid"
            };
            Add(background);
            background.SendToBack();

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ClickSelector());

            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale * 2f);

            DrawNodes();
            DrawConnections();

            graphViewChanged += OnGraphViewChangedEvent;
        }

        private Port GeneratePort(CodeGraphEditorNode editorNode, Direction direction, Port.Capacity capacity = Port.Capacity.Single)
        {
            return editorNode.InstantiatePort(Orientation.Horizontal, direction, capacity, typeof(CodeGraphNode)) as Port;
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.Where(p => (startPort != p
                && startPort.node != p.node
                && startPort.direction != p.direction))
                .ToList();
        }

        private void ShowSearchWindow(NodeCreationContext obj)
        {
            m_searchProvider.target = focusController.focusedElement as VisualElement;
            SearchWindow.Open(new SearchWindowContext(obj.screenMousePosition), m_searchProvider);
        }

        private void DrawNodes()
        {
            foreach (var node in m_codeGraph.Nodes)
            {
                AddNodeToGraph(node);
            }
        }

        private void DrawConnections()
        {
            foreach (var connection in m_codeGraph.Connections)
            {
                var inputEditorNode = GraphEditorNodes.Single(ed => ed.GraphNode.Id == connection.inputNodeId);
                var outputEditorNode = GraphEditorNodes.Single(ed => ed.GraphNode.Id == connection.outputNodeId);

                var inputPortData = inputEditorNode.FieldPorts.Single(p => p.fieldName == connection.inputFieldName);
                var outputPortData = outputEditorNode.FieldPorts.Single(p => p.fieldName == connection.outputFieldName);

                var edge = inputPortData.port.ConnectTo(outputPortData.port);
                AddElement(edge);
            }
        }

        public void Add(CodeGraphNode node)
        {
            Undo.RecordObject(m_serializedObject.targetObject, "Added Node");
            m_codeGraph.Nodes.Add(node);
            m_serializedObject.Update();

            AddNodeToGraph(node);
        }

        private void AddNodeToGraph(CodeGraphNode node)
        {
            node.typeName = node.GetType().AssemblyQualifiedName;

            var editorNode = new CodeGraphEditorNode(node);
            editorNode.SetPosition(node.position);

            var fieldInfos = node.GetType().GetFields();
            int index = 0;
            foreach (var fi in fieldInfos)
            {
                var outAttribute = fi.GetCustomAttribute<OutAttribute>();
                if (outAttribute != null)
                {
                    var outputPort = GeneratePort(editorNode, Direction.Output);
                    outputPort.AddToClassList("flow-output");
                    outputPort.portName = outAttribute.Name;
                    editorNode.outputContainer.Add(outputPort);

                    editorNode.InsertPort(fi.Name, outputPort);
                }

                var inAttribute = fi.GetCustomAttribute<InAttribute>();
                if (inAttribute != null)
                {
                    var inputPort = GeneratePort(editorNode, Direction.Input);
                    inputPort.AddToClassList("flow-input");
                    inputPort.portName = inAttribute.Name;
                    editorNode.inputContainer.Add(inputPort);

                    editorNode.InsertPort(fi.Name, inputPort);
                }
            }

            m_graphNodes.Add(editorNode);
            m_nodeDictionary.Add(node.Id, editorNode);

            AddElement(editorNode);

            editorNode.RefreshExpandedState();
            editorNode.RefreshPorts();
        }

        private GraphViewChange OnGraphViewChangedEvent(GraphViewChange graphViewChange)
        {
            if (graphViewChange.movedElements != null)
            {
                Undo.RecordObject(m_serializedObject.targetObject, "Moved Elements");
            }

            if (graphViewChange.elementsToRemove != null)
            {
                var nodes = graphViewChange.elementsToRemove.OfType<CodeGraphEditorNode>().ToList();
                if (nodes.Count > 0)
                {
                    Undo.RecordObject(m_serializedObject.targetObject, "Remove Node");
                    for (int i = nodes.Count - 1; i >= 0; i--)
                    {
                        m_codeGraph.Nodes.Remove(nodes[i].GraphNode);
                        m_nodeDictionary.Remove(nodes[i].GraphNode.Id);
                        m_graphNodes.Remove(nodes[i]);
                        m_serializedObject.Update();
                    }
                }
            }

            if (graphViewChange.edgesToCreate != null)
            {
                Undo.RecordObject(m_serializedObject.targetObject, "Added Connections");
                foreach (var edge in graphViewChange.edgesToCreate)
                {
                    var inputNodeEditor = edge.input.node as CodeGraphEditorNode;
                    var outputNodeEditor = edge.output.node as CodeGraphEditorNode;

                    var inputEditorPortData = inputNodeEditor.FieldPorts.Single(p => p.port == edge.input);
                    var outputEditorPortData = outputNodeEditor.FieldPorts.Single(p => p.port == edge.output);

                    var connection = new CodeGraphConnection
                    {
                        inputNodeId = inputNodeEditor.GraphNode.Id,
                        inputFieldName = inputEditorPortData.fieldName,
                        outputNodeId = outputNodeEditor.GraphNode.Id,
                        outputFieldName = outputEditorPortData.fieldName
                    };
                    m_codeGraph.Connections.Add(connection);
                }
            }

            return graphViewChange;
        }

    }
}
