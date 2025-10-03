using UnityEngine;
using VContainer;

namespace Scripts.Core.UI
{
    public class ScreenView : MonoBehaviour
    {
        [Inject]
        private void Construct() => gameObject.SetActive(false);
        
        public void Open() => gameObject.SetActive(true);
        public void Close() => gameObject.SetActive(false);
    }
}