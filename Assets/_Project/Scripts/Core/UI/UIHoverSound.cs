using Scripts.Audio;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Core.UI
{
    public class UIHoverSound : MonoBehaviour, IPointerEnterHandler
    {
        public void OnPointerEnter(PointerEventData eventData) =>
            AudioSystem.UI_Hover.PlayOneShot();
    }
}