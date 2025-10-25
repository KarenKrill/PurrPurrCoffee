using System;
using UnityEngine;

using KarenKrill.UI.Presenters.Abstractions;
using KarenKrill.UI.Views.Abstractions;
using KarenKrill.Storytelling.Abstractions;

namespace PurrPurrCoffee.UI.Presenters
{
    using Abstractions;
    using System.Linq;
    using Views.Abstractions;

    public class DialoguePresenter : PresenterBase<IDialogueView>, IDialoguePresenter, IPresenter<IDialogueView>
    {
        public bool ShowInteractionTooltip { get; set; }
#nullable enable
        public event Action<int>? ChoiceMade;
        public event Action? Continued;
        public event Action? Skipped;
#nullable restore

        public DialoguePresenter(IViewFactory viewFactory,
            IPresenterNavigator navigator,
            IDialogueProvider dialogueProvider,
            IDialogueService dialogueService) : base(viewFactory, navigator)
        {
            _dialogueProvider = dialogueProvider;
            _dialogueService = dialogueService;
        }

        protected override void Subscribe()
        {
            OnDialogueStateUpdate(_dialogueProvider.DialogueState);
            _dialogueProvider.DialogueStateChanged += OnDialogueStateUpdate;
            View.ChoiceMade += OnChoiceMade;
            View.NextLineRequested += OnNextLineRequested;
            View.SkipRequested += OnSkipRequested;
            View.ShowInteractionTooltip = ShowInteractionTooltip;
        }
        protected override void Unsubscribe()
        {
            View.ChoiceMade -= OnChoiceMade;
            View.NextLineRequested -= OnNextLineRequested;
            View.SkipRequested -= OnSkipRequested;
            _dialogueProvider.DialogueStateChanged -= OnDialogueStateUpdate;
        }

        private readonly IDialogueProvider _dialogueProvider;
        private readonly IDialogueService _dialogueService;

        private void OnChoiceMade(int index)
        {
            _dialogueService.MakeDialogueChoice(index);
            ChoiceMade?.Invoke(index);
        }
        private void OnNextLineRequested()
        {
            _dialogueService.NextDialogueLine();
            Continued?.Invoke();
        }
        private void OnSkipRequested()
        {
            _dialogueService.SkipDialogue();
            Skipped?.Invoke();
        }
        private void OnDialogueStateUpdate(DialogueState currentDialogueState)
        {
            if (currentDialogueState != null)
            {
                View.ActorName = currentDialogueState.FlowName;
                View.Line = currentDialogueState.Line;
                View.Choices = currentDialogueState.Choices.ToArray();
                var isLineEmpty = string.IsNullOrEmpty(currentDialogueState.Line);
                var isChoicesEmpty = currentDialogueState.Choices?.Count > 0;
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