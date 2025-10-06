namespace Scripts.Game.Butterflies
{
    public class ButterfliesCatcher
    {
        private readonly ButterfliesJournal.ButterfliesJournal _butterfliesJournal;
        
        public ButterfliesCatcher(ButterfliesJournal.ButterfliesJournal butterfliesJournal)
        {
            _butterfliesJournal = butterfliesJournal;
        }

        public void Catch(ButterflyView butterfly)
        {
            _butterfliesJournal.PostButterfly(butterfly);
        }
    }
}