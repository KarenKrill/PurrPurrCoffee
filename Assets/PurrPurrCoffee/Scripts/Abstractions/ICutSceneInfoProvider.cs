using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace PurrPurrCoffee.Abstractions
{
    public interface ICutSceneInfoProvider
    {
        IReadOnlyDictionary<string, CutSceneInfo> CutScenesInfo { get; }
    }

    [Serializable]
    public class CutSceneInfo
    {
        [field: SerializeField]
        public PlayableAsset PlayableAsset { get; private set; }
        [field: SerializeField]
        public AudioClip BackgroundMusic { get; private set; }
        [field: SerializeField]
        public string DialogueId { get; private set; }
        [field: SerializeField]
        public List<CutSceneActionInfo> ActionsInfo { get; private set; }

        public CutSceneInfo(PlayableAsset playableAsset,
            AudioClip backgroundMusic,
            string dialogueId,
            List<CutSceneActionInfo> actionsInfo)
        {
            PlayableAsset = playableAsset;
            BackgroundMusic = backgroundMusic;
            DialogueId = dialogueId;
            ActionsInfo = actionsInfo;
        }
    }

    [Serializable]
    public class CutSceneActionInfo
    {
        [field: SerializeField]
        public SignalAsset SignalTrigger { get; private set; }
        [field: SerializeField]
        public bool IsDialogueAction { get; private set; } = true;
        [field: SerializeField]
        public AudioClip VoiceTrack { get; private set; }
        [field: SerializeField]
        public AudioClip BackgroundMusicOverride { get; private set; }

        public CutSceneActionInfo(SignalAsset signalTrigger,
            bool isDialogueAction,
            AudioClip voiceTrack,
            AudioClip backgroundMusicOverride)
        {
            SignalTrigger = signalTrigger;
            IsDialogueAction = isDialogueAction;
            VoiceTrack = voiceTrack;
            BackgroundMusicOverride = backgroundMusicOverride;
        }
    }
}
