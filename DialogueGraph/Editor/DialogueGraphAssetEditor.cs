using CodeGraph;
using CodeGraph.Editor;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace DialogueGraph.Editor
{
    [CustomEditor(typeof(DialogueGraphAsset))]
    public class DialogueGraphAssetEditor : UnityEditor.Editor
    {
        // Open asset on double click
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceId, int index)
        {
            Object asset = EditorUtility.InstanceIDToObject(instanceId);
            if (asset is DialogueGraphAsset)
            {
                CodeGraphEditorWindow.OpenWindow((CodeGraphAsset)asset);
                return true;
            }

            return false;
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open"))
            {
                CodeGraphEditorWindow.OpenWindow(target as DialogueGraphAsset);
            }
        }
    }
}
