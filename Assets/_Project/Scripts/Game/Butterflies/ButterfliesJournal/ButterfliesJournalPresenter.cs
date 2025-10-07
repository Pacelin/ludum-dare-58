using System;
using System.Collections.Generic;
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
        private readonly CompositeDisposable _disposables = new();
        
        private ButterfliesJournalFilterView _currentFilter;
        private ButterfliesJournalPageView _leftPage;
        private ButterfliesJournalPageView _rightPage;
        
        private int _pageIndex;
        private Dictionary<int, int> _butterflyPageLookup;

        public ButterfliesJournalPresenter(
            ButterfliesConfig generalConfig, 
            ButterfliesJournal journal, 
            ButterfliesJournalView view)
        {
            _generalConfig = generalConfig;
            _journal = journal;
            _view = view;
        }
        
        public void Initialize()
        {
            _view.gameObject.SetActive(false);
            InitializeFilters();
            InitializeNavigation();
            BuildButterflyPageLookup();
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        private void InitializeFilters()
        {
            _view.ObserveOpen()
                .Subscribe(OnOpen)
                .AddTo(_disposables);

            foreach (var filter in _view.Filters)
            {
                InitializeFilter(filter);
            }
        }

        private void InitializeFilter(ButterfliesJournalFilterView filter)
        {
            filter.ResetSelection(false);
            
            foreach (var page in filter.Pages)
            {
                InitializePage(page);
            }
            
            filter.Click
                .Subscribe(_ => UpdateFilter(filter))
                .AddTo(_disposables);
        }

        private void InitializePage(ButterfliesJournalPageView page)
        {
            page.gameObject.SetActive(false);
            
            foreach (var entry in page.Entries)
            {
                InitializeEntry(entry);
            }
        }

        private void InitializeEntry(ButterfliesJournalEntryView entry)
        {
            entry.gameObject.SetActive(false);
            entry.QuestionMark.SetActive(false);
        }

        private void InitializeNavigation()
        {
            _view.LeftArrow.OnClickAsObservable()
                .Subscribe(_ => MovePage(-1))
                .AddTo(_disposables);
                
            _view.RightArrow.OnClickAsObservable()
                .Subscribe(_ => MovePage(1))
                .AddTo(_disposables);
                
            _view.CloseButton.OnClickAsObservable()
                .Subscribe(_ => _view.Close())
                .AddTo(_disposables);
        }

        private void BuildButterflyPageLookup()
        {
            _butterflyPageLookup = new Dictionary<int, int>();
            var allPages = _view.Filters[0].Pages;
            
            for (int pageIndex = 0; pageIndex < allPages.Length; pageIndex++)
            {
                var page = allPages[pageIndex];
                foreach (var entry in page.Entries)
                {
                    var butterflyId = _generalConfig.GetButterflyID(entry.Butterfly);
                    _butterflyPageLookup[butterflyId] = pageIndex / 2;
                }
            }
        }

        private void OnOpen(int butterflyId)
        {
            var targetPage = _butterflyPageLookup.GetValueOrDefault(butterflyId, 0);
            UpdateFilter(_view.Filters[0], targetPage);
        }

        private void InitEntry(ButterfliesJournalEntryView entry)
        {
            var butterflyId = _generalConfig.GetButterflyID(entry.Butterfly);
            
            if (!_journal.HasButterfly(butterflyId))
            {
                entry.gameObject.SetActive(false);
                entry.QuestionMark.SetActive(true);
                return;
            }

            entry.gameObject.SetActive(true);
            entry.QuestionMark.SetActive(false);
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
            InitializePageEntries(_leftPage);

            if (rightPageIndex < _currentFilter.Pages.Length)
            {
                _rightPage = _currentFilter.Pages[rightPageIndex];
                _rightPage.Assign(_view.SecondPageContainer);
                InitializePageEntries(_rightPage);
            }
            else
            {
                _rightPage = null;
            }
        }

        private void InitializePageEntries(ButterfliesJournalPageView page)
        {
            foreach (var entry in page.Entries)
            {
                InitEntry(entry);
            }
        }

        private void MovePage(int offset)
        {
            var minIndex = 0;
            var maxIndex = (_currentFilter.Pages.Length - 1) / 2;
            _pageIndex = Mathf.Clamp(_pageIndex + offset, minIndex, maxIndex);
            UpdatePages();
        }
    }
}