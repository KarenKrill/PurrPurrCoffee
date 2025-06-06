#nullable enable

using KarenKrill.UI.Views.Abstractions;
using System;
using UnityEngine;

namespace PurrPurrCoffee.UI.Views.Abstractions
{
    public enum DialogueMode
    {
        Line,
        Choices,
        Both
    }
    public interface IDialogueView : IView
    {
        public string ActorName { set; }
        public Sprite ActorIcon { set; }
        public string Title { set; }
        /// <summary>
        /// Dialogue line / phrase / sentence
        /// </summary>
        public string Line { set; }
        public string[] Choices { set; }
        public bool NextLineAvailable { set; }
        public bool SkipAvailable { set; }
        public DialogueMode Mode { set; }

        public event Action<int>? ChoiceMade;
        public event Action? NextLineRequested;
        public event Action? SkipRequested;
    }
}
