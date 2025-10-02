using System.Reflection;
using UnityEditor;
using UnityEngine.UIElements;

namespace Scripts.Core.InspectorCustomization.InternalBridge
{
    internal class ToolbarContainers
    {
        public VisualElement LeftContainerLeftAlign => leftContainerLeftAlign;
        public VisualElement LeftContainerRightAlign => leftContainerRightAlign;
        public VisualElement RightContainerLeftAlign => rightContainerLeftAlign;
        public VisualElement RightContainerRightAlign => rightContainerRightAlign;
        
        private VisualElement leftContainerLeftAlign;
        private VisualElement leftContainerRightAlign;
        private VisualElement rightContainerLeftAlign;
        private VisualElement rightContainerRightAlign;
        
        private readonly VisualElement _leftZone;
        private readonly VisualElement _rightZone;
        
        public ToolbarContainers()
        {
            var rootField = typeof(Toolbar).GetField("m_Root", BindingFlags.NonPublic | BindingFlags.Instance);
            var root = (VisualElement) rootField!.GetValue(Toolbar.get);
            _leftZone = root.Q("ToolbarZoneLeftAlign");
            _rightZone = root.Q("ToolbarZoneRightAlign");
        }
        
        public void Build()
        {
            leftContainerLeftAlign = new VisualElement();
            leftContainerLeftAlign.style.flexDirection = FlexDirection.Row;
            leftContainerLeftAlign.style.justifyContent = Justify.FlexStart;
            leftContainerLeftAlign.style.flexGrow = 1;
            
            leftContainerRightAlign = new VisualElement();
            leftContainerRightAlign.style.flexDirection = FlexDirection.Row;
            leftContainerRightAlign.style.justifyContent = Justify.FlexEnd;
            leftContainerRightAlign.style.flexGrow = 1;
            
            rightContainerLeftAlign = new VisualElement();
            rightContainerLeftAlign.style.flexDirection = FlexDirection.Row;
            rightContainerLeftAlign.style.justifyContent = Justify.FlexStart;
            rightContainerLeftAlign.style.flexGrow = 1;
            
            rightContainerRightAlign = new VisualElement();
            rightContainerRightAlign.style.flexDirection = FlexDirection.Row;
            rightContainerRightAlign.style.justifyContent = Justify.FlexEnd;
            rightContainerRightAlign.style.flexGrow = 1;
            
            _leftZone.Add(leftContainerLeftAlign);
            _leftZone.Add(leftContainerRightAlign);
            _rightZone.Add(rightContainerRightAlign);
            _rightZone.Add(rightContainerLeftAlign);
        }
    }
}