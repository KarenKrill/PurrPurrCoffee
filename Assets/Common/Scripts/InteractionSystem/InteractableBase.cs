#nullable enable

using System;
using UnityEngine;

namespace KarenKrill.InteractionSystem
{
    using Abstractions;

    public abstract class InteractableBase : MonoBehaviour, IInteractable
    {
        public event Action<IInteractor>? Interaction;
        public event Action<IInteractor, bool>? InteractionAvailabilityChanged;

        public void Interact(IInteractor interactor)
        {
            OnInteraction();
            Interaction?.Invoke(interactor);
        }
        public void SetInteractionAvailability(IInteractor interactor, bool available = true)
        {
            OnInteractionAvailabilityChanged(available);
            InteractionAvailabilityChanged?.Invoke(interactor, available);
        }

        protected abstract void OnInteraction();
        protected abstract void OnInteractionAvailabilityChanged(bool available);
    }
}
