using UnityEngine;

namespace PurrPurrCoffee.Interactions
{
    [RequireComponent(typeof(Animator))]
    public class DoorOpener : MonoBehaviour
    {
        public bool IsOpen => _isOpen;
        public void Open(float speedMultiplier = 1)
        {
            if (!_isOpen)
            {
                _animator.SetFloat("Speed", speedMultiplier);
                _animator.SetTrigger("Door_Open");
                _openDoorAudio.Play();
                _isOpen = true;
            }
        }
        public void Close(float speedMultiplier = 1)
        {
            if (_isOpen)
            {
                _animator.SetFloat("Speed", speedMultiplier);
                _animator.SetTrigger("Door_Close");
                _closeDoorAudio.Play();
                _isOpen = false;
            }
        }

        [SerializeField]
        private bool _isOpen = false;
        [SerializeField]
        private AudioSource _openDoorAudio;
        [SerializeField]
        private AudioSource _closeDoorAudio;
        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }
    }
}
