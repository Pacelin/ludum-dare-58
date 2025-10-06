using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Game.Flowers
{
    public abstract class HotbarElement : MonoBehaviour
    {
        public Button Button => _button;
        
        [SerializeField] private Button _button;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private RectTransform _layoutGroup;
        [SerializeField] private Vector2 _defaultSize;
        [SerializeField] private Vector2 _selectedSize;

        private void OnDisable() => _rectTransform.DOKill();

        public void UpdateSelection(bool selected)
        {
            _rectTransform.DOSizeDelta(selected ? _selectedSize : _defaultSize, 0.1f)
                .OnUpdate(() => LayoutRebuilder.ForceRebuildLayoutImmediate(_layoutGroup));
        }
        public abstract void Visit(HotbarController hotbarController);
    }
}