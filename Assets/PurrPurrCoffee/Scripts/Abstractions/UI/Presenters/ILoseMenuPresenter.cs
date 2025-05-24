#nullable enable

using System;

using KarenKrill.UI.Presenters.Abstractions;

namespace PurrPurrCoffee.UI.Presenters.Abstractions
{
    using Views.Abstractions;

    public interface ILoseMenuPresenter : IPresenter<ILoseMenuView>
    {
        public event Action? Restart;
        public event Action? MainMenu;
        public event Action? Exit;
    }
}
