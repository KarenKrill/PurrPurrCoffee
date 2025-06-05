using UnityEngine;
using Zenject;

using KarenKrill.InteractionSystem;
using KarenKrill.InteractionSystem.Abstractions;

namespace PurrPurrCoffee.Interactions
{
    using Input.Abstractions;

    public class RaycastInteractionDetector : RaycastInteractionDetectorBase
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
            //var cameraTransform = Camera.main.transform;
            //var ray = new Ray(cameraTransform.position, cameraTransform.forward);
            //var ray = new Ray(_interactor.transform.position, _interactor.transform.forward);
            var ray = new Ray(_interactorEyePoint.position, _interactorLookPoint.position - _interactorEyePoint.position);
            OnLookChanged(_interactor, ray);
        }
        private void OnInteract() => OnInteract(_interactor);

        [SerializeField]
        private InteractorBase _interactor;
        [SerializeField]
        private Transform _interactorEyePoint;
        [SerializeField]
        private Transform _interactorLookPoint;
    }
}
