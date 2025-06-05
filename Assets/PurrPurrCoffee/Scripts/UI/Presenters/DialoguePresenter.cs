using System;
using UnityEngine;

using KarenKrill.UI.Presenters.Abstractions;
using KarenKrill.UI.Views.Abstractions;
using KarenKrill.Storytelling.Abstractions;

namespace PurrPurrCoffee.UI.Presenters
{
    using Abstractions;
    using Views.Abstractions;

    public class DialoguePresenter : PresenterBase<IDialogueView>, IDialoguePresenter, IPresenter<IDialogueView>
    {
#nullable enable
        public event Action<int>? ChoiceMade;
        public event Action? NextLineRequested;
        public event Action? SkipRequested;
#nullable restore

        public DialoguePresenter(IViewFactory viewFactory,
            IPresenterNavigator navigator,
            IDialogueProvider dialogueProvider) : base(viewFactory, navigator)
        {
            _dialogueProvider = dialogueProvider;
        }

        protected override void Subscribe()
        {
            OnDialogueStateUpdate(_dialogueProvider.CurrentDialogueState);
            _dialogueProvider.DialogueStateChanged += OnDialogueStateUpdate;
            View.ChoiceMade += OnChoiceMade;
            View.NextLineRequested += OnNextLineRequested;
            View.SkipRequested += OnSkipRequested; 
        }
        protected override void Unsubscribe()
        {
            View.ChoiceMade -= OnChoiceMade;
            View.NextLineRequested -= OnNextLineRequested;
            View.SkipRequested -= OnSkipRequested;
            _dialogueProvider.DialogueStateChanged -= OnDialogueStateUpdate;
        }

        private readonly IDialogueProvider _dialogueProvider;

        private void OnChoiceMade(int index) => ChoiceMade?.Invoke(index);
        private void OnNextLineRequested() => NextLineRequested?.Invoke();
        private void OnSkipRequested() => SkipRequested?.Invoke();
        private void OnDialogueStateUpdate(DialogueState currentDialogueState)
        {
            if (currentDialogueState != null)
            {
                View.ActorName = currentDialogueState.Name_Temp;
                View.Line = currentDialogueState.Line;
                View.Choices = currentDialogueState.Choices;
                var isLineEmpty = string.IsNullOrEmpty(currentDialogueState.Line);
                var isChoicesEmpty = currentDialogueState.Choices?.Length > 0;
                //View.Mode = isChoicesEmpty ? (isLineEmpty ? DialogueMode.Choices : DialogueMode.Both) : DialogueMode.Line;
                View.Mode = isChoicesEmpty ? DialogueMode.Choices : DialogueMode.Line;
            }
            else
            {
                View.ActorName = "...";
                View.Line = string.Empty;
                View.Choices = Array.Empty<string>();
                View.Mode = DialogueMode.Line;
                Debug.LogError($"{nameof(OnDialogueStateUpdate)}: {currentDialogueState.Line} [{string.Join(", ", currentDialogueState.Choices)}]");
            }
        }
    }
}