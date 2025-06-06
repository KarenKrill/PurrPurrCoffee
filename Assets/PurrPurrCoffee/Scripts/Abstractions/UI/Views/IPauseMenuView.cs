#nullable enable

using System;

using KarenKrill.UI.Views.Abstractions;

namespace PurrPurrCoffee.UI.Views.Abstractions
{
    public interface IPauseMenuView : IView
    {
        public event Action? ResumeRequested;
        public event Action? SettingsOpenRequested;
        public event Action? RestartRequested;
        public event Action? MainMenuExitRequested;
        public event Action? ExitRequested;
    }
}