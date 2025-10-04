using Scripts.Game.Currency;

namespace Scripts.Game.Butterflies
{
    public class ButterfliesCatcher
    {
        private readonly ButterfliesConfig _config;
        private readonly Wallet _wallet;
        
        public ButterfliesCatcher(ButterfliesConfig config, Wallet wallet)
        {
            _config = config;
            _wallet = wallet;
        }

        public void Catch(ButterflyView butterfly)
        {
            _wallet.Earn(ButterfliesUtils.CalculateCost(butterfly.Config, butterfly.Scale));
        }
    }
}