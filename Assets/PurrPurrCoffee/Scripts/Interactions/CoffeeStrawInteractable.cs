using KarenKrill.InteractionSystem.Abstractions;

namespace PurrPurrCoffee.Interactions
{
    public class CoffeeStrawInteractable : PickupInteractable, IInteractable
    {
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
