using System;

namespace CodeGraph
{
    [System.Serializable]
    public class CodeGraphConnection
    {
        public string inputNodeId;
        public string inputFieldName;

        public string outputNodeId;
        public string outputFieldName;
    }
}
