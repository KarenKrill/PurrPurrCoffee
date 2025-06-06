using UnityEngine;

using KarenKrill.InteractionSystem.Abstractions;

namespace PurrPurrCoffee.Interactions
{
    public class DoorHandleInteraction : OutlineInteractableBase, IInteractable
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

        [SerializeField]
        private DoorOpener _doorOpener;
    }
}
