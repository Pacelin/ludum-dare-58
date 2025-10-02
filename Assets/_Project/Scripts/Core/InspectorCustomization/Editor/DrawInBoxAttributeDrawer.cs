using UnityEditor;
using UnityEngine;

namespace Scripts.Core.InspectorCustomization.Editor
{
    [CustomPropertyDrawer(typeof(DrawInBoxAttribute))]
    [CanEditMultipleObjects]
    public class DrawInBoxAttributeDrawer : PropertyDrawer
    {
        private const float PADDING = 4;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.serializedObject.Update();
            
            GUI.Box(position, GUIContent.none, EditorStyles.helpBox);
            position.x += PADDING;
            position.width -= PADDING * 2;
            position.y += PADDING;
            position.height -= PADDING * 2;
            
            var drawInBoxAttribute = (DrawInBoxAttribute)attribute;
            if (drawInBoxAttribute.ShowLabel)
            {
                var labelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(labelRect, label);
                position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
            
            property.DrawInline(position);
            property.serializedObject.ApplyModifiedProperties();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = property.GetDrawInlineHeight() + PADDING * 2;
            var drawInBoxAttribute = (DrawInBoxAttribute)attribute;
            if (drawInBoxAttribute.ShowLabel)
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            return height;
        }
    }
}