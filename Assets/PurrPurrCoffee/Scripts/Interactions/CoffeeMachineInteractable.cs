using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Cysharp.Threading.Tasks;

using KarenKrill.InteractionSystem.Abstractions;

using PurrPurrCoffee.InventorySystem;

namespace PurrPurrCoffee.Interactions
{
    [RequireComponent(typeof(AudioSource))]
    public class CoffeeMachineInteractable : DialogInteractable, IInteractable
    {
        protected override void Awake()
        {
            base.Awake();
            _audioSource = GetComponent<AudioSource>();
        }
        protected override bool OnInteraction(IInteractor interactor)
        {
            if (!_isWorking)
            {
                //_coffeeCupInteractable = _coffeeCupParentTransform.GetComponentInChildren<CoffeeCupInteractable>();
                if (_coffeeCupInteractable != null)
                {
                    if (Mathf.Abs(_coffeeCupInteractable.CoffeeCupItem.Fullness) <= float.Epsilon)
                    {
                        return base.OnInteraction(interactor);
                    }
                    else
                    {
                        Debug.LogWarning("Coffee in cup already");
                    }
                }
                else if (interactor is PickupInteractor pickupInteractor)
                {
                    if (pickupInteractor.PickedInteractable != null && pickupInteractor.PickedInteractable is CoffeeCupInteractable coffeeCupInteractable)
                    {
                        pickupInteractor.DropIfPicked(false);
                        ConnectWithCup(coffeeCupInteractable);
                        return false;
                    }
                }
            }
            return false;
        }
        protected override void OnDialogueEnded(string id)
        {
            _ = UniTask.RunOnThreadPool(async () => await MakeCoffeeAsync());
            base.OnDialogueEnded(id);
        }

        [SerializeField]
        private Transform _coffeeCupParentTransform;
        [SerializeField]
        private List<AudioClip> _audioClips = new();
        private AudioSource _audioSource;
        private CoffeeCupInteractable _coffeeCupInteractable = null;
        private Transform _coffeeCupPrevParrent;
        private bool _isWorking = false;

        private void ConnectWithCup(CoffeeCupInteractable coffeeCupInteractable)
        {
            Debug.Log("Coffee binded to coffee machine");
            _coffeeCupInteractable = coffeeCupInteractable;
            _coffeeCupInteractable.Interaction += OnCoffeeCupInteraction;
            _coffeeCupPrevParrent = _coffeeCupInteractable.transform.parent;
            _coffeeCupInteractable.transform.parent = _coffeeCupParentTransform;
            _coffeeCupInteractable.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        private void OnCoffeeCupInteraction(IInteractor interactor)
        {
            Debug.Log("Coffee unbinded from coffee machine");
            _coffeeCupInteractable.Interaction -= OnCoffeeCupInteraction;
            _coffeeCupInteractable.transform.parent = _coffeeCupPrevParrent;
            _coffeeCupInteractable = null;
        }

        public async UniTask MakeCoffeeAsync()
        {
            _isWorking = true;
            try
            {
                await UniTask.SwitchToMainThread();
                var servedBase = GetDialogueVariableValue("served_base");
                var servedSyrup = GetDialogueVariableValue("served_syrup");
                Debug.Log($"Served base+syrup: {servedBase}, {servedSyrup}");
                if (Enum.TryParse<CoffeeBase>(servedBase, out var coffeeBase))
                {
                    _coffeeCupInteractable.CoffeeCupItem.Base = coffeeBase;
                }
                if (Enum.TryParse<CoffeeSyrup>(servedBase, out var coffeeSyrup))
                {
                    _coffeeCupInteractable.CoffeeCupItem.Syrup = coffeeSyrup;
                }
                var totalSeconds = _audioClips.Sum(clip => clip.length);
                var passedSeconds = 0f;
                foreach (var audioClip in _audioClips)
                {
                    await UniTask.SwitchToMainThread();
                    _audioSource.PlayOneShot(audioClip);
                    var audioClipLength = audioClip.length;
                    for (int i = 0; i < 10; i++) // progress report cycle
                    {
                        var coffeeCupInteractable = _coffeeCupInteractable;
                        await UniTask.SwitchToMainThread();
                        if (coffeeCupInteractable == null)
                        {
                            _audioSource.Stop();
                            return;
                        }
                        await UniTask.SwitchToThreadPool();
                        passedSeconds += audioClipLength / 10f;
                        coffeeCupInteractable.CoffeeCupItem.Fullness = passedSeconds / totalSeconds;
                        await UniTask.Delay((int)(audioClipLength * 100));
                    }
                }
                _coffeeCupInteractable.CoffeeCupItem.Fullness = 1;
            }
            finally
            {
                _isWorking = false;
            }
        }
    }
}
