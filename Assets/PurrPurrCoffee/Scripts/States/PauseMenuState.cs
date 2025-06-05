using UnityEngine;

using KarenKrill.StateSystem.Abstractions;

namespace PurrPurrCoffee.GameStates
{
    using Abstractions;
    using States;
    using Input.Abstractions;
    using UI.Presenters.Abstractions;

    public class PauseMenuState : PresentableStateHandlerBase<GameState>, IStateHandler<GameState>
    {
        public override GameState State => GameState.Pause;

        public PauseMenuState(ILogger logger,
            IGameFlow gameFlow,
            IInputActionService inputActionService,
            IPauseMenuPresenter pauseMenuPresenter)
            : base(pauseMenuPresenter)
        {
            _logger = logger;
            _gameFlow = gameFlow;
            _inputActionService = inputActionService;
            _pauseMenuPresenter = pauseMenuPresenter;

        }
        public override void Enter(GameState prevState)
        {
            base.Enter(prevState);
            _logger.Log($"{GetType().Name}.{nameof(Enter)}()");
            _inputActionService.Cancel += OnResume;
            _pauseMenuPresenter.Resume += OnResume;
            _inputActionService.SetActionMap(ActionMap.UI);
        }
        public override void Exit(GameState nextState)
        {
            _logger.Log($"{GetType().Name}.{nameof(Exit)}()");
            _inputActionService.Cancel -= OnResume;
            _pauseMenuPresenter.Resume -= OnResume;
            base.Exit(nextState);
        }

        private readonly ILogger _logger;
        private readonly IGameFlow _gameFlow;
        private readonly IInputActionService _inputActionService;
        private readonly IPauseMenuPresenter _pauseMenuPresenter;

        private void OnResume()
        {
            _gameFlow.PlayLevel();
        }
    }
}
