using UnityEngine;

using KarenKrill.InteractionSystem;
using KarenKrill.InteractionSystem.Abstractions;

namespace PurrPurrCoffee.Interactions
{
    public class PickupInteractor : InteractorBase, IInteractor
    {
        public PickupInteractable PickedInteractable => _pickedInteractable;
        public void DropIfPicked(bool usePushDrop = false)
        {
            if (_pickedInteractable != null)
            {
                Drop(usePushDrop);
            }
        }

#nullable enable
        protected override void OnInteraction(IInteractable? interactable)
        {
            if (interactable is null && _pickedInteractable != null)
            {
                Drop(_usePushDrop);
            }
            else if (interactable is PickupInteractable pickupInteractable)
            {
                if (_pickedInteractable != null)
                {
                    Drop(_usePushDrop);
                }
                if (pickupInteractable != _pickedInteractable)
                {
                    Grab(pickupInteractable);
                }
            }
        }
#nullable restore
        protected override void OnInteractionAvailabilityChanged(IInteractable interactable, bool available)
        {
            if (interactable is PickupInteractable pickupInteractable)
            {
                //Debug.Log($"Press E to pickup {pickupInteractable.name}");
            }
        }
        protected virtual void OnGrab(PickupInteractable pickupInteractable) { }
        protected virtual void OnDrop(PickupInteractable pickupInteractable) { }

        [SerializeField]
        private Transform _grabPointTransform;
        [SerializeField]
        private Transform _eyesTransform;
        [SerializeField]
        private Transform _lookPointTransform;
        [SerializeField]
        private bool _usePushDrop = true;
        [SerializeField, Min(0)]
        private float _pushDropSpeed = 2;

        private PickupInteractable _pickedInteractable = null;
        private readonly Collider[] _overlapBoxNonAllocResults = new Collider[1];
        // Picked item previous parameters:
        private bool _pickedItemPrevUseGravity;
        private bool _pickedItemPrevIsKinematic;
        private RigidbodyConstraints _pickedItemPrevConstraints;
        private RigidbodyInterpolation _pickedItemPrevInterpolation;
        private Quaternion _pickedItemPrevRotation;
        private Transform _pickedItemPrevParentTransform;

        private bool IsOverlappingAny(in Bounds bounds, in Quaternion rotation)
        {
            return Physics.OverlapBoxNonAlloc(bounds.center, bounds.extents, _overlapBoxNonAllocResults, rotation) > 0;
        }
        private void Grab(PickupInteractable pickupInteractable)
        {
            _pickedInteractable = pickupInteractable;

            _pickedItemPrevUseGravity = _pickedInteractable.Rigidbody.useGravity;
            _pickedItemPrevIsKinematic = _pickedInteractable.Rigidbody.isKinematic;
            _pickedItemPrevConstraints = _pickedInteractable.Rigidbody.constraints;
            _pickedItemPrevInterpolation = _pickedInteractable.Rigidbody.interpolation;
            _pickedItemPrevParentTransform = _pickedInteractable.transform.parent;
            _pickedItemPrevRotation = _pickedInteractable.transform.rotation;

            _pickedInteractable.Rigidbody.Sleep();
            _pickedInteractable.Rigidbody.useGravity = false;
            _pickedInteractable.Rigidbody.isKinematic = false;
            _pickedInteractable.Rigidbody.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
            _pickedInteractable.Rigidbody.interpolation = RigidbodyInterpolation.None;
            _pickedInteractable.transform.parent = _grabPointTransform;
            _pickedInteractable.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            OnGrab(_pickedInteractable);
        }
        private void Drop(bool usePushDrop = false)
        {
            _pickedInteractable.Rigidbody.useGravity = _pickedItemPrevUseGravity;//true;
            _pickedInteractable.Rigidbody.isKinematic = _pickedItemPrevIsKinematic;// false;
            _pickedInteractable.Rigidbody.constraints = _pickedItemPrevConstraints;// RigidbodyConstraints.FreezeRotation;
            _pickedInteractable.Rigidbody.interpolation = _pickedItemPrevInterpolation;// RigidbodyInterpolation.Extrapolate;
            _pickedInteractable.transform.parent = _pickedItemPrevParentTransform;
            _pickedInteractable.Rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            var dropItemCollider = _pickedInteractable.GetComponent<Collider>();
            
            if (IsOverlappingAny(dropItemCollider.bounds, _pickedInteractable.Rigidbody.rotation))
            {
                var direction = _lookPointTransform.position - _grabPointTransform.position;
                var ray = new Ray(_grabPointTransform.position, direction);
                _pickedInteractable.Rigidbody.position = ray.GetPoint(direction.magnitude * .1f);
            }
            _pickedInteractable.transform.rotation = _pickedItemPrevRotation;
            _pickedInteractable.Rigidbody.rotation = _pickedItemPrevRotation;
            if (usePushDrop)
            {
                var direction = _lookPointTransform.position - _eyesTransform.position;
                _pickedInteractable.Rigidbody.AddForce(direction.normalized * _pushDropSpeed, ForceMode.VelocityChange);
            }
            _pickedInteractable.Rigidbody.WakeUp();

            OnDrop(_pickedInteractable);
            _pickedInteractable = null;
        }
    }
}
