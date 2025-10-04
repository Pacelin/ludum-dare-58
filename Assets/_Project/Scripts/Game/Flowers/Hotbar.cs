using R3;
using UnityEngine;

namespace Scripts.Game.Flowers
{
    public class Hotbar : MonoBehaviour
    {
        [SerializeField] private HotbarElement[] _elements;
        [SerializeField] private HotbarElement _selectedElement;
        [SerializeField] private DrawCollider _drawCollider;

        private CompositeDisposable _disposables;
        
        private void OnEnable()
        {
            _disposables = new CompositeDisposable();
            foreach (var element in _elements)
            {
                element.UpdateSelection(_drawCollider, false);
                element.Button.OnClickAsObservable()
                    .Subscribe(_ =>
                    {
                        _selectedElement.UpdateSelection(_drawCollider, false);
                        _selectedElement = element;
                        _selectedElement.UpdateSelection(_drawCollider, true);
                    }).AddTo(_disposables);
            }
            _selectedElement.UpdateSelection(_drawCollider, true);
        }

        private void OnDisable()
        {
            _disposables.Dispose();
        }
    }
}