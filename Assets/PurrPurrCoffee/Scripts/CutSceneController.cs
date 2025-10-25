using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

using Zenject;

namespace PurrPurrCoffee
{
    public class CutSceneControllerRegistry : ICutSceneControllerRegistry
    {
        public IReadOnlyList<ICutSceneController> Controllers => _registredControllers;
        public event Action<ICutSceneController> ControllerRegistered;

        public void Register(ICutSceneController cutSceneController)
        {
            _registredControllers.Add(cutSceneController);
            ControllerRegistered?.Invoke(cutSceneController);
        }
        public void Unregister(ICutSceneController cutSceneController)
        {
            _registredControllers.Remove(cutSceneController);
        }

        private readonly List<ICutSceneController> _registredControllers = new();
    }
    [RequireComponent(typeof(PlayableDirector))]
    public class CutSceneController : MonoBehaviour, ICutSceneController
    {
        public event Action<(PlayableAsset sender, SignalAsset signal)> EventSignaled;
        public event Action<PlayableAsset> CutSceneFinished;

        [Inject]
        public void Initialize(ICutSceneControllerRegistry cutSceneControllerRegistry)
        {
            _cutSceneControllerRegistry = cutSceneControllerRegistry;
        }

        public void PlayCutScene(PlayableAsset playableAsset)
        {
            _playableDirector.stopped -= OnTimelineStopped;
            _playableDirector.Stop();
            _playableDirector.playableAsset = playableAsset;
            _playableDirector.stopped += OnTimelineStopped;
            _playableDirector.Play();
        }
        public void PauseCutScene()
        {
            _playableDirector.Pause();
        }
        public void ResumeCutScene()
        {
            _playableDirector.Pause();
        }
        public void OnNotify(INotification notification)
        {
            if (notification is SignalEmitter signalEmitter && signalEmitter.asset != null)
            {
                EventSignaled?.Invoke((_playableDirector.playableAsset, signalEmitter.asset));
            }
        }

        private PlayableDirector _playableDirector;
        private ICutSceneControllerRegistry _cutSceneControllerRegistry;

        private void Awake()
        {
            _playableDirector = GetComponent<PlayableDirector>();
        }
        private void OnEnable()
        {
            _cutSceneControllerRegistry.Register(this);
        }
        private void OnDisable()
        {
            _cutSceneControllerRegistry.Unregister(this);
        }

        private void OnTimelineStopped(PlayableDirector playableDirector)
        {
            CutSceneFinished?.Invoke(playableDirector.playableAsset);
        }
    }
}
