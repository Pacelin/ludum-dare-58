using System;
using System.Collections.Generic;
using R3;
using Scripts.Audio;
using Scripts.Core.Lifetime;
using Scripts.Game.Currency;
using UnityEngine.Profiling;
using VContainer.Unity;

namespace Scripts.Game.Flowers
{
    public class HotbarController : IInitializable, IDisposable
    {
        private readonly DrawFacade _drawFacade;
        private readonly Hotbar _hotbar;
        private readonly Wallet _wallet;
        private readonly CompositeDisposable _disposables;
        private readonly Dictionary<FlowersHotbarElement, bool> _unlockedFlowers;

        private HotbarElement _selectedElement;
        private DrawableObject _selectedDrawable;

        public HotbarController(DrawFacade drawFacade, Hotbar hotbar, Wallet wallet)
        {
            _drawFacade = drawFacade;
            _hotbar = hotbar;
            _wallet = wallet;
            _disposables = new CompositeDisposable();
            _unlockedFlowers = new Dictionary<FlowersHotbarElement, bool>();
        }

        public void Initialize()
        {
            foreach (var element in _hotbar.Elements)
            {
                element.UpdateSelection(false);
                element.Button.OnClickAsObservable()
                    .Subscribe(_ => SelectElement(element))
                    .AddTo(_disposables);
                if (element is FlowersHotbarElement flowerElement)
                {
                    _unlockedFlowers[flowerElement] = flowerElement.Flower.UnlockPrice <= 0;
                    flowerElement.SetUnlockCoins(flowerElement.Flower.UnlockPrice);
                    flowerElement.SetDrawCoins(flowerElement.Flower.DrawPrice);
                    _wallet.Balance.Subscribe(_ => UpdateFlowerElement(flowerElement))
                        .AddTo(_disposables);
                    if (!_unlockedFlowers[flowerElement])
                    {
                        flowerElement.UnlockButton.OnClickAsObservable().Subscribe(_ =>
                        {
                            _wallet.Spend(flowerElement.Flower.UnlockPrice);
                            AudioSystem.Game_UnlockFlower.PlayOneShot();
                            _unlockedFlowers[flowerElement] = true;
                            UpdateFlowerElement(flowerElement);
                            SelectElement(flowerElement);
                        }).AddTo(_disposables);
                    }
                }
            }
            SelectElement(_hotbar.InitialSelectedElement);

            _drawFacade.DrawField.ObserveDraw()
                .Subscribe(d => _wallet.Spend(d.DrawPrice))
                .AddTo(_disposables);
            _drawFacade.DrawField.ObserveErase()
                .Subscribe(d => _wallet.Earn(d.EraseReward))
                .AddTo(_disposables);

            _wallet.Balance.Subscribe(_ => UpdateDrawState()).AddTo(_disposables);

            ApplicationState.IsPaused.DistinctUntilChanged().Subscribe(isPaused =>
            {
                if (isPaused)
                {
                    _drawFacade.SetCanDraw(false);
                    _drawFacade.SetCanErase(false);
                }
                else
                {
                    _drawFacade.SetCanErase(true);
                    UpdateDrawState();
                }
            }).AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        private void UpdateFlowerElement(FlowersHotbarElement element)
        {
            if (_unlockedFlowers[element])
                element.SetState(true, true);
            else
                element.SetState(false, _wallet.HasEnough(element.Flower.UnlockPrice));
        }

        public void Accept(EraseHotbarElement _)
        {
            _selectedDrawable = null;
            _drawFacade.SetDrawObject(null);
        }

        public void Accept(FlowersHotbarElement element)
        {
            _selectedDrawable = element.Flower;
            _drawFacade.SetDrawObject(_selectedDrawable);
        }

        private void UpdateDrawState()
        {
            if (!_selectedDrawable)
                _drawFacade.SetCanDraw(true);
            else
                _drawFacade.SetCanDraw(_wallet.HasEnough(_selectedDrawable.DrawPrice));
        }

        private void SelectElement(HotbarElement element)
        {
            if (_selectedElement == element)
                return;
            if (_selectedElement)
                _selectedElement.UpdateSelection(false);
            _selectedElement = element;
            _selectedElement.UpdateSelection(true);
            _selectedElement.Visit(this);
            UpdateDrawState();
        }
    }
}