using KarenKrill.StateSystem.Abstractions;
using KarenKrill.Storytelling.Abstractions;
using UnityEngine;

namespace PurrPurrCoffee.GameStates
{
    using Abstractions;
    using Input.Abstractions;
    using KarenKrill.UI.Presenters.Abstractions;
    using PurrPurrCoffee.UI.Views.Abstractions;
    using States;
    using System.Collections;
    using System.Threading.Tasks;
    using UI.Presenters.Abstractions;

    public class GameplayState : PresentableStateHandlerBase<GameState>, IStateHandler<GameState>
    {
        public override GameState State => GameState.Gameplay;

        public GameplayState(ILogger logger,
            IGameFlow gameFlow,
            IInputActionService inputService,
            IDialoguePresenter dialoguePresenter,
            IDialogueProvider dialogueProvider,
            IDialogueService dialogueService,
            IClientController clientController,
            GameSession gameSession,
            IWeatherController weatherController,
            IPresenter<IGameSessionStatusView> gameSessionStatusPresenter) : base(gameSessionStatusPresenter)
        {
            _logger = logger;
            _gameFlow = gameFlow;
            _inputService = inputService;
            _dialoguePresenter = dialoguePresenter;
            _dialogueProvider = dialogueProvider;
            _dialogueService = dialogueService;
            _clientController = clientController;
            _gameSession = gameSession;
            _weatherController = weatherController;
        }
        public override void Enter(GameState prevState)
        {
            base.Enter(prevState);
            if (prevState != GameState.Pause)
            {
                _gameSession.Clear();
            }
            _dialogueProvider.DialogueStarted += OnDialogueStarted;
            _dialogueProvider.DialogueStarting += OnDialogueStarting;
            _dialogueService.ClientServed += OnClientServed;
            _dialogueProvider.DialogueEnded += OnDialogueEnded;
            _dialoguePresenter.NextLineRequested += _dialogueService.NextDialogueLine;
            _dialoguePresenter.SkipRequested += _dialogueService.SkipDialogue;
            _dialoguePresenter.ChoiceMade += _dialogueService.MakeDialogueChoice;

            _inputService.Pause += OnPause;
            _inputService.SetActionMap(ActionMap.Player);
            Cursor.lockState = CursorLockMode.Locked;
            _logger.Log($"{nameof(MainMenuState)}.{nameof(Enter)}()");
            if (prevState != GameState.Pause)
            {
                _dialogueService.StartDialogue(5); // start dialog
            }
        }
        public override void Exit(GameState nextState)
        {
            base.Exit(nextState);

            _dialogueProvider.DialogueStarted -= OnDialogueStarted;
            _dialogueProvider.DialogueStarting -= OnDialogueStarting;
            _dialogueService.ClientServed -= OnClientServed;
            _dialogueProvider.DialogueEnded -= OnDialogueEnded;
            _dialoguePresenter.NextLineRequested -= _dialogueService.NextDialogueLine;
            _dialoguePresenter.SkipRequested -= _dialogueService.SkipDialogue;
            _dialoguePresenter.ChoiceMade -= _dialogueService.MakeDialogueChoice;

            _inputService.Pause -= OnPause;
            _inputService.SetActionMap(ActionMap.UI);
            Cursor.lockState = CursorLockMode.None;
            _logger.Log($"{nameof(MainMenuState)}.{nameof(Exit)}()");
        }

        private readonly ILogger _logger;
        private readonly IGameFlow _gameFlow;

        private readonly IInputActionService _inputService;
        private readonly IDialoguePresenter _dialoguePresenter;
        private readonly IDialogueProvider _dialogueProvider;
        private readonly IDialogueService _dialogueService;
        private readonly IClientController _clientController;
        private readonly IWeatherController _weatherController;
        private readonly GameSession _gameSession;
        private bool _isLastClient = false;

        private void OnPause()
        {
            _gameFlow.PauseLevel();
        }
        private void OnDialogueStarted(int id)
        {
            _inputService.SetActionMap(ActionMap.UI);
            _dialoguePresenter.Enable();
        }
        private void OnDialogueStarting(int id)
        {
            if (id > 0) // костыль: 0 - кофемашина, остальное - персонажи
            {
                if (_gameSession.IsCoffeeInPlayerHands)
                {
                    _dialogueService.SetVariable("coffee_ready"); // своеобразный триггер, в false перейдёт само из ink скрипта
                }
            }
        }
        private void OnDialogueEnded(int id)
        {
            _dialoguePresenter.Disable();
            _inputService.SetActionMap(ActionMap.Player);
            Debug.LogError($"{nameof(OnDialogueEnded)}({id})");
            if (id == 2)
            {
                _weatherController.Type = WeatherType.Rain;
            }
            else if (id == 4) // end dialog
            {
                _gameFlow.EndGame();
            }
            else if (id == 5) // start dialog
            {
                Task.Run(() => SpawnClientAsync());
            }
        }
        private void OnClientServed(int id, int reputation, float revenue)
        {
            Debug.LogError($"ReputationDelta: {reputation}");
            _gameSession.AddReview(reputation);
            _gameSession.AddMoney(revenue);
            _gameSession.IsCoffeeInPlayerHands = false;
            _clientController.ClientReturned += OnClientReturned;
            _clientController.ReturnCurrentClient();
            Debug.LogError($"{nameof(OnClientServed)}({id})");
            if (id == 3) // last client of shift
            {
                _isLastClient = true;
                _weatherController.LightningStrikeEnded += OnLightningStrikeEnded;
                _weatherController.LightningStrike();
            }
        }

        private void OnLightningStrikeEnded()
        {
            _weatherController.LightningStrikeEnded -= OnLightningStrikeEnded;
            _dialogueService.StartDialogue(4); // end dialog
        }

        private void OnClientReturned()
        {
            _clientController.ClientReturned -= OnClientReturned;
            if (!_isLastClient)
            {
                _clientController.SendClient();
            }
        }

        private async Task SpawnClientAsync()
        {
            await Task.Delay(10000);
            _clientController.SendClient();
        }
    }
}