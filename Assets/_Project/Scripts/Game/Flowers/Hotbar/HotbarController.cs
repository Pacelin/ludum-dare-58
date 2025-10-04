using System;
using R3;
using Scripts.Core.Lifetime;
using Scripts.Game.Currency;
using VContainer.Unity;

namespace Scripts.Game.Flowers
{
    public class HotbarController : IInitializable, IDisposable
    {
        private readonly DrawFacade _drawFacade;
        private readonly Hotbar _hotbar;
        private readonly Wallet _wallet;
        private readonly CompositeDisposable _disposables;
        
        private HotbarElement _selectedElement;
        private DrawableObject _selectedDrawable;
        
        public HotbarController(DrawFacade drawFacade, Hotbar hotbar, Wallet wallet)
        {
            _drawFacade = drawFacade;
            _hotbar = hotbar;
            _wallet = wallet;
            _disposables = new CompositeDisposable();
        }

        public void Initialize()
        {
            foreach (var element in _hotbar.Elements)
            {
                element.UpdateSelection(false);
                element.Button.OnClickAsObservable()
                    .Subscribe(_ => SelectElement(element))
                    .AddTo(_disposables);
            }
            SelectElement(_hotbar.InitialSelectedElement);

            _drawFacade.DrawField.ObserveDraw()
                .Subscribe(d => _wallet.Spend(d.DrawPrice))
                .AddTo(_disposables);
            _drawFacade.DrawField.ObserveErase()
                .Subscribe(d => _wallet.Earn(d.EraseReward))
                .AddTo(_disposables);
            
            _wallet.Balance.Subscribe(_ => UpdateDrawState()).AddTo(_disposables);
            
            ApplicationState.IsPaused.Subscribe(isPaused =>
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