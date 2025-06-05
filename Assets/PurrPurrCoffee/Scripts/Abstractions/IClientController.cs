#nullable enable

using System;
using UnityEngine;

namespace PurrPurrCoffee.Abstractions
{
    public interface IClientController
    {
        event Action? ClientReturned;
        Vector3 ClientPosition { get; }
        void SendClient();
        void ReturnCurrentClient();
    }
}
