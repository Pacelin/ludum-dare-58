using Cysharp.Threading.Tasks;
using DG.Tweening;
using Scripts.Core.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Loading
{
    public class EntryNicknameScreenView : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _goButton;
        [SerializeField] private TMP_Text _errorText;

        private void OnEnable()
        {
            _goButton.onClick.AddListener(OnClick);
            _errorText.gameObject.SetActive(false);
            _errorText.alpha = 0;
        }

        private void OnDisable()
        {
            _goButton.onClick.RemoveListener(OnClick);
            _errorText.DOKill();
        } 

        private void OnClick()
        {
            var nickName = _inputField.text;
            if (string.IsNullOrEmpty(nickName) || nickName.Length < 3)
                Error();
            else
                Go();
        }

        private void Error()
        {
            _errorText.DOFade(1, 0.5f)
                .From(0)
                .SetLoops(2, LoopType.Yoyo)
                .OnStart(() => _errorText.gameObject.SetActive(true))
                .OnComplete(() => _errorText.gameObject.SetActive(false));
        }

        private void Go()
        {
            PlayerPrefs.SetString("username", _inputField.text);
            SceneManager.LoadScene(SceneManager.Database.MainMenu).Forget(); 
        }
    }
}