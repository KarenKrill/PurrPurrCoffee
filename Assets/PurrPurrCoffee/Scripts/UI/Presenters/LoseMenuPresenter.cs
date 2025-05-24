using System;

using KarenKrill.UI.Presenters.Abstractions;
using KarenKrill.UI.Views.Abstractions;

namespace PurrPurrCoffee.UI.Presenters
{
    using Abstractions;
    using Views.Abstractions;

    public class LoseMenuPresenter : PresenterBase<ILoseMenuView>, ILoseMenuPresenter, IPresenter<ILoseMenuView>
    {
#nullable enable
        public event Action? Restart;
        public event Action? MainMenu;
        public event Action? Exit;
#nullable restore

        public LoseMenuPresenter(IViewFactory viewFactory,
            IPresenterNavigator navigator) : base(viewFactory, navigator)
        {
        }

        protected override void Subscribe()
        {
            View.Restart += OnRestart;
            View.MainMenuExit += OnMainMenuExit;
            View.Exit += OnExit;
        }
        protected override void Unsubscribe()
        {
            View.Restart -= OnRestart;
            View.MainMenuExit -= OnMainMenuExit;
            View.Exit -= OnExit;
        }

        private void OnRestart() => Restart?.Invoke();
        private void OnMainMenuExit() => MainMenu?.Invoke();
        private void OnExit() => Exit?.Invoke();
    }
}