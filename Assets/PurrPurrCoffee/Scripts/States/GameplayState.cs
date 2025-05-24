using UnityEngine;

using KarenKrill.StateSystem.Abstractions;

namespace PurrPurrCoffee.GameStates
{
    using Abstractions;
    using States;

    public class GameplayState : PresentableStateHandlerBase<GameState>, IStateHandler<GameState>
    {
        public override GameState State => GameState.Gameplay;

        public GameplayState(ILogger logger,
            IGameFlow gameFlow)
        {
            _logger = logger;
            _gameFlow = gameFlow;
        }
        public override void Enter(GameState prevState)
        {
            base.Enter(prevState);
            _logger.Log($"{nameof(MainMenuState)}.{nameof(Enter)}()");
        }
        public override void Exit(GameState nextState)
        {
            base.Exit(nextState);
            _logger.Log($"{nameof(MainMenuState)}.{nameof(Exit)}()");
        }
        
        private readonly ILogger _logger;
        private readonly IGameFlow _gameFlow;
    }
}