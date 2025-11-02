#nullable enable

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Ink.Runtime;

using KarenKrill.UniCore.Storytelling.Abstractions;
using UnityEngine;

namespace PurrPurrCoffee.Storytelling
{
    public class DialogueService : IDialogueService, IDialogueProvider
    {
        public bool CanContinue => _story.canContinue;
        public bool IsEnds => _isStoryEnds;
        public event Action<int, int, float>? ClientServed;
        private const string ReputationTag = "rep_change:";
        private const string MoneyTag = "money:";
        public DialogueState DialogueState
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

        public event Action<DialogueState>? DialogueStateChanged;
        public event Action<string>? DialogueStarting;
        public event Action<string>? DialogueStarted;
        public event Action<string>? DialogueEnded;

        public DialogueService(string storyJson, ILogger? logger = null)
        {
            _story = new(storyJson);
            _story.ResetState();
            //_story.SwitchFlow("shift_1");
            //_story.ChoosePath("shift_1");// variablesState["shift"] = 1;
            _logger = logger;
        }
        public void StartDialogue(string id)
        {
            _currentCharacterId = id;
            _story.ChoosePathString(id);
            Debug.LogError($"StartDialogue flow {id}/{_story.currentFlowName}");
            DialogueStarting?.Invoke(id);
            ContinueStory();
            DialogueStarted?.Invoke(id);
        }
        public void MakeDialogueChoice(int index)
        {
            if (index < _story.currentChoices.Count)
            {
                _story.ChooseChoiceIndex(index);
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
            OnStoryFlowEnd();
        }
        public void SetVariable(string name, bool state = true)
        {
            _story.variablesState[name] = state;
        }
        public string GetVariable(string name)
        {
            return _story.variablesState[name].ToString();
        }
        public void AddVariableListener(string varName, Action<object> callback)
        {
            if (_varListeners.TryAdd(varName, callback))
            {
                _story.ObserveVariable(varName, OnVariableChanged);
            }
            else
            {
                _logger?.LogWarning(nameof(DialogueService), $"Can't add listener for \"{varName}\" variable");
            }
        }
        public void RemoveVariableListener(string varName)
        {
            if (_varListeners.TryRemove(varName, out _))
            {
                _story.RemoveVariableObserver(OnVariableChanged, varName);
            }
            else
            {
                _logger?.LogWarning(nameof(DialogueService), $"Can't remove listener for \"{varName}\" variable");
            }
        }

        private readonly Story _story;
        private DialogueState _currentDialogueState = new(string.Empty, string.Empty, Array.Empty<string>(), Array.Empty<string>());
        private bool _isStoryEnds = true;
        private readonly ConcurrentDictionary<string, Action<object>> _varListeners = new();
        private readonly ILogger? _logger;

        List<string> _characters = new() { "Кофемашина", "Мария", "Игорь", "Ольга", "Игрок", "Игрок" }; // не успеваю вынести в конфиг
        List<string> _storyFlows = new() { "coffee_machine", "client_maria", "client_igor", "client_olga", "player_end_shift_1", "player_shift_1_intro" };
        string _currentCharacterId = string.Empty;

        private void ContinueStory()
        {
            bool isCanContinue = _story.canContinue;
            _isStoryEnds = false;
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
                        ClientServed?.Invoke(_storyFlows.IndexOf(_currentCharacterId), reputationDelta, money);
                    }
                    if (!_story.canContinue)
                    {
                        _isStoryEnds = true;
                    }
                }
                else if (string.IsNullOrEmpty(sentence) && !_story.canContinue && _story.currentChoices.Count == 0)
                {
                    // костыль, надо придумать получше
                    // проблема: пустой диалог в конце сюжетного узла
                    _isStoryEnds = true;
                }
                else
                {
                    DialogueState = new(_currentCharacterId, sentence, Array.Empty<string>(), _story.currentTags.ToArray());
                }
            }
            else if (choices.Count > 0)
            {
                DialogueState = new(_currentCharacterId, _story.currentText, choices.Select(choice => choice.text).ToArray(), _story.currentTags.ToArray());
            }
            else
            {
                _isStoryEnds = true;
            }
            if(_isStoryEnds)
            {
                DialogueState = new(_currentCharacterId, string.Empty, Array.Empty<string>(), _story.currentTags.ToArray());
                OnStoryFlowEnd();
            }
        }
        private void OnStoryFlowEnd() => DialogueEnded?.Invoke(_currentCharacterId);
        private void OnVariableChanged(string name, object value)
        {
            if (_varListeners.TryGetValue(name, out var action))
            {
                action.Invoke(value);
            }
            else
            {
                _logger?.LogWarning(nameof(DialogueService), $"Variable \"{name}\" change notified, despite of listener not found");
            }
        }
    }
}
