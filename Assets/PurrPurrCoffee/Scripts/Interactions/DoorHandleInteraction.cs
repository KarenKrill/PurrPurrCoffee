using UnityEngine;

using KarenKrill.UniCore.Interactions.Abstractions;

namespace PurrPurrCoffee.Interactions
{
    public class DoorHandleInteraction : OutlineInteractableBase, IInteractable
    {
        protected override bool OnInteraction(IInteractor interactor)
        {
            if (_doorOpener.IsOpen)
            {
                _doorOpener.Close();
            }
            else
            {
                _doorOpener.Open();
            }
            return true;
        }

        [SerializeField]
        private DoorOpener _doorOpener;
    }
}
