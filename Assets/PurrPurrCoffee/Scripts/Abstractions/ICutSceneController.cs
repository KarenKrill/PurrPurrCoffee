using System;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace PurrPurrCoffee
{
    public interface ICutSceneControllerRegistry
    {
        public IReadOnlyList<ICutSceneController> Controllers { get; }
        public event Action<ICutSceneController> ControllerRegistered;

        public void Register(ICutSceneController cutSceneController);
        public void Unregister(ICutSceneController cutSceneController);
    }
    public interface ICutSceneController
    {
        event Action<(PlayableAsset sender, SignalAsset signal)> EventSignaled;
        event Action<PlayableAsset> CutSceneFinished;

        void PlayCutScene(PlayableAsset playableAsset);
        void PauseCutScene();
        void ResumeCutScene();
    }
}
