using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Scripts.Core.InspectorCustomization.Editor
{
    [CustomPropertyDrawer(typeof(InterfaceSelectAttribute))]
    [CanEditMultipleObjects]
    public class InterfaceSelectAttributePropertyDrawer : PropertyDrawer
    {
        private const float BOX_PADDINGS = 4;
        private const string NONE = "None";

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            return this.CreateIMGUIVisualElement(property);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position = EditorGUI.IndentedRect(position);
            GUI.Box(position, GUIContent.none, EditorStyles.helpBox);
            EditorGUI.BeginProperty(position, label, property);
            position = ApplyPadding(position);
            position = EditorGUI.PrefixLabel(position, label);
            
            var selectedTypeName = property.managedReferenceValue == null ? 
                NONE : GetTypeName(property.managedReferenceValue.GetType());
            if (EditorGUI.DropdownButton(position, new GUIContent(selectedTypeName), FocusType.Passive))
            {
                var interfaceType = GetInterfaceType();
                var types = TypeCache.GetTypesDerivedFrom(interfaceType).ToList();
                if (!interfaceType.IsAbstract && !interfaceType.IsInterface)
                    types.Insert(0, interfaceType);
                types.Insert(0, null);

                var menu = new GenericMenu();
                foreach (var type in types)
                {
                    if (type == null)
                    {
                        menu.AddItem(new GUIContent(NONE), selectedTypeName == NONE, () =>
                        {
                            property.managedReferenceValue = null;
                            property.serializedObject.ApplyModifiedProperties();
                            property.serializedObject.Update();
                        });
                    }
                    else
                    {
                        var typeName = GetTypeName(type);
                        menu.AddItem(new GUIContent(typeName), selectedTypeName == typeName, () =>
                        {
                            property.managedReferenceValue = Activator.CreateInstance(type);
                            property.serializedObject.ApplyModifiedProperties();
                            property.serializedObject.Update();
                        });
                    }
                }
                menu.ShowAsContext();
            }
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            position.height -= EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            if (property.managedReferenceValue != null)
                property.DrawInline(position);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = EditorGUIUtility.singleLineHeight + BOX_PADDINGS * 2;
            
            if (property.managedReferenceValue != null)
                height += EditorGUIUtility.standardVerticalSpacing + property.GetDrawInlineHeight();
            return height;
        }

        private Type GetInterfaceType()
        {
            var fieldType = fieldInfo.FieldType;
            if (fieldType.IsArray)
                fieldType = fieldType.GetElementType()!;
            return fieldType;
        }

        private Rect ApplyPadding(Rect rect)
        {
            rect.x += BOX_PADDINGS;
            rect.width -= BOX_PADDINGS * 2;
            rect.y += BOX_PADDINGS;
            rect.height -= BOX_PADDINGS * 2;
            return rect;
        }

        private string GetTypeName(Type type)
        {
            var nameAttr = type.GetCustomAttribute<InterfaceSelectNameAttribute>();
            if (nameAttr != null) 
                return nameAttr.Name;
            return type.Name;
        }
    }
}