using System.Collections.Generic;
using Scripts.Game.Butterflies;
using UnityEngine;

namespace Scripts.Game
{
    public class Cheats : MonoBehaviour
    {
        [SerializeField] private int _countToGenerate = 100;

        private ButterfliesConfig _generalConfig;
        
        public void Construct(ButterfliesConfig generalConfig)
        {
            _generalConfig = generalConfig;
        }

        private void Update()
        {
            if (!_generalConfig)
                return;
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                var dict = new Dictionary<ButterflySizeConfig, int>(); 
                var maxSize = 0f;
                var maxName = "";
                var minSize = float.MaxValue;
                var minName = "";
                
                for (int i = 0; i < _countToGenerate; i++)
                {
                    var randomButterfly = _generalConfig.GetRandomButterfly();
                    var size = ButterfliesUtils.CalculateSize(_generalConfig, randomButterfly.Config, 20);
                    var sizeConfig = _generalConfig.GetButterflySizeConfig(randomButterfly.Config, size);
                    if (!dict.ContainsKey(sizeConfig))
                        dict[sizeConfig] = 0;
                    dict[sizeConfig]++;
                    var name = ButterfliesUtils.GetButterflyName(_generalConfig, randomButterfly.Config, size);
                    if (size > maxSize)
                    {
                        maxName = name;
                        maxSize = size;
                    }

                    if (size < minSize)
                    {
                        minName = name;
                        minSize = size;
                    }
                }
                Debug.LogWarning($"Max size: {maxSize:F} mm, name: {maxName}");
                Debug.LogWarning($"Min size: {minSize:F} mm, name: {minName}");
                foreach (var pair in dict)
                    Debug.LogWarning($"{pair.Key.SizeString.GetLocalizedString()} count: {pair.Value}");
            }
        }
    }
}