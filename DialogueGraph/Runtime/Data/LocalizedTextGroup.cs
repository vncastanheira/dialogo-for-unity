using System;
using System.Collections.Generic;
using CodeGraph;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

#endif
using UnityEngine.UIElements;

namespace DialogueGraph
{
    [System.Serializable]
    public class LocalizedTextGroup : INodeProperty
    {
        public List<LocalizedText> localizedTexts;

        public LocalizedTextGroup()
        {
            localizedTexts = new();
        }

#if UNITY_EDITOR
        public VisualElement Draw(SerializedProperty serializedProperty)
        {
            var listProperty = serializedProperty.FindPropertyRelative(nameof(localizedTexts));

            var listView = new ListView();
            void addLocalizedText()
            {
                localizedTexts.Add(new LocalizedText());
                listProperty.serializedObject.ApplyModifiedProperties();
                DrawList(listView, listProperty);
            }
            var addBtn = new Button(addLocalizedText) { text = "Add Text" };
            addBtn.AddToClassList("dialogue-btn");

            listView.hierarchy.Add(addBtn);

            DrawList(listView, listProperty);

            return listView;
        }

        private void DrawList(ListView listView, SerializedProperty listProperty)
        {
            for (int i = 0; i < localizedTexts.Count; i++)
            {
                var item = localizedTexts[i];
                var arrayElement = listProperty.GetArrayElementAtIndex(i);
                var element = item.Draw(arrayElement);
                listView.hierarchy.Add(element);
            }
        }
#endif
    }
}
