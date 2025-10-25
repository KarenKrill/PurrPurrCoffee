using System.Collections.Generic;

using UnityEngine;

using AYellowpaper.SerializedCollections;

namespace PurrPurrCoffee
{
    using Abstractions;

    [CreateAssetMenu(fileName = nameof(CutSceneInfoRepository),
        menuName = "Scriptable Objects/" + nameof(CutSceneInfoRepository))]
    public class CutSceneInfoRepository : ScriptableObject, ICutSceneInfoProvider
    {
        public IReadOnlyDictionary<string, CutSceneInfo> CutScenesInfo => _cutScenesInfo;

        [SerializeField, SerializedDictionary(keyName: "Id", valueName: "CutSceneInfo")]
        private SerializedDictionary<string, CutSceneInfo> _cutScenesInfo = new();
    }
}
