#nullable enable

using System;
using UnityEngine;

namespace PurrPurrCoffee.Abstractions
{
    public interface IClientController
    {
        event Action? ClientReturned;
        event Action<GameObject, bool>? ChaseFinished;

        Vector3 ClientPosition { get; }

        void SendClient();
        void SendEnemy();
        void StartChase(Transform transform);
        void ReturnCurrentClient();
        void ReturnEnemy();
    }
}
