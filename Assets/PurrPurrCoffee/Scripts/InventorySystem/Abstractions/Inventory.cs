using System;
using System.Collections.Generic;

namespace PurrPurrCoffee.InventorySystem.Abstractions
{
    public class Inventory
    {
        public IReadOnlyList<InventoryItem> Items => _items;

        public event Action<InventoryItem> ItemAdded;
        public event Action<InventoryItem> ItemRemoved;

        public void AddItem(InventoryItem item)
        {
            _items.Add(item);
            ItemAdded?.Invoke(item);
        }
        public void RemoveItem(InventoryItem item)
        {
            _items.Remove(item);
            ItemRemoved?.Invoke(item);
        }

        private readonly List<InventoryItem> _items = new();
    }
}
