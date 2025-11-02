#nullable enable

using UnityEngine;

using KarenKrill.UniCore.Input.Abstractions;
using KarenKrill.UniCore.StateSystem.Abstractions;

namespace PurrPurrCoffee.GameStates
{
    using Abstractions;
    using Input.Abstractions;
    using States;
    using UI.Presenters.Abstractions;

    public class MainMenuState : PresentableStateHandlerBase<GameState>, IStateHandler<GameState>
    {
        public override GameState State => GameState.MainMenu;

        public MainMenuState(ILogger logger,
            IGameFlow gameFlow,
            IInputActionService inputService,
            IMainMenuPresenter mainMenuPresenter) : base(mainMenuPresenter)
        {
            _logger = logger;
            _gameFlow = gameFlow;
            _mainMenuPresenter = mainMenuPresenter;
            _inputService = inputService;
        }
        public override void Enter(GameState prevState, object? context = null)
        {
            _mainMenuPresenter.NewGame += OnNewGame;
            _mainMenuPresenter.Exit += OnExit;
            base.Enter(prevState);
            _inputService.SetActionMap(ActionMap.UI);
            _logger.Log($"{nameof(MainMenuState)}.{nameof(Enter)}()");
        }
        public override void Exit(GameState nextState)
        {
            base.Exit(nextState);
            _mainMenuPresenter.NewGame -= OnNewGame;
            _mainMenuPresenter.Exit -= OnExit;
            _logger.Log($"{nameof(MainMenuState)}.{nameof(Exit)}()");
        }
        
        private readonly ILogger _logger;
        private readonly IGameFlow _gameFlow;
        private readonly IInputActionService _inputService;
        private readonly IMainMenuPresenter _mainMenuPresenter;

        private void OnExit()
        {
            _gameFlow.Exit();
        }
        private void OnNewGame()
        {
            _gameFlow.StartGame();
        }
    }
}