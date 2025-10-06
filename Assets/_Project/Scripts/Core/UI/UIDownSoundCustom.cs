using Scripts.Audio;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Core.UI
{
    public class UIDownSoundCustom : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private SoundEvent _event;
        
        public void OnPointerDown(PointerEventData eventData) =>
            _event.PlayOneShot();
    }
}