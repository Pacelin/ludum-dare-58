using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Game.Flowers
{
    public abstract class HotbarElement : MonoBehaviour
    {
        public Button Button => _button;
        
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _selectedMark;

        public virtual void UpdateSelection(DrawCollider drawCollider, bool selected) =>
            _selectedMark.SetActive(selected);
    }
}