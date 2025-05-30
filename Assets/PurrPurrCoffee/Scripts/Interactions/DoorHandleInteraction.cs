using KarenKrill.InteractionSystem.Abstractions;
using KarenKrill.InteractionSystem;
using UnityEngine;

namespace PurrPurrCoffee.Interactions
{
    [RequireComponent(typeof(Outline))]
    public class DoorHandleInteraction : InteractableBase, IInteractable
    {
        protected override void OnInteraction()
        {
            if (_doorOpener.IsOpen)
            {
                _doorOpener.Close();
            }
            else
            {
                _doorOpener.Open();
            }
        }
        protected override void OnInteractionAvailabilityChanged(bool available)
        {
            Debug.Log($"{gameObject.name} InteractionAvailability changed to {available}");
            _outline.enabled = available;
        }

        [SerializeField]
        private DoorOpener _doorOpener;
        private Outline _outline;

        private void Awake()
        {
            _outline = GetComponent<Outline>();
        }
    }
}
