namespace CodeGraph
{
    [System.Serializable]
    public abstract class InNode : CodeGraphNode
    {
        [In]
        public CodeGraphNode InFlow;
    }
}
