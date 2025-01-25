namespace CodeGraph
{
    [System.Serializable]
    public abstract class OutNode : CodeGraphNode
    {
        [Out]
        public CodeGraphNode OutFlow;
    }
}
