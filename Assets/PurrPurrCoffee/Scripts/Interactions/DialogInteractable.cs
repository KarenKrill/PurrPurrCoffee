using UnityEngine.Events;
using Zenject;

using KarenKrill.InteractionSystem.Abstractions;
using KarenKrill.Storytelling.Abstractions;
using UnityEngine;

namespace PurrPurrCoffee.Interactions.Assets.PurrPurrCoffee.Scripts.Interactions
{
    public class DialogInteractable : OutlineInteractableBase, IInteractable
    {
        public UnityEvent InteractionEvent = new();
        public UnityEvent DialogEnded = new();

        [Inject]
        public void Initialize(IDialogueService dialogueService, IDialogueProvider dialogueProvider)
        {
            _dialogueService = dialogueService;
            _dialogueProvider = dialogueProvider;
        }

        protected override void OnInteraction()
        {
            InteractionEvent.Invoke();
            _dialogueProvider.DialogueEnded += OnDialogueEnded;
            _dialogueService.StartDialogue(_dialogueId);
            // история отвечает за сюжет, то есть:
            // она даёт команду персонажам прийти в кофейню,
            // мониторит указанные события и выполняет указанные действия
            // DialogueService позволяет кому удобно мониторить диалоги, и влиять на них.

            // начать диалог с персонажем
            // -- презентер выключает инпут пользователя

        }

        private void OnDialogueEnded(int obj)
        {
            _dialogueProvider.DialogueEnded -= OnDialogueEnded;
            DialogEnded.Invoke();
        }

        [SerializeField]
        private int _dialogueId = 0;

        private IDialogueService _dialogueService;
        private IDialogueProvider _dialogueProvider;
    }
}
