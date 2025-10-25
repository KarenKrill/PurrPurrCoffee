using UnityEngine;

namespace PurrPurrCoffee
{
    public class WaypathProvider : MonoBehaviour
    {
        public Waypath Waypath => _waypath;
        public Waypath ShortWaypath => _shortWaypath;
        public Waypath BackWaypath => _backWaypath;

        [SerializeField]
        private Waypath _waypath;
        [SerializeField]
        private Waypath _shortWaypath;
        [SerializeField]
        private Waypath _backWaypath;
    }
}