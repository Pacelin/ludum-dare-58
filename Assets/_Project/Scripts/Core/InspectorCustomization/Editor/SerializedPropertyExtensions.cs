using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Scripts.Core.InspectorCustomization.Editor
{
    public static class SerializedPropertyExtensions
    {
        public static float GetDrawInlineHeight(this SerializedProperty property)
        {
            var minDepth = property.depth + 1;
            var copy = property.Copy();
            if (!copy.NextVisible(true))
                return 0;
            
            float height = 0;
            int count = 0;
            do
            {
                if (copy.depth >= minDepth)
                {
                    height += EditorGUI.GetPropertyHeight(copy);
                    count++;
                }
            } while (copy.NextVisible(false));
            
            return height + EditorGUIUtility.standardVerticalSpacing * (count - 1);
        }
        
        public static Rect DrawInline(this SerializedProperty property, Rect rect)
        {
            var minDepth = property.depth + 1;
            var copy = property.Copy();
            if (!copy.NextVisible(true))
                return rect;
            do
            {
                if (copy.depth >= minDepth)
                {
                    var propHeight = EditorGUI.GetPropertyHeight(copy);
                    var propRect = new Rect(rect.x, rect.y, rect.width, propHeight);
                    EditorGUI.PropertyField(propRect, copy, true);
                    rect.y += propHeight + EditorGUIUtility.standardVerticalSpacing;
                    rect.height -= propHeight;
                }
            } while (copy.NextVisible(false));
            return rect;
        }

        public static VisualElement CreateIMGUIVisualElement(this PropertyDrawer drawer, SerializedProperty property)
        {
            var label = new GUIContent(property.displayName);
            var root = new IMGUIContainer();
            root.contentContainer.style.height = drawer.GetPropertyHeight(property, label);
            root.contentContainer.style.paddingBottom = 0;
            root.contentContainer.style.paddingTop = 0;
            root.contentContainer.style.paddingRight = 0;
            root.contentContainer.style.paddingLeft = 0;
            root.contentContainer.style.left = 0;
            root.contentContainer.style.right = 0;
            root.contentContainer.style.top = 0;
            root.contentContainer.style.bottom = 0;
            root.contentContainer.style.marginBottom = 0;
            root.contentContainer.style.marginTop = 0;
            root.contentContainer.style.marginRight = 0;
            root.contentContainer.style.marginLeft = 0;
            root.onGUIHandler = () => drawer.OnGUI(root.contentRect, property, label);
            return root;
            
        }
    }
}