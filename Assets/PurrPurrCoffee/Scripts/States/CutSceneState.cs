using System.Threading.Tasks;
using UnityEngine;

using KarenKrill.StateSystem.Abstractions;
using KarenKrill.Storytelling.Abstractions;

namespace PurrPurrCoffee.GameStates
{
    using Abstractions;
    using Input.Abstractions;
    using UI.Presenters.Abstractions;
    using States;
    using KarenKrill.UI.Presenters.Abstractions;
    using PurrPurrCoffee.UI.Views.Abstractions;

    /*public class CutSceneState : PresentableStateHandlerBase<GameState>, IStateHandler<GameState>
    {
        public override GameState State => GameState.CutScene;

        public CutSceneState(ILogger logger,
            IGameFlow gameFlow,
            IInputActionService inputService,
            IDialoguePresenter dialoguePresenter,
            IDialogueProvider dialogueProvider,
            IDialogueService dialogueService,
            IClientController clientController,
            GameSession gameSession,
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
            Debug.LogError("TrySpan");
            Task.Run(() => SpawnClientAsync());
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
        private readonly GameSession _gameSession;

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
        }
        private void OnClientServed(int id, int reputation, float revenue)
        {
            Debug.LogError($"ReputationDelta: {reputation}");
            _gameSession.AddReview(reputation);
            _gameSession.AddMoney(revenue);
            _gameSession.IsCoffeeInPlayerHands = false;
            _clientController.ClientReturned += OnClientReturned;
            _clientController.ReturnCurrentClient();
            if (id == 3) // last client of shift
            {

            }
        }

        private void OnClientReturned()
        {
            _clientController.ClientReturned -= OnClientReturned;
            _clientController.SendClient();
        }

        private async Task SpawnClientAsync()
        {
            await Task.Delay(5000);
            _clientController.SendClient();
        }
    }*/
}