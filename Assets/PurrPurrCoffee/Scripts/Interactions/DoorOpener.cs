using System;
using UnityEngine;
using UnityEngine.Events;

namespace PurrPurrCoffee.Interactions
{
    [RequireComponent(typeof(Animator))]
    public class DoorOpener : MonoBehaviour
    {
        public event Action Opened;
        public event Action Closed;

        public bool IsOpen => _isOpen;
        public void Open(float speedMultiplier = 1)
        {
            if (!_isOpen)
            {
                _animator.SetFloat(_DoorAnimSpeedParamName, speedMultiplier);
                _animator.SetTrigger(_DoorOpenAnimName);
                _openDoorAudio.Play();
                _isOpen = true;
            }
        }
        public void Close(float speedMultiplier = 1)
        {
            if (_isOpen)
            {
                _animator.SetFloat(_DoorAnimSpeedParamName, speedMultiplier);
                _animator.SetTrigger(_DoorCloseAnimName);
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

        private const string _DoorCloseAnimName = "Door_Close";
        private const string _DoorOpenAnimName = "Door_Open";
        private const string _DoorAnimSpeedParamName = "Speed";
        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }
        private void OnDoorOpened(AnimationEvent _) => Opened?.Invoke();
        private void OnDoorClosed(AnimationEvent _) => Closed?.Invoke();
    }
}
