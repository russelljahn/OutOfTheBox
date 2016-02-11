#if UNITY_EDITOR
using Sense.PropertyAttributes;
using UnityEditor;
using UnityEngine;

namespace Sense.Editor.PropertyAttributes
{
    [CustomPropertyDrawer(typeof(ReadonlyAttribute))]
    public class ReadonlyAttributeDrawer : PropertyDrawer 
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) 
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) 
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}
#endif