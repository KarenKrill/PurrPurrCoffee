using System;
using KarenKrill.UniCore.UI.Presenters.Abstractions;
using KarenKrill.UniCore.UI.Views.Abstractions;

namespace PurrPurrCoffee.UI.Presenters
{
    using Abstractions;
    using PurrPurrCoffee.Abstractions;
    using UnityEngine;
    using Views.Abstractions;

    public class GameEndMenuPresenter : PresenterBase<IGameEndMenuView>, IGameEndMenuPresenter, IPresenter<IGameEndMenuView>
    {
#nullable enable
        public event Action? Restart;
        public event Action? MainMenu;
        public event Action? Exit;
#nullable restore

        public GameEndMenuPresenter(IViewFactory viewFactory,
            IPresenterNavigator navigator,
            GameProfileSession gameProfileSession) : base(viewFactory, navigator)
        {
            _gameProfileSession = gameProfileSession;
        }

        protected override void Subscribe()
        {
            View.RestartRequested += OnRestart;
            View.MainMenuExitRequested += OnMainMenuExit;
            View.ExitRequested += OnExit;
            if (_gameProfileSession.IsPlayerWin)
            {
                View.TitleText = "Вы пережили первую смену! Продолжение следует...";
                View.TitleTextColor = new Color(1, (float)0xAC / 0xFF, (float)0x40 / 0xFF, 1);
            }
            else if (_gameProfileSession.IsPlayerLose)
            {
                View.TitleText = "Вы мертвы!";
                View.TitleTextColor = new Color((float)0xE1 / 0xFF, (float)0x24 / 0xFF, 0);
            }
            else
            {
                View.TitleText = "Вы смогли сломать игру, поздравляю :D";
                View.TitleTextColor = Color.white;
            }
        }
        protected override void Unsubscribe()
        {
            View.RestartRequested -= OnRestart;
            View.MainMenuExitRequested -= OnMainMenuExit;
            View.ExitRequested -= OnExit;
        }

        private readonly GameProfileSession _gameProfileSession;
        private void OnRestart() => Restart?.Invoke();
        private void OnMainMenuExit() => MainMenu?.Invoke();
        private void OnExit() => Exit?.Invoke();
    }
}