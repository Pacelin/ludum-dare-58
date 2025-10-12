using System.Collections.Generic;
using Scripts.Game.Butterflies;
using Scripts.Game.Butterflies.ButterfliesJournal;
using UnityEngine;

namespace Scripts.Game
{
    public class Cheats : MonoBehaviour
    {
        [SerializeField] private int _countToGenerate = 100;
        [SerializeField] private ButterflyView _butterfly;

        private ButterfliesConfig _generalConfig;
        private ButterfliesJournal _journal;
        
        public void Construct(ButterfliesConfig generalConfig, ButterfliesJournal journal)
        {
            _generalConfig = generalConfig;
            _journal = journal;
        }

        private void Update()
        {
            if (!_generalConfig)
                return;
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                for (int i = 0; i < _generalConfig.GetButterfliesCount(); i++)
                    _journal.CheatButterfly(i);
                /*var maxSize = 0f;
                
                for (int i = 0; i < _countToGenerate; i++)
                {
                    var size = ButterfliesUtils.CalculateSize(_generalConfig, _butterfly.Config, 20);
                    if (size > maxSize)
                        maxSize = size;

                    if (maxSize > 60.00f)
                        break;
                }
                var maxName = ButterfliesUtils.GetButterflyName(_generalConfig, _butterfly.Config, maxSize);
                Debug.LogWarning($"Max size: {maxSize:F} mm, name: {maxName}");*/
            }
        }
    }
}