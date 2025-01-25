namespace CodeGraph
{
    [System.Serializable]
    public abstract class InOutNode : CodeGraphNode
    {
        [In]
        public CodeGraphNode InFlow;

        [Out]
        public CodeGraphNode OutFlow;
    }
}
