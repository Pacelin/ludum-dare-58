using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Scripts.Game.Currency
{
    public class WalletView : MonoBehaviour
    {
        [SerializeField] private RectTransform _reactTransform;
        [SerializeField] private TMP_Text _balanceText;

        private void OnDisable() => _reactTransform.DOKill();
        
        public void SetBalance(int balance) => _balanceText.text = balance.ToString();

        public void DOSpend()
        {
            _reactTransform.DOKill();
            _reactTransform.DOScale(1.2f, 0.1f)
                .From(1)
                .SetLoops(2, LoopType.Yoyo);
        }

        public void DOEarn()
        {
            _reactTransform.DOKill();
            _reactTransform.DOScale(1.2f, 0.1f)
                .From(1)
                .SetLoops(2, LoopType.Yoyo);
        }
    }
}