using System;
using UnityEngine;

namespace KarenKrill.InteractionSystem
{
    using Abstractions;
    using Zenject;

    public abstract class RaycastInteractionDetectorBase : MonoBehaviour
    {
        [Inject]
        public void Initialize(IInteractionTargetRegistry interactionTargetRegistry)
        {
            _interactionTargetRegistry = interactionTargetRegistry;
        }

        protected abstract void InputSubscribe();
        protected abstract void InputUnsubscribe();
        protected virtual void OnEnable()
        {
            InputSubscribe();
            _interactionTargetRegistry.Unregistred += OnInteractionTargetUnregistred;
        }
        protected virtual void OnDisable()
        {
            InputUnsubscribe();
            _interactionTargetRegistry.Unregistred -= OnInteractionTargetUnregistred;
        }
        protected void OnLookChanged(IInteractor interactor, Ray ray)
        {
            var hitsCount = Physics.RaycastNonAlloc(ray, _cachedRaycastHits, _detectDistance, InteractableLayer);
            for (int i = 0; i < hitsCount; i++)
            {
                if (_cachedRaycastHits[i].collider.TryGetComponent<IInteractionTarget>(out var interactionTarget))
                {
                    if (_interactionTargetRegistry.Contains(interactionTarget))
                    {
                        OnTargetFocused(interactionTarget, interactor);
                        return;
                    }
                }
            }
            /*if (Physics.Raycast(ray, out var hitInfo, _detectDistance, InteractableLayer))
            {
                if (hitInfo.collider.TryGetComponent<IInteractionTarget>(out var interactionTarget))
                {
                    if (_interactionTargetRegistry.Contains(interactionTarget))
                    {
                        OnTargetFocused(interactionTarget, interactor);
                        return;
                    }
                }
            }*/
            OnTargetLostFocus();

        }
        protected void OnInteract(IInteractor interactor)
        {
            _lastAvailableInteractionTarget?.Interactable.Interact(interactor);
            interactor?.Interact(_lastAvailableInteractionTarget?.Interactable);
        }

        [SerializeField]
        private float _detectDistance = 1;
        [SerializeField]
        private LayerMask InteractableLayer;

        private IInteractionTargetRegistry _interactionTargetRegistry;
        private IInteractionTarget _lastAvailableInteractionTarget = null;
        private IInteractor _lastInteractor = null;
        private const int CACHED_RAYCAST_HITS_COUNT = 5;
        private readonly RaycastHit[] _cachedRaycastHits = new RaycastHit[CACHED_RAYCAST_HITS_COUNT];

        private void OnTargetFocused(IInteractionTarget interactionTarget, IInteractor interactor)
        {
            _lastAvailableInteractionTarget?.Interactable.SetInteractionAvailability(_lastInteractor, false);
            _lastInteractor?.SetInteractionAvailability(_lastAvailableInteractionTarget?.Interactable, false);
            _lastAvailableInteractionTarget = interactionTarget;
            _lastInteractor = interactor;
            interactionTarget.Interactable.SetInteractionAvailability(interactor, true);
            interactor.SetInteractionAvailability(_lastAvailableInteractionTarget?.Interactable, true);
        }
        private void OnTargetLostFocus()
        {
            // target not found:
            _lastAvailableInteractionTarget?.Interactable.SetInteractionAvailability(_lastInteractor, false);
            _lastInteractor?.SetInteractionAvailability(_lastAvailableInteractionTarget?.Interactable, false);
            _lastAvailableInteractionTarget = null;
            _lastInteractor = null;
        }
        private void OnInteractionTargetUnregistred(IInteractionTarget target)
        {
            if (_lastAvailableInteractionTarget == target)
            {
                OnTargetLostFocus();
            }
        }
    }
}
