using PurrPurrCoffee.Abstractions;
using UnityEngine;
using Zenject;

public class CoffeeGrabber : MonoBehaviour // � ���� �������� ����� �������� ������� ������ ����! (�������� ������ �� �����)
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
