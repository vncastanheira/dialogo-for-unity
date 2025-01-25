using System.Collections.Generic;
using UnityEngine;

namespace CodeGraph
{
    public abstract class CodeGraphAsset : ScriptableObject
    {
        [SerializeReference]
        private List<CodeGraphNode> nodes;
        [SerializeReference]
        private List<CodeGraphConnection> connections;

        public List<CodeGraphNode> Nodes => nodes;
        public List<CodeGraphConnection> Connections => connections;

        public CodeGraphAsset()
        {
            nodes = new();
            connections = new();
        }
    }
}
