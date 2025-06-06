using PurrPurrCoffee.Abstractions;
using UnityEngine;
using Zenject;

public class CoffeeGrabber : MonoBehaviour // с этим скриптом любой поднятый предмет станет кофе! (дедлайны никого не щадят)
{
    [Inject]
    public void Initialize(GameSession gameSession)
    {
        _gameSession = gameSession;
    }
    public void GrabCoffee()
    {
        _gameSession.IsCoffeeInPlayerHands = true;
    }

    private GameSession _gameSession;
}
