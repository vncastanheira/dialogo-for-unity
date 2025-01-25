using CodeGraph;
using UnityEngine;

namespace DialogueGraph
{
    [NodeInfo("Dialogue", "RPG/Dialogue")]
    public class DialogueNode : InOutNode
    {
        [SerializeField, Expose]
        public LocalizedTextGroup localizedTextGroup;

        public DialogueNode()
            :base()
        {
            localizedTextGroup = new();
        }
    }
}
