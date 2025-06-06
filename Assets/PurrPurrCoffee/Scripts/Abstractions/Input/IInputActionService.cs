#nullable enable

using System;
using UnityEngine;

namespace PurrPurrCoffee.Input.Abstractions
{
    public enum ActionMap
    {
        Player,
        UI
    }

    public delegate void MoveDelegate(Vector2 moveDelta);
    public delegate void LookDelegate(Vector2 lookDelta);

    public delegate void NavigateDelegate(Vector2 value);
    public delegate void PointDelegate(Vector2 value);
    public delegate void ScrollWheelDelegate(Vector2 value);
    public delegate void TrackedDevicePositionDelegate(Vector3 value);
    public delegate void TrackedDeviceOrientationDelegate(Quaternion value);

    public interface IInputActionService
    {
        #region Player Actions

        public Vector2 LastLookDelta { get; }
        public Vector2 LastMoveDelta { get; }
        public bool IsSprintActive { get; }
        public bool IsCrouchActive { get; }
        public bool IsJumpActive { get; }
        public bool IsAttackActive { get; }
        public bool IsInteractActive { get; }

        public event LookDelegate? Look;
        public event Action? LookCancel;
        public event MoveDelegate? Move;
        public event Action? MoveCancel;
        public event Action? Sprint;
        public event Action? SprintCancel;
        public event Action? Crouch;
        public event Action? CrouchCancel;
        public event Action? Jump;
        public event Action? JumpCancel;
        public event Action? Attack;
        public event Action? AttackCancel;
        public event Action? Interact;
        public event Action? InteractCancel;
        public event Action? Pause;

        #endregion

        #region UI Actions

        public Vector2 LastNavigateValue { get; }
        public Vector2 LastPointValue { get; }
        public Vector2 LastScrollWheelValue { get; }
        public Vector3 LastTrackedDevicePosition { get; }
        public Quaternion LastTrackedDeviceOrientation { get; }

        public event NavigateDelegate? Navigate;
        public event Action? NavigateCancel;
        public event PointDelegate? Point;
        public event Action? PointCancel;
        public event ScrollWheelDelegate? ScrollWheel;
        public event Action? ScrollWheelCancel;
        public event TrackedDevicePositionDelegate? TrackedDevicePosition;
        public event Action? TrackedDevicePositionCancel;
        public event TrackedDeviceOrientationDelegate? TrackedDeviceOrientation;
        public event Action? TrackedDeviceOrientationCancel;
        public event Action? Submit;
        public event Action? Cancel;
        public event Action? Click;
        public event Action? RightClick;
        public event Action? MiddleClick;

        #endregion

        public event Action<ActionMap>? ActionMapChanged;

        public void SetActionMap(ActionMap actionMap);
        public void Disable();
    }
}
