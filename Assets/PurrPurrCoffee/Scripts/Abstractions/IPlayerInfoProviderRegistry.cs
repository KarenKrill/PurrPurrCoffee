using System;
using System.Collections.Generic;

namespace PurrPurrCoffee.Abstractions
{
    public interface IPlayerInfoProviderRegistry
    {
        IReadOnlyList<IPlayerInfoProvider> Instances { get; }
        event Action<IPlayerInfoProvider> Registred;
        event Action<IPlayerInfoProvider> Unregistred;

        void Register(IPlayerInfoProvider playerInfoProvider);
        void Unregister(IPlayerInfoProvider playerInfoProvider);
    }
}
