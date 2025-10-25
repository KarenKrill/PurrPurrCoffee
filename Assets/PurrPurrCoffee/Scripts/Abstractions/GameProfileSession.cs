using System;
using System.Collections.Generic;
using System.Linq;

using PurrPurrCoffee.InventorySystem.Abstractions;

namespace PurrPurrCoffee.Abstractions
{
    public class GameProfileSession
    {
        public float Reputation => (float)_reviews.Average();
        public float Revenue => _revenue;
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
        public bool IsPlayerWin { get; set; } = false;
        public bool IsPlayerLose { get; set; } = false;

        public Inventory PlayerInventory { get; } = new();

        public event Action<float> ReputationChanged;
        public event Action<float> RevenueChanged;
        public event Action<bool> IsCoffeeInPlayerHandsChanged;

        public void AddReview(int reputation)
        {
            var validReputation = reputation < 0 ? 0 : reputation > 5 ? 5 : reputation; // от 0 до 5
            _reviews.Add(validReputation);
            ReputationChanged?.Invoke(Reputation);
        }
        public void AddMoney(float moneys)
        {
            if (moneys > 0)
            {
                _revenue += moneys;
                RevenueChanged?.Invoke(_revenue);
            }
        }
        public void Clear()
        {
            _reviews.Clear();
            _reviews.Add(4);
            IsCoffeeInPlayerHands = false;
        }

        private readonly List<int> _reviews = new() { 4 };
        private float _revenue = 0;
        private bool _isCoffeeInPlayerHands = false;
    }
}
