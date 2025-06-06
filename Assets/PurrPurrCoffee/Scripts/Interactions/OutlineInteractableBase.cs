using UnityEngine;

using KarenKrill.InteractionSystem.Abstractions;
using KarenKrill.InteractionSystem;

namespace PurrPurrCoffee.Interactions
{
    [RequireComponent(typeof(Outline))]
    public abstract class OutlineInteractableBase : InteractableBase, IInteractable
    {
        protected override void OnInteractionAvailabilityChanged(bool available)
        {
            //Debug.Log($"{gameObject.name} InteractionAvailability changed to {available}");
            _outline.enabled = available;
        }

        private Outline _outline;

        protected virtual void Awake()
        {
            _outline = GetComponent<Outline>();
            _outline.enabled = false;
        }
    }
}
