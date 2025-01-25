using CodeGraph;
using UnityEngine;

namespace DialogueGraph
{
    [NodeInfo("Dialogue", "RPG/Dialogue")]
    public class DialogueNode : InOutNode
    {
        [SerializeField]
        public string dialogue;

        public DialogueNode()
            :base()
        {
            dialogue = string.Empty;
        }
    }
}
