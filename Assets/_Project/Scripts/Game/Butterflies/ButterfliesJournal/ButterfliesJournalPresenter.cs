using System;
using System.Linq;
using R3;
using Scripts.Core.Lifetime;
using UnityEngine;
using VContainer.Unity;

namespace Scripts.Game.Butterflies.ButterfliesJournal
{
    public class ButterfliesJournalPresenter : IInitializable, IDisposable
    {
        private readonly ButterfliesConfig _generalConfig;
        private readonly ButterfliesJournal _journal;
        private readonly ButterfliesJournalView _view;
        private readonly CompositeDisposable _disposables;
        
        private ButterfliesJournalFilterView _currentFilter;
        private ButterfliesJournalPageView _leftPage;
        private ButterfliesJournalPageView _rightPage;
        
        private int _pageIndex;
        
        public ButterfliesJournalPresenter(ButterfliesConfig generalConfig, 
            ButterfliesJournal journal, ButterfliesJournalView view)
        {
            _generalConfig = generalConfig;
            _journal = journal;
            _view = view;
            _disposables = new CompositeDisposable();
        }
        
        public void Initialize()
        {
            _view.gameObject.SetActive(false);
            _view.ObserveOpen()
                .Subscribe(OnOpen)
                .AddTo(_disposables);
            
            foreach (var filter in _view.Filters)
            {
                filter.ResetSelection(false);
                foreach (var page in filter.Pages)
                {
                    page.gameObject.SetActive(false);
                    foreach (var entry in page.Entries)
                        entry.gameObject.SetActive(false);
                }
                filter.Click
                    .Subscribe(_ => UpdateFilter(filter))
                    .AddTo(_disposables);
            }
            
            _view.LeftArrow.OnClickAsObservable()
                .Subscribe(_ => MovePage(-1))
                .AddTo(_disposables);
            _view.RightArrow.OnClickAsObservable()
                .Subscribe(_ => MovePage(1))
                .AddTo(_disposables);
            _view.CloseButton.OnClickAsObservable()
                .Subscribe(_ =>
                    _view.Close())
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        private void OnOpen(int butterflyId)
        {
            var entries = _view.Filters
                .SelectMany(f => f.Pages)
                .SelectMany(p => p.Entries)
                .Distinct();
            foreach (var entry in entries)
                InitEntry(entry);
            UpdateFilter(_view.Filters[0], FindPage(butterflyId));
        }

        private void InitEntry(ButterfliesJournalEntryView entry)
        {
            var butterflyId = _generalConfig.GetButterflyID(entry.Butterfly);
            if (_journal.HasButterfly(butterflyId))
            {
                Debug.Log("Butterfly entry found", entry);
                entry.gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("Butterfly entry not found", entry);
                entry.gameObject.SetActive(false);
                return;
            }
            
            entry.ResetWorld();
            var localRecord = _journal.GetLocalRecord(butterflyId);
            entry.SetData(_generalConfig, localRecord.Size, localRecord.Cost);
            _journal.GetWorldRecord(butterflyId, entry.SetWorldData);
        }
        
        private void UpdateFilter(ButterfliesJournalFilterView filter, int pageIndex = 0)
        {
            if (_currentFilter)
                _currentFilter.UpdateSelection(false);
            _currentFilter = filter;
            _currentFilter.UpdateSelection(true);
            
            _pageIndex = pageIndex;
            UpdatePages();
        }

        private void UpdatePages()
        {
            if (_leftPage)
                _leftPage.Deassign();
            if (_rightPage)
                _rightPage.Deassign();

            var leftPageIndex = _pageIndex * 2;
            var rightPageIndex = leftPageIndex + 1;
            _leftPage = _currentFilter.Pages[leftPageIndex];
            _leftPage.Assign(_view.FirstPageContainer);
            if (rightPageIndex < _currentFilter.Pages.Length)
            {
                _rightPage = _currentFilter.Pages[rightPageIndex];
                _rightPage.Assign(_view.SecondPageContainer);
            }
            else
            {
                _rightPage = null;
            }
        }

        private void MovePage(int offset)
        {
            var minIndex = 0;
            var maxIndex = (_currentFilter.Pages.Length - 1) / 2;
            _pageIndex = Mathf.Clamp(_pageIndex + offset, minIndex, maxIndex);
            UpdatePages();
        }

        private int FindPage(int butterflyId)
        {
            for (int i = 0; i < _view.Filters[0].Pages.Length; i++)
            {
                var page = _view.Filters[0].Pages[i];
                foreach (var entry in page.Entries)
                    if (_generalConfig.GetButterflyID(entry.Butterfly) == butterflyId)
                        return i / 2;
            }
            return 0;
        }
    }
}