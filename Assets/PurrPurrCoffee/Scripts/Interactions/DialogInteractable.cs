using UnityEngine;
using UnityEngine.Events;

using Zenject;

using KarenKrill.InteractionSystem.Abstractions;
using KarenKrill.Storytelling.Abstractions;

namespace PurrPurrCoffee.Interactions
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

        protected override bool OnInteraction(IInteractor interactor)
        {
            InteractionEvent.Invoke();
            _dialogueProvider.DialogueEnded += OnDialogueEnded;
            _dialogueService.StartDialogue(_dialogueIdStr);
            // история отвечает за сюжет, то есть:
            // она даёт команду персонажам прийти в кофейню,
            // мониторит указанные события и выполняет указанные действия
            // DialogueService позволяет кому удобно мониторить диалоги, и влиять на них.

            // начать диалог с персонажем
            // -- презентер выключает инпут пользователя
            return true;
        }
        protected virtual void OnDialogueEnded(string id)
        {
            _dialogueProvider.DialogueEnded -= OnDialogueEnded;
            DialogEnded.Invoke();
        }
        protected string GetDialogueVariableValue(string name)
        {
            return _dialogueService.GetVariable(name);
        }

        [SerializeField]
        private string _dialogueIdStr = string.Empty;

        private IDialogueService _dialogueService;
        private IDialogueProvider _dialogueProvider;
    }
}
