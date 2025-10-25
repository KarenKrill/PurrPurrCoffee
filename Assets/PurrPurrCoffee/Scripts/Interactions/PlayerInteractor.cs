using System.Linq;

using UnityEngine;

using Zenject;

using KarenKrill.InteractionSystem.Abstractions;

using PurrPurrCoffee.Abstractions;
using PurrPurrCoffee.InventorySystem.Abstractions;

namespace PurrPurrCoffee.Interactions
{
    public class PlayerInteractor : PickupInteractor, IInteractor, IInventoryHolder
    {
        public Inventory Inventory => _gameProfileSession.PlayerInventory;

        [Inject]
        public virtual void Initialize(GameProfileSession gameProfileSession)
        {
            _gameProfileSession = gameProfileSession;
        }

        protected override void OnGrab(PickupInteractable pickupInteractable)
        {
            if (pickupInteractable is IInventoryItemHolder inventoryItemHolder)
            {
                Inventory.AddItem(inventoryItemHolder.InventoryItem);
                Debug.LogError($"Inventory: {string.Join(", ", Inventory.Items.Select(item => item.GetType().Name))}");
            }
        }
        protected override void OnDrop(PickupInteractable pickupInteractable)
        {
            if (pickupInteractable is IInventoryItemHolder inventoryItemHolder)
            {
                Inventory.RemoveItem(inventoryItemHolder.InventoryItem);
                Debug.LogError($"Inventory: {string.Join(", ", Inventory.Items.Select(item => item.GetType().Name))}");
            }
        }

        private GameProfileSession _gameProfileSession;
    }
}
