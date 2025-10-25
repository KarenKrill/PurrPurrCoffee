using UnityEngine;

using KarenKrill.InteractionSystem.Abstractions;

using PurrPurrCoffee.InventorySystem;
using PurrPurrCoffee.InventorySystem.Abstractions;

namespace PurrPurrCoffee.Interactions
{
    public class CoffeeCupInteractable : PickupInteractable, IInteractable, IInventoryItemHolder
    {
        public InventoryItem InventoryItem => CoffeeCupItem;
        public CoffeeCupItem CoffeeCupItem { get; } = new();

        protected override bool OnInteraction(IInteractor interactor)
        {
            if (interactor is PickupInteractor pickupInteractor &&
                pickupInteractor.PickedInteractable != null)
            {
                switch (pickupInteractor.PickedInteractable)
                {
                    case CoffeeCupLidInteractable coffeeCupLidInteractable:
                        if (!CoffeeCupItem.IsCoveredWithLid)
                        {
                            pickupInteractor.DropIfPicked(false);
                            ConnectWithLid(coffeeCupLidInteractable);
                            return false;
                        }
                        break;
                    case CoffeeStrawInteractable coffeeStrawInteractable:
                        if (!CoffeeCupItem.IsContainsStraw)
                        {
                            pickupInteractor.DropIfPicked(false);
                            ConnectWithStraw(coffeeStrawInteractable);
                            return false;
                        }
                        break;
                    default:
                        break;
                }
            }
            return base.OnInteraction(interactor);
        }

        [SerializeField]
        private GameObject _coffeeCupLidObject;
        [SerializeField]
        private GameObject _coffeeStrawObject;

        private void OnEnable()
        {
            CoffeeCupItem.Fullness = 0;
            CoffeeCupItem.Base = CoffeeBase.Water;
            CoffeeCupItem.IsContainsStraw = _coffeeStrawObject.activeInHierarchy;
            CoffeeCupItem.IsCoveredWithLid = _coffeeCupLidObject.activeInHierarchy;
            CoffeeCupItem.Syrup = CoffeeSyrup.None;
            CoffeeCupItem.GameObject = gameObject;

        }
        private void ConnectWithLid(CoffeeCupLidInteractable coffeeCupLidInteractable)
        {
            _coffeeCupLidObject.SetActive(true);
            CoffeeCupItem.IsCoveredWithLid = true;
            coffeeCupLidInteractable.gameObject.SetActive(false);
        }
        private void ConnectWithStraw(CoffeeStrawInteractable coffeeStrawInteractable)
        {
            _coffeeStrawObject.SetActive(true);
            CoffeeCupItem.IsContainsStraw = true;
            coffeeStrawInteractable.gameObject.SetActive(false);
        }
    }
}
