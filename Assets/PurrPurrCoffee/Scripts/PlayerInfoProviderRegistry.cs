using PurrPurrCoffee.Abstractions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PurrPurrCoffee
{
    public class PlayerInfoProviderRegistry : IPlayerInfoProviderRegistry
    {
        public IReadOnlyList<IPlayerInfoProvider> Instances => _instances;

        public event Action<IPlayerInfoProvider> Registred;
        public event Action<IPlayerInfoProvider> Unregistred;

        public void Register(IPlayerInfoProvider playerInfoProvider)
        {
            _instances.Add(playerInfoProvider);
            Registred?.Invoke(playerInfoProvider);
        }
        public void Unregister(IPlayerInfoProvider playerInfoProvider)
        {
            _instances.Remove(playerInfoProvider);
            Unregistred?.Invoke(playerInfoProvider);
        }

        readonly List<IPlayerInfoProvider> _instances = new();
    }
}
