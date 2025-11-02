#nullable enable

using System.Threading.Tasks;

using UnityEngine;

using KarenKrill.UniCore.Input.Abstractions;
using KarenKrill.UniCore.StateSystem.Abstractions;
using KarenKrill.UniCore.Storytelling.Abstractions;
using KarenKrill.UniCore.UI.Presenters.Abstractions;

namespace PurrPurrCoffee.GameStates
{
    using Abstractions;
    using Input.Abstractions;
    using UI.Views.Abstractions;
    using UI.Presenters.Abstractions;
    using States;
    using Cysharp.Threading.Tasks;
    using PurrPurrCoffee.InventorySystem;
    using System.Linq;

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
            GameProfileSession gameSession,
            IWeatherController weatherController,
            IPresenter<IGameSessionStatusView> gameSessionStatusPresenter,
            IAudioController audioController,
            IPlayerInfoProviderRegistry playerInfoProviderRegistry) : base(gameSessionStatusPresenter)
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
            _audioController = audioController;
            _playerInfoProvider = playerInfoProviderRegistry.Instances.FirstOrDefault();
            playerInfoProviderRegistry.Registred += OnPlayerInfoProviderRegistred;
        }
        public override void Enter(GameState prevState, object? context = null)
        {
            base.Enter(prevState);

            if (context is GameplayStateContext gameplayStateContext && gameplayStateContext.FirstStart)
            {
                _clientNumber = 0;
                _logger.LogError(nameof(GameplayState), $"ClientNumber: {_clientNumber}");
                _gameSession.Clear();
                _weatherController.Type = WeatherType.Rain;
                _SubState = GameplaySubState.Normal;
                _gameFlow.PlayCutscene("WakeUpIntro");
                return;
            }

            if (prevState != GameState.Pause)
            {
                if (_storyProgressIndex == 0)
                {
                    _storyProgressIndex++;
                    UniTask.RunOnThreadPool(async () => await SpawnClientAsync(10000));
                }
                _SubState = GameplaySubState.Normal;
            }
            else
            {
                // restart sub state behaviour (refactor it)
                var prevSubState = _SubState;
                _SubState = GameplaySubState.Normal;
                _SubState = prevSubState;
            }

            _inputService.Pause += OnPause;
            _inputService.Submit += OnSubmit;
            _dialogueProvider.DialogueStarted += OnDialogueStarted;
            _dialogueProvider.DialogueStarting += OnDialogueStarting;
            _dialogueProvider.DialogueEnded += OnDialogueEnded;
            _dialogueService.AddVariableListener("star_raiting", OnStarRaitingChanged);
            _dialogueService.AddVariableListener("paid_amount", OnPaidAmountChanged);
            _inputService.SetActionMap(ActionMap.Player);

            _logger.Log($"{nameof(MainMenuState)}.{nameof(Enter)}()");
        }
        public override void Exit(GameState nextState)
        {
            base.Exit(nextState);

            _dialogueProvider.DialogueStarted -= OnDialogueStarted;
            _dialogueProvider.DialogueStarting -= OnDialogueStarting;
            _dialogueProvider.DialogueEnded -= OnDialogueEnded;
            _inputService.Submit -= OnSubmit;
            _inputService.Pause -= OnPause;
            _dialogueService.RemoveVariableListener("star_raiting");
            _dialogueService.RemoveVariableListener("paid_amount");
            _inputService.SetActionMap(ActionMap.UI);
            _logger.Log($"{nameof(MainMenuState)}.{nameof(Exit)}()");
            if (nextState != GameState.Pause && nextState != GameState.CutScene)
            {
                _SubState = GameplaySubState.None;
            }
        }

        private readonly ILogger _logger;
        private readonly IGameFlow _gameFlow;

        private readonly IInputActionService _inputService;
        private readonly IDialoguePresenter _dialoguePresenter;
        private readonly IDialogueProvider _dialogueProvider;
        private readonly IDialogueService _dialogueService;
        private readonly IClientController _clientController;
        private readonly IWeatherController _weatherController;
        private readonly IAudioController _audioController;
        private IPlayerInfoProvider? _playerInfoProvider;

        private readonly GameProfileSession _gameSession;
        private bool _isLastClient = false;
        private int _storyProgressIndex = 0;

        private GameplaySubState _SubState
        {
            get => _subState;
            set
            {
                if (_subState != value)
                {
                    var prevSubState = _subState;
                    _subState = value;
                    OnSubStateExit(prevSubState, _subState);
                    OnSubStateEnter(prevSubState, _subState);
                }
            }
        }
        private GameplaySubState _subState = GameplaySubState.None;

        private void OnPause()
        {
            _gameFlow.PauseLevel();
        }
        private void OnSubmit()
        {
            if (!_dialogueService.IsEnds)
            {
                _dialogueService.NextDialogueLine();
            }
        }
        private void OnDialogueStarting(string id)
        {
            if (_gameSession.PlayerInventory.Items.Any(item => item is CoffeeCupItem coffeeCupItem && coffeeCupItem.Fullness > 0))
            {
                _dialogueService.SetVariable("coffee_ready", true); // своеобразный триггер, в false перейдёт само из ink скрипта
            }
        }
        private void OnDialogueStarted(string id)
        {
            _inputService.SetActionMap(ActionMap.UI);
            _dialoguePresenter.ShowInteractionTooltip = true;
            _dialoguePresenter.Enable();
        }
        private void OnDialogueEnded(string id)
        {
            _dialoguePresenter.Disable();
            if (_SubState != GameplaySubState.Fear)
            {
                _inputService.SetActionMap(ActionMap.Player);
            }
            Debug.LogError($"{nameof(OnDialogueEnded)}({id})");
            if (id == "player_end_shift_1") // end dialog
            {
                _gameFlow.EndGame();
            }
        }
        private void OnStarRaitingChanged(object value)
        {
            _logger.LogError(nameof(GameplayState), $"StarRaiting value changed {value}");
            if (value is int starRaiting)
            {
                _gameSession.AddReview(starRaiting);
            }
            else
            {
                _logger?.LogError(nameof(GameplayState), $"StarRaiting value isn't Integer");
            }
        }
        private void OnPaidAmountChanged(object value)
        {
            _logger?.LogError(nameof(GameplayState), $"PaidAmount value change {value}");
            if (value is int paidAmount)
            {
                _gameSession.AddMoney(paidAmount);
                foreach (var item in _gameSession.PlayerInventory.Items)
                {
                    if (item is CoffeeCupItem coffeeCupItem)
                    {
                        coffeeCupItem.Fullness = 0;
                        coffeeCupItem.IsCoveredWithLid = false;
                        coffeeCupItem.IsContainsStraw = false;
                        coffeeCupItem.Base = CoffeeBase.Water;
                        coffeeCupItem.Syrup = CoffeeSyrup.None;
                        if (coffeeCupItem.GameObject != null)
                        {
                            coffeeCupItem.GameObject.SetActive(false);
                        }
                        _gameSession.PlayerInventory.RemoveItem(coffeeCupItem);
                        break;
                    }
                }
                _clientController.ClientReturned += OnClientReturned;
                _clientController.ReturnCurrentClient();
                Debug.LogError($"{nameof(OnPaidAmountChanged)}({_clientNumber})");
                if (_clientNumber == 3) // last client of shift
                {
                    _isLastClient = true;
                    _SubState = GameplaySubState.Fear;
                    _weatherController.LightningStrikeEnded += OnLightningStrikeEnded;
                    _weatherController.LightningStrike();
                }
            }
            else
            {
                _logger?.LogError(nameof(GameplayState), $"PaidAmount value isn't Integer");
            }
        }
        private void OnSubStateEnter(GameplaySubState previousState, GameplaySubState nextState)
        {
            _logger.LogWarning(nameof(GameplayState), $"Substate changed {previousState} -> {nextState}");
            switch (nextState)
            {
                case GameplaySubState.Normal:
                    _audioController.PlayBackgroundTheme(BackgroundTheme.Relax);
                    break;
                case GameplaySubState.Fear:
                    _audioController.PlayBackgroundTheme(BackgroundTheme.Fear);
                    UniTask.RunOnThreadPool(async () => await SpawnEnemyAsync(0));
                    _inputService.SetActionMap(ActionMap.UI); // lock camera
                    break;
                case GameplaySubState.Chase:
                    if (_playerInfoProvider is not null)
                    {
                        _inputService.SetActionMap(ActionMap.Player); // unlock camera
                        _audioController.PlayBackgroundTheme(BackgroundTheme.Chasing);
                        _clientController.ChaseFinished += OnChaseFinished;
                        UniTask.RunOnThreadPool(async () =>
                        {
                            await Task.Delay(2000);
                            await UniTask.SwitchToMainThread();
                            _clientController.StartChase(_playerInfoProvider.PlayerTransform);
                        });
                    }
                    else
                    {
                        _logger.LogError(nameof(GameplayState), $"Can't start chasing while {nameof(_playerInfoProvider)} is null");
                    }
                    break;
                case GameplaySubState.Death:
                    _audioController.PlayBackgroundTheme(BackgroundTheme.Death);
                    _gameSession.IsPlayerWin = false;
                    _gameSession.IsPlayerLose = true;
                    _gameFlow.EndGame();
                    break;
                case GameplaySubState.Subtitles:
                    _audioController.PlayBackgroundTheme(BackgroundTheme.Subtitles);
                    break;
                case GameplaySubState.None:
                default:
                    _audioController.StopBackgroundTheme();
                    break;
            }
        }
        private void OnSubStateExit(GameplaySubState previousState, GameplaySubState nextState)
        {
        }
        private void OnPlayerInfoProviderRegistred(IPlayerInfoProvider playerInfoProvider)
        {
            _playerInfoProvider = playerInfoProvider;
        }
        private void OnLightningStrikeEnded()
        {
            _weatherController.LightningStrikeEnded -= OnLightningStrikeEnded;
            //_dialogueService.StartDialogue("player_end_shift_1"); // end dialog
        }

        private void OnClientReturned()
        {
            _clientController.ClientReturned -= OnClientReturned;
            if (!_isLastClient)
            {
                _ = UniTask.RunOnThreadPool(async () => await SpawnClientAsync(0));
            }
        }

        private int _clientNumber;
        private async UniTask SpawnClientAsync(int msDelay)
        {
            if (msDelay > 0)
            {
                await UniTask.Delay(msDelay);
            }
            _clientNumber++;
            await UniTask.SwitchToMainThread();
            _logger.LogError(nameof(GameplayState), $"ClientNumber: {_clientNumber}");
            _clientController.SendClient();
        }
        private async UniTask SpawnEnemyAsync(int msDelay)
        {
            if (msDelay > 0)
            {
                await UniTask.Delay(msDelay);
            }
            await UniTask.SwitchToMainThread();
            _logger.LogError(nameof(GameplayState), $"ClientNumber: {_clientNumber}");
            _clientController.SendEnemy();
            await UniTask.Delay(6000);
            _SubState = GameplaySubState.Chase;
        }

        private void OnChaseFinished(GameObject enemy, bool isChaseSuccessful)
        {
            _clientController.ChaseFinished -= OnChaseFinished;
            if (isChaseSuccessful)
            {
                _inputService.SetActionMap(ActionMap.UI); // lock movement
                var enemyAnimator = enemy.GetComponentInChildren<Animator>();
                if (enemyAnimator != null)
                {
                    enemyAnimator.SetTrigger("Attack");
                }
                _audioController.StopBackgroundTheme();
                UniTask.RunOnThreadPool(async () =>
                {
                    await UniTask.Delay(1000); // wait for attack animation
                    await UniTask.SwitchToMainThread();
                    _audioController.PlayEffect(SoundEffect.BloodHit);
                    await UniTask.Delay(1000); // wait for attack animation
                    await UniTask.SwitchToMainThread();
                    _SubState = GameplaySubState.Death;
                });
            }
            else
            {
                _logger.LogWarning(nameof(GameplayState), "You are escape?");
                UniTask.RunOnThreadPool(async () =>
                {
                    await UniTask.SwitchToMainThread();
                    _weatherController.LightningStrike();
                    await UniTask.Delay(500);
                    await UniTask.SwitchToMainThread();
                    enemy.SetActive(false);
                    await UniTask.Delay(500);
                    await UniTask.SwitchToMainThread();
                    _SubState = GameplaySubState.Normal;
                    await UniTask.Delay(10000);
                    await UniTask.SwitchToMainThread();
                    _gameSession.IsPlayerWin = true;
                    _gameSession.IsPlayerLose = false;
                    _gameFlow.EndGame();
                });
            }
        }
    }
    public class GameplayStateContext
    {
        public bool FirstStart;
        public GameplayStateContext(bool firstStart)
        {
            FirstStart = firstStart;
        }
    }
    public enum GameplaySubState
    {
        None,
        Normal,
        Fear,
        Chase,
        Death,
        Subtitles
    }
}