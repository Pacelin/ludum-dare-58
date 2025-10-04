using System;
using R3;
using UnityEngine;

namespace Scripts.Game.Currency
{
    public class Wallet
    {
        public ReadOnlyReactiveProperty<int> Balance => _balance;

        private readonly ReactiveProperty<int> _balance;
        private readonly Subject<int> _earnSubject;
        private readonly Subject<int> _spendSubject;
        
        public Wallet(int initialBalance)
        {
            _balance = new ReactiveProperty<int>(initialBalance);
            _earnSubject = new Subject<int>();
            _spendSubject = new Subject<int>();
        }
        
        public Observable<int> ObserveEarn() => _earnSubject;
        public Observable<int> ObserveSpend() => _spendSubject;
        
        public void Earn(int amount)
        {
            if (amount == 0) 
                return;
            _balance.Value += amount;
            _earnSubject.OnNext(amount);
        }

        public void Spend(int amount)
        {
            if (amount == 0) 
                return;
            if (_balance.Value < amount)
            {
                Debug.LogError("Not enough money to spend");
                return;
            }
            _balance.Value = Math.Max(0, _balance.Value);
            _spendSubject.OnNext(amount);
        }
        
        public bool HasEnough(int amount) => _balance.Value >= amount;
    }
}