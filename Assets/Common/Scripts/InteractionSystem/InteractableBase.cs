#nullable enable

using System;
using UnityEngine;
using Zenject;

namespace KarenKrill.InteractionSystem
{
    using Abstractions;

    public abstract class InteractableBase : MonoBehaviour, IInteractable
    {
        public event Action? Interaction;
        public event Action<bool>? InteractionAvailabilityChanged;

        public void Interact()
        {
            OnInteraction();
            Interaction?.Invoke();
        }
        public void SetInteractionAvailability(bool available = true)
        {
            OnInteractionAvailabilityChanged(available);
            InteractionAvailabilityChanged?.Invoke(available);
        }

        protected abstract void OnInteraction();
        protected abstract void OnInteractionAvailabilityChanged(bool available);
    }
}
