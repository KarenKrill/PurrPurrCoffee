using System.Collections.Generic;

using UnityEngine;

using Zenject;

using KarenKrill.InteractionSystem.Abstractions;

namespace PurrPurrCoffee.Interactions
{
    public class InfinitePickupInteractable : OutlineInteractableBase, IInteractable
    {
        [Inject]
        public void Initialize(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        protected override bool OnInteraction(IInteractor interactor)
        {
            PickupInteractable pickupInteractable;
            if (_pickupInteractables.Count >= _maxPoolLength)
            {
                pickupInteractable = _pickupInteractables.Dequeue();
                pickupInteractable.gameObject.SetActive(false);
                _pickupInteractables.Enqueue(pickupInteractable);
                pickupInteractable.transform.SetParent(transform);
                pickupInteractable.transform.localPosition = Vector3.zero;
                pickupInteractable.gameObject.SetActive(true);
            }
            else
            {
                var instance = _instantiator.InstantiatePrefab(_prefab, transform);
                pickupInteractable = instance.GetComponent<PickupInteractable>();
                _pickupInteractables.Enqueue(pickupInteractable);
            }
            interactor.Interact(pickupInteractable);
            return false;
        }

        [SerializeField]
        private PickupInteractable _prefab;
        [SerializeField, Min(1)]
        private int _maxPoolLength = 10;
        private IInstantiator _instantiator;
        private readonly Queue<PickupInteractable> _pickupInteractables = new();
    }
}
