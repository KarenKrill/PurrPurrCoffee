using System;
using System.Collections.Generic;
using System.Linq;
using Ink.Runtime;

using KarenKrill.Storytelling.Abstractions;
using UnityEngine;

namespace PurrPurrCoffee.Storytelling
{
    public class DialogueService : IDialogueService, IDialogueProvider
    {
        public event Action<int, int, float> ClientServed;
        private const string ReputationTag = "rep_change:";
        private const string MoneyTag = "money:";
        public DialogueState CurrentDialogueState
        {
            get => _currentDialogueState;
            private set
            {
                if (_currentDialogueState != value)
                {
                    _currentDialogueState = value;
                    DialogueStateChanged?.Invoke(_currentDialogueState);
                }
            }
        }

        public event Action<DialogueState> DialogueStateChanged;
        public event Action<int> DialogueStarting;
        public event Action<int> DialogueStarted;
        public event Action<int> DialogueEnded;

        public DialogueService(string story_temp)
        {
            _story = new(story_temp);
            _story.ResetState();
            //_story.SwitchFlow("shift_1");
            //_story.ChoosePath("shift_1");// variablesState["shift"] = 1;
        }
        List<string> _characters = new() { "Кофемашина", "Мария", "Игорь", "Ольга", "Игрок", "Игрок" }; // не успеваю вынести в конфиг
        List<string> _storyFlows = new() { "coffee_machine", "client_maria", "client_igor", "client_olga", "player_end_shift_1", "player_shift_1_intro" };
        int _currentCharacterId;
        public void StartDialogue(int id)
        {
            _currentCharacterId = id;
            //_story.variablesState["client"] = _characters[id];
            _story.ChoosePathString(_storyFlows[id]);
            Debug.LogError($"StartDialogue flow {_storyFlows[id]}/{_story.currentFlowName}");
            DialogueStarting?.Invoke(id);
            ContinueStory();
            DialogueStarted?.Invoke(id);
        }
        public void MakeDialogueChoice(int index)
        {
            if (index < _story.currentChoices.Count)
            {
                _story.ChooseChoiceIndex(index);
                //_story.Continue(); // skip choosen text
                ContinueStory(); // move to next
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
        }
        public void NextDialogueLine() => ContinueStory();
        public void SkipDialogue()
        {
            throw new NotImplementedException();
            OnStoryEnd();
        }
        public void SetVariable(string name, bool state = true)
        {
            _story.variablesState[name] = state;
        }

        private readonly Story _story;
        private DialogueState _currentDialogueState;

        private void ContinueStory()
        {
            bool isCanContinue = _story.canContinue;
            bool isStoryEnds = false;
            var choices = _story.currentChoices;
            if (isCanContinue)
            {
                var sentence = _story.Continue();
                var tags = _story.currentTags;
                if (tags.Count > 0)
                {
                    int reputationDelta = 0;
                    int money = 0;
                    foreach (var tag in tags)
                    {
                        if (tag.StartsWith(ReputationTag))
                        {
                            _ = int.TryParse(tag[ReputationTag.Length..], out reputationDelta);
                        }
                        if (tag.StartsWith(MoneyTag))
                        {
                            _ = int.TryParse(tag[MoneyTag.Length..], out money);
                        }
                    }
                    if (reputationDelta != 0 || money > 0)
                    {
                        ClientServed?.Invoke(_currentCharacterId, reputationDelta, money);
                    }
                    if (!_story.canContinue)
                    {
                        isStoryEnds = true;
                    }
                }
                else if (string.IsNullOrEmpty(sentence) && !_story.canContinue && _story.currentChoices.Count == 0)
                {
                    // костыль, надо придумать получше
                    // проблема: пустой диалог в конце сюжетного узла
                    isStoryEnds = true;
                }
                else
                {
                    CurrentDialogueState = new(sentence, Array.Empty<string>(), _characters[_currentCharacterId]);
                }
            }
            else if (choices.Count > 0)
            {
                CurrentDialogueState = new(_story.currentText, choices.Select(choice => choice.text).ToArray(), _characters[_currentCharacterId]);
            }
            else
            {
                isStoryEnds = true;
            }
            if(isStoryEnds)
            {
                CurrentDialogueState = new(string.Empty, Array.Empty<string>(), _characters[_currentCharacterId]);
                OnStoryEnd();
            }
        }
        private void OnStoryEnd() => DialogueEnded?.Invoke(_currentCharacterId);
    }
}
