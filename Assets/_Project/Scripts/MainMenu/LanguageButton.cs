using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace Scripts.MainMenu
{
    public class LanguageButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _activeMark;
        [SerializeField] private Locale _locale;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
            LocalizationSettings.SelectedLocaleChanged += RefreshLocale;
            RefreshLocale(LocalizationSettings.SelectedLocale);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
            LocalizationSettings.SelectedLocaleChanged -= RefreshLocale;
        }

        private void RefreshLocale(Locale locale) => _activeMark.gameObject.SetActive(_locale.Equals(locale));
        private void OnClick() => LocalizationSettings.SelectedLocale = _locale;
    }
}