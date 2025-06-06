using UnityEngine;

using KarenKrill.StateSystem.Abstractions;

namespace PurrPurrCoffee.GameStates
{
    using Abstractions;
    using PurrPurrCoffee.Input.Abstractions;
    using States;
    using UI.Presenters.Abstractions;

    public class SceneLoadState : PresentableStateHandlerBase<GameState>, IStateHandler<GameState>
    {
        public override GameState State => GameState.SceneLoad;

        public SceneLoadState(ILogger logger,
            IGameFlow gameFlow,
            IMainMenuPresenter mainMenuPresenter) : base(mainMenuPresenter)
        {
            _logger = logger;
            _gameFlow = gameFlow;
            _mainMenuPresenter = mainMenuPresenter;
        }
        public override void Enter(GameState prevState)
        {
            _mainMenuPresenter.NewGame += OnNewGame;
            _mainMenuPresenter.Exit += OnExit;
            base.Enter(prevState);
            _logger.Log($"{nameof(MainMenuState)}.{nameof(Enter)}()");
        }

        private void OnExit()
        {
            _gameFlow.Exit();
        }

        private void OnNewGame()
        {
            _gameFlow.StartGame();
        }

        public override void Exit(GameState nextState)
        {
            base.Exit(nextState);
            _logger.Log($"{nameof(MainMenuState)}.{nameof(Exit)}()");
        }
        
        private readonly ILogger _logger;
        private readonly IGameFlow _gameFlow;
        private readonly IMainMenuPresenter _mainMenuPresenter;
    }
}