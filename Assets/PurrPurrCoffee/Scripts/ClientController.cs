using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace PurrPurrCoffee
{
    using Abstractions;
    
    public class ClientController : MonoBehaviour, IClientController
    {
        public event Action ClientReturned;
        public event Action<GameObject, bool> ChaseFinished;

        public Vector3 ClientPosition => _currentClient != null ? _currentClient.transform.position : Vector3.zero;
        [Inject]
        public void Initialize(ILogger logger, DiContainer diContainer)
        {
            _logger = logger;
            _diContainer = diContainer;
        }

        private readonly Lazy<WaypathProvider> _waypathProvider = new(() => FindFirstObjectByType<WaypathProvider>());
        public void SendClient()
        {
            if (_nextClientIndex < _clientPrefabs.Count)
            {
                _logger.Log($"{nameof(SendClient)}(id={_nextClientIndex})");
                if (_currentClient != null)
                {
                    ReturnCurrentClient();
                }
                var waypath = _waypathProvider.Value.Waypath;
                waypath.ResetPath();
                var spawnPosition = waypath.GetNextPoint(out _);
                _currentClient = _clients[_nextClientIndex];
                _currentClient.transform.position = spawnPosition;
                _currentClient.gameObject.SetActive(true);
                _currentClient.Waypath = waypath;
                _nextClientIndex++;
                _nextClientIndex %= _clientPrefabs.Count;
            }
        }
        public void ReturnCurrentClient()
        {
            _logger.Log($"{nameof(ReturnCurrentClient)}(id={_nextClientIndex})");
            var npcController = _currentClient;
            _waypathProvider.Value.BackWaypath.ResetPath();
            npcController.Waypath = _waypathProvider.Value.BackWaypath;
            npcController.WaypathCompleted += OnClientReturned;
        }
        public void SendEnemy()
        {
            _logger.Log($"{nameof(SendEnemy)}");
            var waypath = _waypathProvider.Value.ShortWaypath;
            waypath.ResetPath();
            var spawnPosition = waypath.GetNextPoint(out _);
            _enemy.transform.position = spawnPosition;
            _enemy.gameObject.SetActive(true);
            _enemy.Waypath = waypath;
        }
        public void StartChase(Transform transform)
        {
            _enemy.Waypoint = transform;
            _enemy.WaypointReached -= OnEnemyWaypointReached;
            _enemy.WaypointUnreachable -= OnEnemyWaypointUnreachable;
            _enemy.WaypointReached += OnEnemyWaypointReached;
            _enemy.WaypointUnreachable += OnEnemyWaypointUnreachable;
        }

        private void OnEnemyWaypointReached()
        {
            ChaseFinished?.Invoke(_enemy.gameObject, true);
        }
        private void OnEnemyWaypointUnreachable()
        {
            ChaseFinished?.Invoke(_enemy.gameObject, false);
        }

        public void ReturnEnemy()
        {
            _logger.Log($"{nameof(ReturnEnemy)}");
            _waypathProvider.Value.BackWaypath.ResetPath();
            _enemy.Waypath = _waypathProvider.Value.BackWaypath;
            void OnEnemyReturned()
            {
                var npcController = _enemy;
                npcController.WaypathCompleted -= OnEnemyReturned;
                npcController.gameObject.SetActive(false);
            }
            _enemy.WaypathCompleted += OnEnemyReturned;
        }

        [SerializeField]
        private List<GameObject> _clientPrefabs = new();
        [SerializeField]
        private GameObject _enemyPrefab;
        [SerializeField, Min(0)]
        private float _attackDistance;
        [SerializeField]
        private Transform _spawnParentTransform;
        [SerializeField]
        private Vector3 _spawnPosition;
        [SerializeField]
        private Quaternion _spawnRotation;
        private ILogger _logger;
        private DiContainer _diContainer;
        private NpcController _currentClient = null;
        private int _nextClientIndex = 0;
        private List<NpcController> _clients = new();
        private NpcController _enemy;

        private void Awake()
        {
            foreach (var clientPrefab in _clientPrefabs)
            {
                var client = _diContainer.InstantiatePrefab(clientPrefab, Vector3.zero, Quaternion.identity, _spawnParentTransform);
                var npcController = client.GetComponentInChildren<NpcController>();
                npcController.gameObject.SetActive(false);
                _clients.Add(npcController);
            }
            var enemy = _diContainer.InstantiatePrefab(_enemyPrefab, Vector3.zero, Quaternion.identity, _spawnParentTransform);
            var enemyController = enemy.GetComponentInChildren<NpcController>();
            enemyController.gameObject.SetActive(false);
            _enemy = enemyController;
        }
        private void OnClientReturned()
        {
            var npcController = _currentClient;
            npcController.WaypathCompleted -= OnClientReturned;
            npcController.gameObject.SetActive(false);
            //Destroy(_currentClient.gameObject);
            _currentClient = null;
            ClientReturned?.Invoke();
        }
    }
}
