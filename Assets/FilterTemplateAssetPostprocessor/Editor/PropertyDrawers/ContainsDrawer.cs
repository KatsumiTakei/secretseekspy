using System.Text.RegularExpressions;
using FilterTemplateAssetPostprocessor.Runtime.Attributes;
using UnityEditor;
using UnityEngine;

namespace FilterTemplateAssetPostprocessor.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(ContainsAttribute))]
    public class ContainsDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var containsAttribute = (ContainsAttribute) this.attribute;
            var oldColor = GUI.backgroundColor;

            if (string.IsNullOrEmpty(property.stringValue))
            {
                GUI.backgroundColor = Color.red;
            }

            EditorGUI.PropertyField(position, property, label, true);

            GUI.backgroundColor = oldColor;

            containsAttribute.IsValid = !string.IsNullOrEmpty(property.stringValue) &&
                                        Regex.IsMatch(property.stringValue, containsAttribute.Value);
            
            if (!containsAttribute.IsValid)
            {
                var helpBoxRect = new Rect(position);
                helpBoxRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                helpBoxRect.yMax -= EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.HelpBox(helpBoxRect,
                    string.IsNullOrEmpty(property.stringValue)
                        ? "This field cannot be empty."
                        : containsAttribute.Message, MessageType.Error);
            }
        }
        
        public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
        {
            var containsAttribute = (ContainsAttribute) this.attribute;
            return !containsAttribute.IsValid
                ? EditorGUI.GetPropertyHeight(property) + 30f
                : EditorGUI.GetPropertyHeight(property);
        }
    }
}