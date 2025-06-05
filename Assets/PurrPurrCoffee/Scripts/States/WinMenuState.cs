using UnityEngine;
using KarenKrill.StateSystem.Abstractions;

namespace PurrPurrCoffee.GameStates
{
    using PurrPurrCoffee.Abstractions;
    using PurrPurrCoffee.Input.Abstractions;
    using PurrPurrCoffee.States;
    using PurrPurrCoffee.UI.Presenters.Abstractions;

    public class WinMenuState : PresentableStateHandlerBase<GameState>, IStateHandler<GameState>
    {
        public override GameState State => GameState.GameEnd;

        public WinMenuState(ILogger logger,
            IGameFlow gameFlow,
            IInputActionService inputService,
            IWinMenuPresenter winMenuPresenter) : base(winMenuPresenter)
        {
            _logger = logger;
            _gameFlow = gameFlow;
            _winMenuPresenter = winMenuPresenter;
            _inputService = inputService;
        }
        public override void Enter(GameState prevState)
        {
            _winMenuPresenter.Exit += OnExit;
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
        private readonly IWinMenuPresenter _winMenuPresenter;

        private void OnExit()
        {
            _gameFlow.Exit();
        }
    }
}
