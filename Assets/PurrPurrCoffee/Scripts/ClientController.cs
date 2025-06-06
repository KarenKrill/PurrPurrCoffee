using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace PurrPurrCoffee
{
    using Abstractions;
    using System;

    public class ClientController : MonoBehaviour, IClientController
    {
        public event Action ClientReturned;
        public Vector3 ClientPosition => _currentClient != null ? _currentClient.transform.position : Vector3.zero;
        [Inject]
        public void Initialize(ILogger logger,
            DiContainer diContainer) // не самое лучшее решение зависеть от контейнера, но при скоростной разработке сойдёт
        {
            _logger = logger;
            _diContainer = diContainer;
        }

        private readonly Lazy<WaypathProvider> _waypathProvider = new(() => FindFirstObjectByType<WaypathProvider>());
        public void SendClient() => _sendClientRequest = true;
        public void ReturnCurrentClient() => _returnClientRequest = true;

        [SerializeField]
        private List<GameObject> _clientPrefabs = new();
        [SerializeField]
        private Transform _spawnParentTransform;
        [SerializeField]
        private Vector3 _spawnPosition;
        [SerializeField]
        private Quaternion _spawnRotation;
        private ILogger _logger;
        private DiContainer _diContainer;
        private GameObject _currentClient = null;
        private int _nextClientIndex = 0;
        private bool _sendClientRequest = false;
        private bool _returnClientRequest = false;

        private void Update()
        {
            if (_sendClientRequest)
            {
                _sendClientRequest = false;
                SendClientInternal();
            }
            if (_currentClient != null && _returnClientRequest)
            {
                _returnClientRequest = false;
                ReturnCurrentClientInternal();
            }
        }
        private void SendClientInternal()
        {
            if (_nextClientIndex < _clientPrefabs.Count)
            {
                _logger.Log($"{nameof(SendClient)}(id={_nextClientIndex})");
                if (_currentClient != null)
                {
                    ReturnCurrentClientInternal();
                }
                var waypath = _waypathProvider.Value.Waypath;
                waypath.ResetPath();
                var spawnPosition = waypath.GetNextPoint(out _);
                _currentClient = _diContainer.InstantiatePrefab(_clientPrefabs[_nextClientIndex], spawnPosition, Quaternion.identity, _spawnParentTransform);
                _currentClient.GetComponentInChildren<NpcController>().Waypath = waypath;
                _nextClientIndex++;
                _nextClientIndex %= _clientPrefabs.Count;
            }
        }
        private void ReturnCurrentClientInternal()
        {
            _logger.Log($"{nameof(ReturnCurrentClientInternal)}(id={_nextClientIndex})");
            var npcController = _currentClient.GetComponentInChildren<NpcController>();
            _waypathProvider.Value.BackWaypath.ResetPath();
            npcController.Waypath = _waypathProvider.Value.BackWaypath;
            npcController.WaypathCompleted += OnClientReturned;
        }
        private void OnClientReturned()
        {
            var npcController = _currentClient.GetComponentInChildren<NpcController>();
            npcController.WaypathCompleted -= OnClientReturned;
            _currentClient.SetActive(false);
            Destroy(_currentClient);
            _currentClient = null;
            ClientReturned?.Invoke();
        }
    }
}
