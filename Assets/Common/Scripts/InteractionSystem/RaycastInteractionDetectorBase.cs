using System;
using System.Collections.Generic;
using UnityEngine;

namespace KarenKrill.InteractionSystem
{
    using Abstractions;

    public abstract class RaycastInteractionDetectorBase : MonoBehaviour, IInteractionDetector
    {
        public void Register(IInteractionTarget interactionTarget)
        {
            _interactionTargets.Add(interactionTarget);
        }
        public void Unregister(IInteractionTarget interactionTarget)
        {
            _interactionTargets.Remove(interactionTarget);
        }

        protected abstract void InputSubscribe();
        protected abstract void InputUnsubscribe();
        protected virtual void OnEnable() => InputSubscribe();
        protected virtual void OnDisable() => InputUnsubscribe();
        protected void OnLookChanged(Ray ray)
        {
            var hitsCount = Physics.RaycastNonAlloc(ray, _cachedRaycastHits, _detectDistance, InteractableLayer);
            for (int i = 0; i < hitsCount; i++)
            {
                if (_cachedRaycastHits[i].collider.TryGetComponent<IInteractionTarget>(out var interactionTarget))
                {
                    if (_interactionTargets.Contains(interactionTarget))
                    {
                        _lastAvailableInteractionTarget?.Interactable.SetInteractionAvailability(false);
                        _lastAvailableInteractionTarget = interactionTarget;
                        _lastAvailableInteractionTarget.Interactable.SetInteractionAvailability(true);
                        return;
                    }
                }
            }
            _lastAvailableInteractionTarget?.Interactable.SetInteractionAvailability(false);
            _lastAvailableInteractionTarget = null;
        }
        protected void OnInteract()
        {
            _lastAvailableInteractionTarget?.Interactable.Interact();
        }

        [SerializeField]
        private float _detectDistance = 1;
        [SerializeField]
        private LayerMask InteractableLayer;

        private readonly HashSet<IInteractionTarget> _interactionTargets = new();
        private IInteractionTarget _lastAvailableInteractionTarget = null;
        private const int CACHED_RAYCAST_HITS_COUNT = 5;
        private readonly RaycastHit[] _cachedRaycastHits = new RaycastHit[CACHED_RAYCAST_HITS_COUNT];
    }
}
