using UnityEditor;
using UnityEngine;

namespace TSS.Audio.Editor
{
    [CustomEditor(typeof(SoundEventComponent))]
    internal class SoundEventComponentEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var refProp = serializedObject.FindProperty("_eventRef");
            EditorGUILayout.PropertyField(refProp, new GUIContent("Sound Reference"));
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }
    }
}