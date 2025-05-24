#nullable enable

using System;

using KarenKrill.UI.Views.Abstractions;

namespace PurrPurrCoffee.UI.Views.Abstractions
{
    public interface ILoseMenuView : IView
    {
        public event Action? Restart;
        public event Action? MainMenuExit;
        public event Action? Exit;
    }
}