#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UIElements;

namespace CodeGraph
{
    public interface INodeProperty 
    {
#if UNITY_EDITOR
        public VisualElement Draw(SerializedProperty serializedProperty);
#endif
    }
}
