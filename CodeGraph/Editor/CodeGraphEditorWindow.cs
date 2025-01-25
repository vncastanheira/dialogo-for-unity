using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace CodeGraph.Editor
{
    public class CodeGraphEditorWindow : EditorWindow
    {
        [SerializeField]
        private CodeGraphAsset currentGraph;

        [SerializeField]
        private CodeGraphView currentView;

        [SerializeField]
        private SerializedObject serializedObject;

        public CodeGraphAsset CurrentGraph => currentGraph;

        public static void OpenWindow(CodeGraphAsset target)
        {
            var windows = Resources.FindObjectsOfTypeAll<CodeGraphEditorWindow>();
            foreach (var w in windows)
            {
                if (w.currentGraph == target)
                {
                    w.Focus();
                    return;
                }
            }

            var window = CreateWindow<CodeGraphEditorWindow>(typeof(CodeGraphEditorWindow), typeof(SceneView));
            window.titleContent = new GUIContent($"{target.name}");
            window.Load(target);
        }

        private void OnEnable()
        {
            if (currentGraph != null)
                DrawGraph();

            rootVisualElement.RegisterCallback<KeyDownEvent>(e =>
            {
                if(e.keyCode == KeyCode.S && e.modifiers == EventModifiers.Control)
                {
                    SaveChanges();
                }
            });


        }

        private void OnDisable()
        {
            if(currentView != null)
               rootVisualElement.Remove(currentView);
        }

        private void OnGUI()
        {
            if(currentGraph != null)
            {
                hasUnsavedChanges = EditorUtility.IsDirty(currentGraph);
            }
        }


        private void Load(CodeGraphAsset target)
        {
            currentGraph = target;
            DrawGraph();
        }

        private void DrawGraph()
        {
            serializedObject = new SerializedObject(currentGraph);
            currentView = new CodeGraphView(serializedObject, this);
            currentView.graphViewChanged += OnChange;
            rootVisualElement.Add(currentView);
        }

        private GraphViewChange OnChange(GraphViewChange graphViewChange)
        {
            EditorUtility.SetDirty(currentGraph);
            return graphViewChange;
        }

        public override void SaveChanges()
        {
            foreach (var editorNode in currentView.GraphEditorNodes)
            {
                editorNode.SavePosition();
            }

            base.SaveChanges();
        }
    }
}
