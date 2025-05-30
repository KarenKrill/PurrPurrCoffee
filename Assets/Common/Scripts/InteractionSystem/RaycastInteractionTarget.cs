using UnityEngine;

namespace KarenKrill.InteractionSystem
{
    using Abstractions;

    [RequireComponent(typeof(Collider))]
    public class RaycastInteractionTarget : InteractionTargetBase, IInteractionTarget
    {
        public override IInteractable Interactable => _interactable;

        [SerializeField]
        private InteractableBase _interactable;
    }
}
