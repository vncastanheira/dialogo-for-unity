using System;

namespace CodeGraph
{
    public class InAttribute : Attribute
    {
        public string Name { get; private set; }

        public InAttribute(string name = "Flow Start")
        {
            Name = name;
        }
    }
}
