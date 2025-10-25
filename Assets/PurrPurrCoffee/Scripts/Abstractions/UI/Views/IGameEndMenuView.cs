#nullable enable

using System;
using UnityEngine;
using KarenKrill.UI.Views.Abstractions;

namespace PurrPurrCoffee.UI.Views.Abstractions
{
    public interface IGameEndMenuView : IView
    {
        public string TitleText { set; }
        public Color TitleTextColor { set; }

        public event Action? RestartRequested;
        public event Action? MainMenuExitRequested;
        public event Action? ExitRequested;
    }
}