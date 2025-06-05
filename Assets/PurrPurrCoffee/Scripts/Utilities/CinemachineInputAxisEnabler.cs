using PurrPurrCoffee.Input.Abstractions;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(CinemachineInputAxisController))]
public class CinemachineInputAxisEnabler : MonoBehaviour
{
    [Inject]
    public void Initialize(IInputActionService inputActionService)
    {
        _inputActionService = inputActionService;
    }

    [SerializeField]
    private ActionMap _axesActionMap = ActionMap.Player;

    private IInputActionService _inputActionService;
    private CinemachineInputAxisController _cinemachineInputAxisController;

    private void Awake()
    {
        _cinemachineInputAxisController = GetComponent<CinemachineInputAxisController>();
    }
    private void OnEnable()
    {
        _inputActionService.ActionMapChanged += OnActionMapChanged;
    }
    private void OnDisable()
    {
        _inputActionService.ActionMapChanged -= OnActionMapChanged;
    }

    private void OnActionMapChanged(ActionMap actionMap)
    {
        foreach (var controller in _cinemachineInputAxisController.Controllers)
        {
            controller.Enabled = actionMap == _axesActionMap;
        }
    }
}
