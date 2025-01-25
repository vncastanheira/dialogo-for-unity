using UnityEngine;

namespace CodeGraph
{
    [NodeInfo("Debug", "Dialogo/Debug")]
    public class DebugNode : CodeGraphNode
    {
        [SerializeField, Expose]
        public int exposedInt;

        [SerializeField, Expose]
        public string exposedString;
    }
}
