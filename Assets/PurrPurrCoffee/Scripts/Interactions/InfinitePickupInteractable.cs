using KarenKrill.InteractionSystem.Abstractions;

namespace PurrPurrCoffee.Interactions
{
    public class InfinitePickupInteractable : PickupInteractable, IInteractable
    {
        protected override void OnInteraction()
        {
            base.OnInteraction();
            if (_isClonable)
            {
                _isClonable = false; // подбираем именно этот объект, а клон подменяет оригинал
                var cloneGameObject = Instantiate(gameObject, transform.position, transform.rotation, transform.parent);
                this.transform.rotation = transform.localRotation;
                /*var interactable = cloneGameObject.GetComponent<IInteractable>();
                var interactionTarget = cloneGameObject.GetComponentInChildren<IInteractionTarget>();
                interactionTarget.Interactable = interactable;*/
            }
        }
        private bool _isClonable = true;
    }
}
