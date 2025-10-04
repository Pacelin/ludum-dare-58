using UnityEngine;

namespace Scripts.Game.Flowers
{
    public class Hotbar : MonoBehaviour
    {
        public HotbarElement[] Elements => _elements;
        public HotbarElement InitialSelectedElement => _initialSelectedElement;
        
        [SerializeField] private HotbarElement[] _elements;
        [SerializeField] private HotbarElement _initialSelectedElement;
    }
}