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

    public class GameEndMenuState : PresentableStateHandlerBase<GameState>, IStateHandler<GameState>
    {
        public override GameState State => GameState.GameEnd;

        public GameEndMenuState(ILogger logger,
            IGameFlow gameFlow,
            IInputActionService inputService,
            IGameEndMenuPresenter gameEndMenuPresenter) : base(gameEndMenuPresenter)
        {
            _logger = logger;
            _gameFlow = gameFlow;
            _gameEndMenuPresenter = gameEndMenuPresenter;
            _inputService = inputService;
        }
        public override void Enter(GameState prevState, object? context = null)
        {
            _gameEndMenuPresenter.Exit += OnExit;
            base.Enter(prevState);
            _inputService.SetActionMap(ActionMap.UI);
            _logger.Log($"{nameof(MainMenuState)}.{nameof(Enter)}()");
        }
        public override void Exit(GameState nextState)
        {
            base.Exit(nextState);
            _logger.Log($"{nameof(MainMenuState)}.{nameof(Exit)}()");
        }

        private readonly ILogger _logger;
        private readonly IGameFlow _gameFlow;
        private readonly IInputActionService _inputService;
        private readonly IGameEndMenuPresenter _gameEndMenuPresenter;

        private void OnExit()
        {
            _gameFlow.Exit();
        }
    }
}
