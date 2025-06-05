using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using KarenKrill.UI.Views;
using KarenKrill.Instantiattion;

namespace PurrPurrCoffee.UI.Views
{
    using Abstractions;

    public class DialogueView : ViewBehaviour, IDialogueView
    {
        public string ActorName { set => _actorNameText.text = value; }
        public Sprite ActorIcon { set => _actorIcon.sprite = value; }
        public string Title { set => _titleText.text = value; }
        public string Line { set { _lineText.text = value; } }
        public string[] Choices { set => UpdateChoiceButtons(value); }
        public bool NextLineAvailable { set => _nextLineButton.enabled = value; }
        public bool SkipAvailable { set => _skipButton.enabled = value; }
        public DialogueMode Mode { set => SetMode(value); }

#nullable enable
        public event Action<int>? ChoiceMade;
        public event Action? NextLineRequested;
        public event Action? SkipRequested;
#nullable restore

        [SerializeField]
        private TextMeshProUGUI _actorNameText;
        [SerializeField]
        private Image _actorIcon;
        [SerializeField]
        private TextMeshProUGUI _titleText;
        [SerializeField]
        private TextMeshProUGUI _lineText;
        [SerializeField]
        private Button _skipButton;
        [SerializeField]
        private Button _nextLineButton;
        [SerializeField]
        private Button _dialogueChoiceButtonPrefab;
        [SerializeField]
        private int _defaultChoicesCapacity = 3;
        [SerializeField]
        private GameObject _lineContainer;
        [SerializeField]
        private GameObject _choicesContainer;
        [SerializeField]
        private Transform _choiceButtonsParent;

        private ComponentPool<Button> _buttonsPool;
        private readonly List<Button> _currentChoiceButtons = new();

        private void Awake()
        {
            _buttonsPool = new(_dialogueChoiceButtonPrefab, _choiceButtonsParent, _defaultChoicesCapacity);
        }
        private void OnDestroy()
        {
            _buttonsPool.Dispose();
        }
        private void OnEnable()
        {
            _skipButton.onClick.AddListener(OnSkipButtonClicked);
            _nextLineButton.onClick.AddListener(OnNextLineButtonClicked);
        }
        private void OnDisable()
        {
            _skipButton.onClick.RemoveListener(OnSkipButtonClicked);
            _nextLineButton.onClick.RemoveListener(OnNextLineButtonClicked);
        }

        private void OnSkipButtonClicked() => SkipRequested?.Invoke();
        private void OnNextLineButtonClicked() => NextLineRequested?.Invoke();
        private void UpdateChoiceButtons(string[] choices)
        {
            foreach (var button in _currentChoiceButtons)
            {
                _buttonsPool.Release(button);
            }
            _currentChoiceButtons.Clear();

            for (int i = 0; i < choices?.Length; i++)
            {
                var button = _buttonsPool.Get();
                button.GetComponentInChildren<TextMeshProUGUI>().text = choices[i];
                button.onClick.RemoveAllListeners();
                var index = i;
                button.onClick.AddListener(() => ChoiceMade?.Invoke(index));
                _currentChoiceButtons.Add(button);
            }
        }
        private void SetMode(DialogueMode mode)
        {
            switch (mode)
            {
                case DialogueMode.Line:
                    _lineContainer.SetActive(true);
                    _choicesContainer.SetActive(false);
                    break;
                case DialogueMode.Choices:
                    _lineContainer.SetActive(false);
                    _choicesContainer.SetActive(true);
                    break;
                default:
                    _lineContainer.SetActive(true);
                    _choicesContainer.SetActive(true);
                    break;
            }
        }
    }
}