using mixpanel;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Analytics
{
    public class ButtonAnalytics : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private string _sendEvent;

        private void OnValidate()
        {
            if (!_button)
                _button = GetComponent<Button>();
        }

        private void OnEnable() => _button.onClick.AddListener(OnClick);
        private void OnDisable() => _button.onClick.RemoveListener(OnClick);

        private void OnClick() => Mixpanel.Track(_sendEvent);
    }
}