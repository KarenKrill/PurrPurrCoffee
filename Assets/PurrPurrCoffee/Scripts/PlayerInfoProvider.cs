using UnityEngine;

using Zenject;

namespace PurrPurrCoffee
{
    using Abstractions;

    public class PlayerInfoProvider : MonoBehaviour, IPlayerInfoProvider
    {
        public Transform PlayerTransform => _playerTransform;

        [Inject]
        public void Initialize(IPlayerInfoProviderRegistry registry)
        {
            _registry = registry;
        }

        [SerializeField]
        private Transform _playerTransform;

        private IPlayerInfoProviderRegistry _registry;

        private void OnEnable()
        {
            _registry.Register(this);
        }
        private void OnDisable()
        {
            _registry.Unregister(this);
        }
    }
}
