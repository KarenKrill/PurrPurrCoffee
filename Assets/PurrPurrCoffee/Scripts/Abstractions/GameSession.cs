using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PurrPurrCoffee.Abstractions
{
    public class GameSession // ещё один костыльный класс (всё в последний день)
    {
        public event Action<float> CoffeeReputationChanged;
        public event Action<float> CoffeeRevenueChanged;
        public event Action<bool> IsCoffeeInPlayerHandsChanged;
        public float CoffeeReputation => (float)_reviews.Average();
        public float CoffeeRrevenue => _coffeeRevenue;
        public bool IsCoffeeInPlayerHands
        {
            get => _isCoffeeInPlayerHands;
            set
            {
                if (_isCoffeeInPlayerHands != value)
                {
                    _isCoffeeInPlayerHands = value;
                    IsCoffeeInPlayerHandsChanged?.Invoke(value);
                }
            }
        }
        public void AddReview(int reputation)
        {
            var validReputation = reputation < 0 ? 0 : reputation > 5 ? 5 : reputation; // от 0 до 5
            _reviews.Add(validReputation);
            CoffeeReputationChanged?.Invoke(CoffeeReputation);
        }
        public void AddMoney(float moneys)
        {
            if (moneys > 0)
            {
                _coffeeRevenue += moneys;
                CoffeeRevenueChanged?.Invoke(_coffeeRevenue);
            }
        }
        public void Clear()
        {
            _reviews.Clear();
            _reviews.Add(4);
            IsCoffeeInPlayerHands = false;
        }
        private readonly List<int> _reviews = new() { 4 };
        private float _coffeeRevenue = 0;
        private bool _isCoffeeInPlayerHands = false;
    }
}
