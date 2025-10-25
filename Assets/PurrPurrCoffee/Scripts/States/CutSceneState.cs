#nullable enable

using KarenKrill.StateSystem.Abstractions;
using UnityEngine;

namespace PurrPurrCoffee.GameStates
{
    using Abstractions;
    using Cysharp.Threading.Tasks;
    using Input.Abstractions;
    using KarenKrill.Input.Abstractions;
    using KarenKrill.Storytelling.Abstractions;
    using KarenKrill.UI.Presenters.Abstractions;
    using PurrPurrCoffee.UI.Presenters.Abstractions;
    using PurrPurrCoffee.UI.Views.Abstractions;
    using States;
    using System.Linq;
    using UnityEngine.Playables;
    using UnityEngine.Timeline;

    public class CutSceneState : PresentableStateHandlerBase<GameState>, IStateHandler<GameState>
    {
        public override GameState State => GameState.CutScene;

        public CutSceneState(ILogger logger,
            IGameFlow gameFlow,
            IStateSwitcher<GameState> stateSwitcher,
            IInputActionService inputService,
            IPresenter<IGameSessionStatusView> gameSessionStatusPresenter,
            IDialoguePresenter dialoguePresenter,
            IDialogueProvider dialogueProvider,
            IDialogueService dialogueService,
            ICutSceneInfoProvider cutSceneInfoProvider,
            ICutSceneControllerRegistry cutSceneControllerRegistry)
            : base(gameSessionStatusPresenter)
        {
            _logger = logger;
            _gameFlow = gameFlow;
            _stateSwitcher = stateSwitcher;
            _inputService = inputService;
            _dialoguePresenter = dialoguePresenter;
            _dialogueProvider = dialogueProvider;
            _dialogueService = dialogueService;
            _cutSceneInfoProvider = cutSceneInfoProvider;
            _cutSceneControllerRegistry = cutSceneControllerRegistry;
        }
        public override void Enter(GameState prevState, object? context)
        {
            base.Enter(prevState);
            _inputService.Cancel += OnPause;
            _inputService.SetActionMap(ActionMap.UI);
            _dialogueProvider.DialogueStarted += OnDialogueStarted;
            _dialogueProvider.DialogueEnded += OnDialogueEnded;
            _cutSceneControllerRegistry.ControllerRegistered += OnCutSceneControllerRegistered;
            _cutSceneController = _cutSceneControllerRegistry.Controllers.FirstOrDefault();
            if (prevState != GameState.Pause)
            {
                _previousGameState = prevState;
                _isDialogueStarted = false;
                if (context is CutSceneStateContext ctx)
                {
                    if (_cutSceneInfoProvider.CutScenesInfo.TryGetValue(ctx.Id, out _currentCutSceneInfo))
                    {
                        UniTask.RunOnThreadPool(async () =>
                        {
                            while (_cutSceneController is null)
                            {
                                await UniTask.Yield();
                            }
                            _isFirstDialogueAction = true;
                            _cutSceneController.EventSignaled += OnCutSceneEventSignaled;
                            _cutSceneController.CutSceneFinished += OnCutSceneFinished;
                            await UniTask.SwitchToMainThread();
                            _cutSceneController.PlayCutScene(_currentCutSceneInfo.PlayableAsset);
                        });
                    }
                    else
                    {
                        _logger.LogWarning(nameof(CutSceneState), $"CutScene \"{ctx.Id}\" not found");
                    }
                }
                else
                {
                    _logger.LogWarning(nameof(CutSceneState), "Context not specified or type mismatch");
                }
            }
        }
        public override void Exit(GameState nextState)
        {
            base.Exit(nextState);

            _inputService.Cancel -= OnPause;
            _dialogueProvider.DialogueStarted -= OnDialogueStarted;
            _dialogueProvider.DialogueEnded -= OnDialogueEnded;
            _cutSceneControllerRegistry.ControllerRegistered -= OnCutSceneControllerRegistered;
            _cutSceneController = null;
            _logger.Log($"{nameof(CutSceneState)}.{nameof(Exit)}()");
        }

        private readonly ILogger _logger;
        private readonly IGameFlow _gameFlow;
        private readonly IStateSwitcher<GameState> _stateSwitcher;
        private readonly IInputActionService _inputService;
        private readonly IDialoguePresenter _dialoguePresenter;
        private readonly IDialogueProvider _dialogueProvider;
        private readonly IDialogueService _dialogueService;
        private readonly ICutSceneInfoProvider _cutSceneInfoProvider;
        private readonly ICutSceneControllerRegistry _cutSceneControllerRegistry;
        private ICutSceneController? _cutSceneController;
        private CutSceneInfo? _currentCutSceneInfo;
        private bool _isFirstDialogueAction;
        private GameState _previousGameState;
        private bool _isDialogueStarted = false;

        private void OnPause()
        {
            _gameFlow.PauseLevel();
        }
        private void OnDialogueStarted(string id)
        {
            _dialoguePresenter.ShowInteractionTooltip = false;
            _dialoguePresenter.Enable();
        }
        private void OnDialogueEnded(string id)
        {
            _dialoguePresenter.Disable();
            Debug.LogError($"{nameof(OnDialogueEnded)}({id})");
        }
        private void OnCutSceneControllerRegistered(ICutSceneController cutSceneController)
        {
            _cutSceneController = cutSceneController;
        }
        private void OnCutSceneFinished(PlayableAsset playableAsset)
        {
            UniTask.SwitchToMainThread();
            if (_cutSceneController != null)
            {
                _cutSceneController.EventSignaled -= OnCutSceneEventSignaled;
                _cutSceneController.CutSceneFinished -= OnCutSceneFinished;
            }
            else
            {
                _logger.LogWarning(nameof(CutSceneState), $"CutSceneController is null since cut scene {playableAsset.name} is finished");
            }
            if (_isDialogueStarted)
            {
                _dialogueService.NextDialogueLine();
            }
            _stateSwitcher.TransitTo(_previousGameState);
        }
        private void OnCutSceneEventSignaled((PlayableAsset sender, SignalAsset signal) args)
        {
            UniTask.SwitchToMainThread();
            if (_currentCutSceneInfo is not null)
            {
                var actionInfo = _currentCutSceneInfo.ActionsInfo.FirstOrDefault(actionInfo => actionInfo.SignalTrigger == args.signal);
                if (actionInfo?.IsDialogueAction ?? false)
                {
                    if (_isFirstDialogueAction)
                    {
                        _isFirstDialogueAction = false;
                        _dialogueService.StartDialogue(_currentCutSceneInfo.DialogueId);
                        _isDialogueStarted = true;
                    }
                    else
                    {
                        _dialogueService.NextDialogueLine();
                    }
                }
            }
            else
            {
                _logger.LogWarning(nameof(CutSceneState), $"CurrentCutSceneInfo is null since event {args.signal} is signaled");
            }
        }
    }
    public class CutSceneStateContext
    {
        public string Id { get; }

        public CutSceneStateContext(string id)
        {
            Id = id;
        }
    }
}