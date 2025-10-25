#nullable enable

using System;

using KarenKrill.UI.Presenters.Abstractions;

namespace PurrPurrCoffee.UI.Presenters.Abstractions
{
    using Views.Abstractions;

    public interface IGameEndMenuPresenter : IPresenter<IGameEndMenuView>
    {
        public event Action? Restart;
        public event Action? MainMenu;
        public event Action? Exit;
    }
}
