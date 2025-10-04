using JetBrains.Annotations;
using Scripts.Core.InspectorCustomization.InternalBridge;
using UnityEditor.UIElements;

namespace Scripts.Audio.Editor
{
    [UsedImplicitly]
    [ToolbarElement(EToolbarPosition.RightRightAlign)]
    internal class FMODAudioGenerateButton : ToolbarButton
    {
        public FMODAudioGenerateButton()
        {
            text = "Refresh FMOD";
            iconImage = FMODUtilsInternal.GetFMODStudioIcon();
            style.paddingLeft = 4;
            style.paddingRight = 4;
            clicked += OnClick;
        }
        ~FMODAudioGenerateButton()
        {
            clicked -= OnClick;
        }

        private void OnClick() =>
            FMODAudioGenerator.Generate();
    }
}