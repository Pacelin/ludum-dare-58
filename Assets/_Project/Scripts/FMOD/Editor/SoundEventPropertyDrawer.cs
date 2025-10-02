using UnityEditor;
using UnityEngine;

namespace TSS.Audio.Editor
{
    [CanEditMultipleObjects]
    [CustomPropertyDrawer(typeof(SoundEvent))]
    internal class SoundEventPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var refProp = property.FindPropertyRelative("_eventRef");
            EditorGUI.PropertyField(position, refProp, label);
            property.serializedObject.ApplyModifiedProperties();
            property.serializedObject.Update();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_eventRef"));
        }
    }
}