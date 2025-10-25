using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace PurrPurrCoffee
{
    public class CutSceneSignalReceiver : SignalReceiver, INotificationReceiver
    {
        public new void OnNotify(Playable origin, INotification notification, object context)
        {
            _cutSceneController.OnNotify(notification);
        }

        [SerializeField]
        private CutSceneController _cutSceneController;
    }
}

