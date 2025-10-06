using System.Linq;
using Scripts.Game.Flowers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Game.Butterflies
{
    public static class ButterfliesUtils
    {
        public static Flower GetRandomFlower(DrawFacade drawFacade) => drawFacade.GetRandomDrawnObject<Flower>();
        
        public static ButterflyView CalculateButterfly(ButterfliesConfig config, Flower flower,
            DrawFacade drawFacade, GameTime time, out int flowersCount)
        {
            var drawnFlowers = drawFacade.GetDrawnObjects<Flower>(flower.transform.position,
                config.FlowersFindRadius).ToList();
            flowersCount = drawnFlowers.Count;
            if (drawnFlowers.Count == 0) return null;
            if (drawnFlowers.Count == 1) return config.GetButterfly(drawnFlowers[0].GetId(time));
            
            var first = drawnFlowers[Random.Range(0, drawnFlowers.Count)];
            drawnFlowers.Remove(first);
            var second = drawnFlowers[Random.Range(0, drawnFlowers.Count)];
            var firstId = first.GetId(time);
            var secondId = second.GetId(time);
            
            if (firstId == secondId)
                return config.GetButterfly(firstId);

            var haveSecond = Random.Range(0, 1) <= config.DoubleFlowerChance;
            return haveSecond ?
                config.GetButterfly(firstId, secondId) :
                config.GetButterfly(firstId);
        }

        public static string GetButterflyName(ButterfliesConfig config, ButterflyView butterfly, float size)
        {
            var sizeConfig = config.GetButterflySizeConfig(butterfly.Config, size);
            var sizeString = butterfly.Config.IsFemale ?
                sizeConfig.SizeStringFemale.GetLocalizedString() :
                sizeConfig.SizeString.GetLocalizedString();
            var avgString = butterfly.Config.Prefix.GetLocalizedString();
            var nameString = butterfly.Config.Name.GetLocalizedString();
            return sizeString + " " + avgString + " " + nameString;
        }

        public static float CalculateSize(ButterfliesConfig generalConfig, ButterflyConfig config, int flowersCount)
        {
            var flowersMeanMultiplier = generalConfig.GetFlowerMeanMultiplier(flowersCount);
            var mean = config.AverageSize * generalConfig.SizeMeanMultiplier * flowersMeanMultiplier;
            var deviation = config.AverageSize * generalConfig.SizeDeviationMultiplier;
            var size = Mathf.Round(NextGaussian(mean, deviation) * 100f) / 100f;
            var minSize = generalConfig.MinSizeMultiplier * config.AverageSize;
            var maxSize = generalConfig.MaxSizeMultiplier * config.AverageSize;
            return Mathf.Clamp(size, minSize, maxSize);
        }
        
        public static int CalculateCost(ButterfliesConfig generalConfig, ButterflyConfig config, float size)
        {
            var sizeConfig = generalConfig.GetButterflySizeConfig(config, size);
            var bySizeMultipler = size / config.AverageSize;
            var cost = config.AverageCost * (bySizeMultipler * 0.5f + sizeConfig.CostMutiplier);
            return Mathf.Max(1, Mathf.RoundToInt(cost));
        }
        
        private static float NextGaussian(float mean = 0.0f, float standardDeviation = 1.0f)
        {
            float v1, v2, s;
            do
            {
                // Генерируем две случайные точки в единичном круге
                v1 = 2.0f * Random.Range(0f, 1f) - 1.0f;
                v2 = 2.0f * Random.Range(0f, 1f) - 1.0f;
                s = v1 * v1 + v2 * v2;
            } 
            while (s >= 1.0f || s == 0f);

            s = Mathf.Sqrt((-2.0f * Mathf.Log(s)) / s);

            return mean + v1 * s * standardDeviation;
        }
    }
}