using Scripts.Audio;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Core.UI
{
    public class UIDownSound : MonoBehaviour, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData) =>
            AudioSystem.UI_Down.PlayOneShot();
    }
}