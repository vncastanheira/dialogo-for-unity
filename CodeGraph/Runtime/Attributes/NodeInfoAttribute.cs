using System;

namespace CodeGraph
{
    public class NodeInfoAttribute : Attribute
    {
        private string m_nodeTitle;
        private string m_menuItem;

        public string Title => m_nodeTitle;
        public string MenuItem => m_menuItem;

        public NodeInfoAttribute(string title, string menuItem = "")
        {
            m_nodeTitle = title;
            m_menuItem = menuItem;
        }
    }
}
