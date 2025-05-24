#nullable enable

using System;

using KarenKrill.UI.Presenters.Abstractions;

namespace PurrPurrCoffee.UI.Presenters.Abstractions
{
    using Views.Abstractions;

    public interface IMainMenuPresenter : IPresenter<IMainMenuView>
    {
        public event Action? NewGame;
        public event Action? Exit;
    }
}
