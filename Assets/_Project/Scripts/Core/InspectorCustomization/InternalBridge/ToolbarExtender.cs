using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine.UIElements;

namespace Scripts.Core.InspectorCustomization.InternalBridge
{
    [InitializeOnLoad]
    public static class ToolbarExtender
    {
        static ToolbarExtender()
        {
            EditorApplication.delayCall += Refresh;
        }

        private static void Refresh()
        {
            var containers = new ToolbarContainers();
            containers.Build();
            
            var llElements = new List<(int order, VisualElement element)>();
            var lrElements = new List<(int order, VisualElement element)>();
            var rlElements = new List<(int order, VisualElement element)>();
            var rrElements = new List<(int order, VisualElement element)>();
            
            foreach (var (type, attribute) in Get())
            {
                switch (attribute.Position)
                {
                    case EToolbarPosition.LeftLeftAlign:
                        llElements.Add((attribute.Order, (VisualElement)Activator.CreateInstance(type)));
                        break;
                    case EToolbarPosition.LeftRightAlign:
                        lrElements.Add((attribute.Order, (VisualElement)Activator.CreateInstance(type)));
                        break;
                    case EToolbarPosition.RightLeftAlign:
                        rlElements.Add((attribute.Order, (VisualElement)Activator.CreateInstance(type)));
                        break;
                    case EToolbarPosition.RightRightAlign:
                    default:
                        rrElements.Add((attribute.Order, (VisualElement)Activator.CreateInstance(type)));
                        break;
                }
            }
            
            foreach (var (_, element) in llElements.OrderBy(t => t.order))
                containers.LeftContainerLeftAlign.Add(element);
            foreach (var (_, element) in lrElements.OrderBy(t => t.order))
                containers.LeftContainerRightAlign.Add(element);
            foreach (var (_, element) in rlElements.OrderBy(t => t.order))
                containers.RightContainerLeftAlign.Add(element);
            foreach (var (_, element) in rrElements.OrderBy(t => t.order))
                containers.RightContainerRightAlign.Add(element);
        }
        
        private static IEnumerable<(Type type, ToolbarElementAttribute Attribute)> Get()
        {
            var types = TypeCache.GetTypesWithAttribute<ToolbarElementAttribute>();
            foreach (var type in types)
                if (typeof(VisualElement).IsAssignableFrom(type))
                    yield return (type, type.GetCustomAttribute<ToolbarElementAttribute>());
        }
    }
}