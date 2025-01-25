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
    public class CodeGraphWindowSearchProvider : ScriptableObject, ISearchWindowProvider
    {
        public CodeGraphView graph;
        public VisualElement target;

        public static List<SearchContextElement> elements;

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry>();
            tree.Add(new SearchTreeGroupEntry(new GUIContent("Nodes"), 0));

            elements = new List<SearchContextElement>();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.CustomAttributes.ToList() != null)
                    {
                        var attribute = type.GetCustomAttribute<NodeInfoAttribute>();
                        if (attribute != null)
                        {
                            var node = Activator.CreateInstance(type);
                            if (string.IsNullOrEmpty(attribute.MenuItem)) continue;

                            elements.Add(new SearchContextElement(node, attribute.MenuItem));
                        }
                    }
                }
            }

            // Sort by name
            elements.Sort((entry1, entry2) =>
            {
                var splits1 = entry1.title.Split('/');
                var splits2 = entry2.title.Split('/');
                for (int i = 0; i < splits1.Length; i++)
                {
                    if (i >= splits2.Length)
                        return i;

                    int value = splits1[i].CompareTo(splits2[i]);
                    if (value != 0)
                    {
                        // make sure that leaves go before nodes
                        if (splits1.Length != splits2.Length
                        && (i == splits1.Length - 1 || i == splits2.Length - 1))
                            return splits1.Length < splits2.Length ? 1 : -1;

                        return value;
                    }
                }

                return 0;
            });

            var groups = new List<string>();

            foreach (var element in elements)
            {
                var entryTitle = element.title.Split("/");
                var groupName = "";

                for (int i = 0; i < entryTitle.Length - 1; i++)
                {
                    groupName += entryTitle[i];
                    if (!groups.Contains(groupName))
                    {
                        tree.Add(new SearchTreeGroupEntry(new GUIContent(entryTitle[i]), i + 1));
                        groups.Add(groupName);
                    }
                    groupName += "/";
                }

                var entry = new SearchTreeEntry(new GUIContent(entryTitle.Last()))
                {
                    level = entryTitle.Length,
                    userData = new SearchContextElement(element.target, element.title)
                };
                tree.Add(entry);
            }

            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            var windowMousePosition = graph.ChangeCoordinatesTo(graph, context.screenMousePosition - graph.Window.position.position);
            var graphMousePosition = graph.contentViewContainer.WorldToLocal(windowMousePosition);

            var element = (SearchContextElement)searchTreeEntry.userData;

            var node = (CodeGraphNode)element.target;
            node.SetPosition(new Rect(graphMousePosition, new Vector2()));
            graph.AddNode(node);

            return true;
        }
    }

    public struct SearchContextElement
    {
        public object target { get; private set; }
        public string title { get; private set; }

        public SearchContextElement(object target, string title)
        {
            this.target = target;
            this.title = title;
        }
    }
}
