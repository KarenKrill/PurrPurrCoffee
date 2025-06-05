using KarenKrill.InteractionSystem;
using KarenKrill.InteractionSystem.Abstractions;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace PurrPurrCoffee.Interactions
{
    public class PickupInteractor : InteractorBase, IInteractor
    {
        public UnityEvent Grabbed = new();
        public UnityEvent Dropped = new();
        public PickupInteractable PickedInteractable => _pickedInteractable;
        public void DropIfPicked()
        {
            if (_pickedInteractable != null)
            {
                Drop();
            }
        }

        protected override void OnInteraction(IInteractable interactable)
        {
            Debug.Log("Interactor: OnInteraction");
            if (interactable is PickupInteractable pickupInteractable)
            {
                Debug.Log("Interactor: IsPickable");
                bool _isNewPickup = pickupInteractable != _pickedInteractable;
                if (_pickedInteractable != null)
                {
#if false // я вам запрещаю дропать, объекты улетают под сцену, исправить не успеваю :(
                    Drop();
#endif
                }
                if(_isNewPickup)
                {
                    Grab(pickupInteractable);
                }
            }
        }
        protected override void OnInteractionAvailabilityChanged(IInteractable interactable, bool available)
        {
            if (interactable is PickupInteractable pickupInteractable)
            {
                //Debug.Log($"Press E to pickup {pickupInteractable.name}");
            }
        }

        [SerializeField]
        private Transform _grabPointTransform;
        [SerializeField]
        private float _pickedItemFollowSpeed = 10;
        /// <summary>
        /// Не обращаем внимания, что PickupInteractor знает про кофе, это всё дедлайны
        /// </summary>
        [SerializeField, Tooltip("Не обращаем внимания, что PickupInteractor знает про кофе, это всё дедлайны")]
        private PickupInteractable _coffeeObject;
        private bool _coffeeIsPicked = false;

        private PickupInteractable _pickedInteractable = null;
        private Transform _pickedItemPrevParentTransform;

        private void FixedUpdate()
        {
            if (_pickedInteractable != null && !_coffeeIsPicked)
            {
                var smoothPosition = Vector3.Lerp(_pickedInteractable.transform.position, _grabPointTransform.position, Time.fixedDeltaTime * _pickedItemFollowSpeed);
                //_pickedInteractable.transform.position = smoothPosition;
                _pickedInteractable.Rigidbody.MovePosition(smoothPosition);
                _pickedInteractable.Rigidbody.rotation = transform.rotation;
            }
        }
        private void Grab(PickupInteractable pickupInteractable)
        {
            _pickedInteractable = pickupInteractable;
            if (pickupInteractable is CoffeeInteractable coffeeInteractable)
            {
                _coffeeIsPicked = true;
                coffeeInteractable.gameObject.SetActive(false);
                _coffeeObject.gameObject.SetActive(true);
            }
            else
            {
                _pickedInteractable.Rigidbody.useGravity = false;
                _pickedInteractable.Rigidbody.freezeRotation = true;
                _pickedInteractable.Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;

                //_pickedItemPrevParentTransform = _pickedInteractable.transform.parent;
                //_pickedInteractable.transform.parent = _grabPointTransform;
            }
            Grabbed.Invoke();
        }
        private void Drop()
        {
            if (_coffeeIsPicked)
            {
                _coffeeIsPicked = false;
                _coffeeObject.gameObject.SetActive(false);
                //_pickedInteractable.gameObject.SetActive(true); активируется через CoffeMachine когда приготовится
            }
            else
            {
                _pickedInteractable.Rigidbody.interpolation = RigidbodyInterpolation.Extrapolate;
                _pickedInteractable.Rigidbody.useGravity = true;
                //_pickedInteractable.Rigidbody.freezeRotation = false;
                //_pickedInteractable.transform.parent = _pickedItemPrevParentTransform;
            }

            Dropped.Invoke();
            _pickedInteractable = null;
        }
    }
}
