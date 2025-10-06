using System;
using R3;
using VContainer.Unity;

namespace Scripts.Game.Butterflies.ButterfliesJournal.Legend
{
    public class ButterfliesLegendPresenter : IInitializable, IDisposable
    {
        private readonly ButterfliesLegend _view;
        private readonly ButterfliesJournal _journal;
        private readonly ButterfliesJournalView _journalView;
        private readonly CompositeDisposable _disposables;
        
        public ButterfliesLegendPresenter(ButterfliesLegend view, ButterfliesJournal journal,
            ButterfliesJournalView journalView)
        {
            _view = view;
            _journal = journal;
            _journalView = journalView;
            _disposables = new CompositeDisposable();
        }
        
        public void Initialize()
        {
            _journal.ObserveEntries()
                .Subscribe(entry =>
                {
                    var element = _view.CreateLegendElement();
                    element.Initialize(_view, _journalView, entry);
                }).AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}