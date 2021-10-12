using FilterTemplateAssetPostprocessor.Runtime.Attributes;
using UnityEditor;
using UnityEngine;

namespace FilterTemplateAssetPostprocessor.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(NotEmptyAttribute))]
    public class NotEmptyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var oldColor = GUI.backgroundColor;

            if (string.IsNullOrEmpty(property.stringValue))
            {
                GUI.backgroundColor = Color.red;
            }
            
            EditorGUI.PropertyField(position, property, label, true);

            GUI.backgroundColor = oldColor;
            
            if (string.IsNullOrEmpty(property.stringValue))
            {
                var helpBoxRect = new Rect(position);
                helpBoxRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                helpBoxRect.yMax -= EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.HelpBox(helpBoxRect, "This field cannot be empty.", MessageType.Error);
            }

            ((NotEmptyAttribute) this.attribute).IsValid = !string.IsNullOrEmpty(property.stringValue);
        }
        
        public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
        {
            return string.IsNullOrEmpty(property.stringValue)
                ? EditorGUI.GetPropertyHeight(property) + 30f
                : EditorGUI.GetPropertyHeight(property);
        }
    }
}