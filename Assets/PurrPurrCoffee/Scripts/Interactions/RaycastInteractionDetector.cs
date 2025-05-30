using UnityEngine;
using Zenject;

using KarenKrill.InteractionSystem;
using KarenKrill.InteractionSystem.Abstractions;

namespace PurrPurrCoffee.Interactions
{
    using Input.Abstractions;

    public class RaycastInteractionDetector : RaycastInteractionDetectorBase, IInteractionDetector
    {
        [Inject]
        public void Initialize(IInputActionService inputActionsService)
        {
            _inputActionsService = inputActionsService;
        }

        protected override void InputSubscribe()
        {
            _inputActionsService.Look += OnLook;
            _inputActionsService.Interact += OnInteract;
        }
        protected override void InputUnsubscribe()
        {
            _inputActionsService.Look -= OnLook;
            _inputActionsService.Interact -= OnInteract;
        }

        private IInputActionService _inputActionsService;

        private void OnLook(Vector2 lookDelta)
        {
            var cameraTransform = Camera.main.transform;
            var ray = new Ray(cameraTransform.position, cameraTransform.forward);
            OnLookChanged(ray);
        }
    }
}
