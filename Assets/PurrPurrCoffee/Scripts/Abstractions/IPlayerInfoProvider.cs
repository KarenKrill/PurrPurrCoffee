using UnityEngine;

namespace PurrPurrCoffee.Abstractions
{
    public interface IPlayerInfoProvider
    {
        Transform PlayerTransform { get; }
    }
}
