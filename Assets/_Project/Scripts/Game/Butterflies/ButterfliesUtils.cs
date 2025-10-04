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
            foreach (var drawnFlower in drawnFlowers)
            {
                Debug.Log("flowerforbutterfly: " + drawnFlower.GetId(time), drawnFlower);
            }
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
        
        public static float CalculateScale(ButterflyConfig config, int flowersCount)
        {
            var t = Random.Range(config.ScaleCoefRandomRange.x, config.ScaleCoefRandomRange.y) *
                    (1 - Mathf.Pow(flowersCount + 1, config.SpeedPow));
            var w = Mathf.Lerp(config.ScaleRange.x, config.ScaleRange.y, t);
            return w;
        }
        
        public static int CalculateCost(ButterflyConfig config, float scale)
        {
            var t = Mathf.Clamp01(Mathf.InverseLerp(config.ScaleRange.x, config.ScaleRange.y, scale));
            return Mathf.CeilToInt(Mathf.Lerp(config.CostRange.x, config.CostRange.y, t));
        }
    }
}