using System;

namespace CodeGraph
{
    [AttributeUsage(AttributeTargets.Field)]
    public class OutAttribute : Attribute
    {
        public string Name { get; private set; }

        public OutAttribute(string name = "Flow Exit")
        {
            Name = name;
        }
    }
}
