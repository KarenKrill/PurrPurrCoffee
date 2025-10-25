using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace PurrPurrCoffee
{
    [CreateAssetMenu(fileName = "EventsManagerConfig", menuName = "Scriptable Objects/EventsManagerConfig")]
    public class EventsManagerConfig : ScriptableObject
    {
        [SerializeField]
        private float _ingameTimeAspect = 600;
        [SerializeField]
        private float _clientsInGameDelayInMinutes = 1;
    }
}
