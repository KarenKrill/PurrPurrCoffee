using System;

using UnityEngine;

namespace PurrPurrCoffee
{
    public class EventsManager : MonoBehaviour
    {
        public event Action ClientApproaching;
        public event Action DemonTime;
        public event Action ScreamerTime;

        [SerializeField]
        private EventsManagerConfig _config;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
        
        }

        // Update is called once per frame
        private void Update()
        {
        
        }
    }
}
