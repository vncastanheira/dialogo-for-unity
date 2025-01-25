using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

namespace CodeGraph
{
    [System.Serializable]
    public abstract class CodeGraphNode 
    {
        [SerializeField]
        private string m_guid;
        [SerializeField]
        private Rect m_position;
        public string typeName;

        public string Id => m_guid;
        public Rect position => m_position;

        public CodeGraphNode()
        {
            m_guid = Guid.NewGuid().ToString();
            
            Debug.Log($"Created Code Graph Node {{{m_guid}}}");
        }

        public void SetPosition(Rect position) => m_position = position;
    }

    [System.Serializable]
    public struct PortData
    {
        public Guid guid;

        public static bool operator ==(PortData p1, PortData p2) => p1.guid == p2.guid;
        public static bool operator !=(PortData p1, PortData p2) => !(p1 == p2);

        public override bool Equals(object obj) => obj is PortData ? this == (PortData)obj : false;
        public override int GetHashCode() => base.GetHashCode();
    }
}
