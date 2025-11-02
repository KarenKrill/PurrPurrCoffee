using KarenKrill.UniCore.Interactions.Abstractions;

namespace PurrPurrCoffee.Interactions
{
    public class CoffeeCupLidInteractable : PickupInteractable, IInteractable
    {
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
