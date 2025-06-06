using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

using KarenKrill.StateSystem.Abstractions;
using KarenKrill.UI.Presenters.Abstractions;
using KarenKrill.StateSystem;
using KarenKrill.UI.Presenters;
using KarenKrill.UI.Views;
using KarenKrill.Logging;
using KarenKrill.Diagnostics;
using KarenKrill.Utilities;
using KarenKrill.InteractionSystem;

namespace PurrPurrCoffee
{
    using Abstractions;
    using Input.Abstractions;
    using Input;
    using UnityEngine.UI;
    using Storytelling;

    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameSession>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<ClientController>().FromComponentInHierarchy(false).AsSingle();
            Container.BindInterfacesAndSelfTo<WeatherController>().FromComponentInHierarchy(false).AsSingle();
            InstallSettings();
            Container.Bind<IInputActionService>().To<InputActionService>().FromNew().AsSingle();
#if DEBUG
            Container.Bind<ILogger>().To<Logger>().FromNew().AsSingle().WithArguments(new DebugLogHandler());
#else
            Container.Bind<ILogger>().To<StubLogger>().FromNew().AsSingle();
#endif
            Container.BindInterfacesAndSelfTo<DialogueService>().AsSingle().WithArguments(_storyInkJson.text);
            Container.BindInterfacesAndSelfTo<GameFlow>().AsSingle();
            InstallGameStateMachine();
            InstallViewFactory();
            Container.BindInterfacesAndSelfTo<DiagnosticsProvider>().FromInstance(_diagnosticsProvider).AsSingle();
            InstallPresenterBindings();
            Container.BindInterfacesAndSelfTo<InteractionTargetRegistry>().FromNew().AsSingle();
        }

        [SerializeField]
        Canvas _uiRootCanvas;
        [SerializeField]
        List<GameObject> _uiPrefabs;
        [SerializeField]
        DiagnosticsProvider _diagnosticsProvider;
        [SerializeField]
        TextAsset _storyInkJson;
        private void InstallSettings()
        {
            var qualityLevel = PlayerPrefs.GetInt("Settings.Graphics.QualityLevel", (int)QualityLevel.High);
            if (qualityLevel < 0 || qualityLevel > (int)QualityLevel.High)
            {
                qualityLevel = (int)QualityLevel.High;
            }
            var showFps = PlayerPrefs.GetInt("Settings.Diagnostic.ShowFps", 0);
            GameSettings gameSettings = new((QualityLevel)qualityLevel, showFps != 0);
            Container.Bind<GameSettings>().To<GameSettings>().FromInstance(gameSettings);
        }
        private void InstallGameStateMachine()
        {
            Container.Bind<IStateMachine<GameState>>()
                .To<StateMachine<GameState>>()
                .AsSingle()
                .WithArguments(new GameStateGraph())
                .OnInstantiated((context, instance) =>
                {
                    if (instance is IStateMachine<GameState> stateMachine)
                    {
                        context.Container.Bind<IStateSwitcher<GameState>>().FromInstance(stateMachine.StateSwitcher);
                    }
                })
                .NonLazy();
            var stateTypes = ReflectionUtilities.GetInheritorTypes(typeof(IStateHandler<GameState>), Type.EmptyTypes);
            foreach (var stateType in stateTypes)
            {
                Container.BindInterfacesTo(stateType).AsSingle();
            }
            Container.BindInterfacesTo<ManagedStateMachine<GameState>>().AsSingle().OnInstantiated((context, target) =>
            {
                if (target is ManagedStateMachine<GameState> managedStateMachine)
                {
                    managedStateMachine.Start();
                }
            }).NonLazy();
        }
        private void InstallViewFactory()
        {
            if (_uiRootCanvas == null)
            {
                _uiRootCanvas = FindFirstObjectByType<Canvas>(FindObjectsInactive.Exclude);
                if (_uiRootCanvas == null)
                {
                    var canvasGO = new GameObject(nameof(Canvas));
                    _uiRootCanvas = canvasGO.AddComponent<Canvas>();
                    _uiRootCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    canvasGO.AddComponent<CanvasScaler>();
                    canvasGO.AddComponent<GraphicRaycaster>();
                }
            }
            Container.BindInterfacesAndSelfTo<ViewFactory>().AsSingle().WithArguments(_uiRootCanvas.gameObject, _uiPrefabs);
        }
        private void InstallPresenterBindings()
        {
            Container.BindInterfacesAndSelfTo<PresenterNavigator>().AsTransient();
            var presenterTypes = ReflectionUtilities.GetInheritorTypes(typeof(IPresenter));
            foreach (var presenterType in presenterTypes)
            {
                Container.BindInterfacesTo(presenterType).FromNew().AsSingle();
            }
        }
    }
}