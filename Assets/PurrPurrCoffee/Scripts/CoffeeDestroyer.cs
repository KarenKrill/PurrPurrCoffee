using PurrPurrCoffee.Abstractions;
using PurrPurrCoffee.Interactions;
using UnityEngine;
using Zenject;

public class CoffeeDestroyer : MonoBehaviour
{
    [Inject]
    public void Initialize(GameSession gameSession)
    {
        _gameSession = gameSession;
    }

    [SerializeField]
    private PickupInteractor _pickupInteractor;
    private GameSession _gameSession;

    private void OnEnable()
    {
        _gameSession.IsCoffeeInPlayerHandsChanged += OnIsCoffeeInPlayerHandsChanged;
    }
    private void OnDisable()
    {
        _gameSession.IsCoffeeInPlayerHandsChanged -= OnIsCoffeeInPlayerHandsChanged;
    }
    private void OnIsCoffeeInPlayerHandsChanged(bool state)
    {
        if (!state && _pickupInteractor.PickedInteractable != null)
        {
            var pickedItem = _pickupInteractor.PickedInteractable;
            _pickupInteractor.DropIfPicked();
            //pickedItem.gameObject.SetActive(false);
            //Destroy(pickedItem.gameObject);
        }
    }
}
