#nullable enable

using System;

namespace KarenKrill.InteractionSystem.Abstractions
{
    public interface IInteractable
    {
        event Action<IInteractor>? Interaction;
        event Action<IInteractor, bool>? InteractionAvailabilityChanged;

        void Interact(IInteractor interactor);
        void SetInteractionAvailability(IInteractor interactor, bool available = true);
    }
}
