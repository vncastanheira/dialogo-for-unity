using UnityEditor;
using UnityEngine;

namespace CodeGraph.Editor
{
    [CustomEditor(typeof(CodeGraphAsset))]
    public class CodeGraphAssetEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open"))
            {
                CodeGraphEditorWindow.OpenWindow(target as CodeGraphAsset);
            }
        }
    }
}
