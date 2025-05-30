#nullable enable

using System;

namespace KarenKrill.InteractionSystem.Abstractions
{
    public interface IInteractable
    {
        event Action? Interaction;
        event Action<bool>? InteractionAvailabilityChanged;

        void Interact();
        void SetInteractionAvailability(bool available = true);
    }
}
