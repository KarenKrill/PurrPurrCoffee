#nullable enable

using System;

using KarenKrill.UI.Views.Abstractions;

namespace PurrPurrCoffee.UI.Views.Abstractions
{
    public interface IMainMenuView : IView
    {
        public event Action? NewGameRequested;
        public event Action? ExitRequested;
        public event Action? SettingsOpenRequested;
    }
}