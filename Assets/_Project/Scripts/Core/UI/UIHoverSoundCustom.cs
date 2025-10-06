using Scripts.Audio;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Core.UI
{
    public class UIHoverSoundCustom : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] private SoundEvent _event;
        public void OnPointerEnter(PointerEventData eventData) =>
            _event.PlayOneShot();
    }
}