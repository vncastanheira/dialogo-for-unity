using System.Linq;
using CodeGraph;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
#endif
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UIElements;

namespace DialogueGraph
{
    [System.Serializable]
    public class LocalizedText : INodeProperty
    {
        public string text;
        public Locale locale;

#if UNITY_EDITOR
        public VisualElement Draw(SerializedProperty serializedProperty)
        {
            var root = new PropertyField(serializedProperty);

            var textProperty = serializedProperty.FindPropertyRelative(nameof(text));
            TextField textField = new("Text", -1, multiline: true, isPasswordField: false, ' ');
            textField.AddToClassList("dialogue-textfield");
            textField.BindProperty(textProperty);
            root.Add(textField);

            var instance = LocalizationSettings.GetInstanceDontCreateDefault();
            if (instance == null)
                return root;

            var localeProperty = serializedProperty.FindPropertyRelative(nameof(locale));
            var ObjField = new ObjectField("Locale");
            ObjField.allowSceneObjects = false;
            ObjField.objectType = typeof(Locale);
            ObjField.BindProperty(localeProperty);
            root.Add(ObjField);

            return root;
        }
#endif

    }
}
