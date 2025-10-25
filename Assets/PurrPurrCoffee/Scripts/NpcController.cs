using System;
using System.Collections;

using UnityEngine;
using UnityEngine.AI;

using PurrPurrCoffee.Interactions;

[RequireComponent(typeof(NavMeshAgent))]
public class NpcController : MonoBehaviour
{
    public Waypath Waypath { get => _waypath; set { _waypoint = null; _waypath = value; _isPathEnds = false; } }
    public Transform Waypoint { get => _waypoint; set { _waypath = null; _waypoint = value; _isPathEnds = false; } }
    public float StopDistance { get; set; } = 2f;

    public event Action WaypathCompleted;
    public event Action WaypointReached;
    public event Action WaypointUnreachable;

    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private Waypath _waypath;
    [SerializeField]
    private Transform _waypoint = null;
    [SerializeField]
    private Transform _headTransform;
    [SerializeField, Min(0)]
    private float _minUnreachableWaypointDetectTime = 1f;
    private NavMeshAgent _navMeshAgent;
    private DoorOpener _currentDoorOpener;
    private bool _isWaitingForAnimation;
    private Vector3 _target;
    private bool _isTargetSet = false;
    private bool _isPathEnds = false;
    private bool _isReachedWaypoint = false;
    private float _npcStopTime = 0f;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        //_target = _waypath.GetNextPoint();
    }
    private void Update()
    {
        if ((_waypath != null || _waypoint != null) && !_isPathEnds && !_isWaitingForAnimation)
        {
            UpdateTarget();

            if (!_isPathEnds && _animator != null)
            {
                if (_navMeshAgent.desiredVelocity.magnitude < 0.001f) // NPC стоит
                {
                    _animator.SetBool("IsMoving", false);
                    if (_waypoint != null)
                    {
                        if (_npcStopTime == 0)
                        {
                            _npcStopTime = Time.realtimeSinceStartup;
                        }
                        var timeSinceNpcStops = Time.realtimeSinceStartup - _npcStopTime;
                        if (timeSinceNpcStops >= _minUnreachableWaypointDetectTime)
                        {
                            WaypointUnreachable?.Invoke();
                        }
                    }
                }
                else
                {
                    _npcStopTime = 0;
                    _animator.SetBool("IsMoving", true);
                    _animator.SetFloat("InputMagnitude", _navMeshAgent.velocity.magnitude);
                    /*Vector3 desiredDirection = _navMeshAgent.desiredVelocity.normalized;
                    Vector3 currentForward = transform.forward;
                    // ѕолучаем угол между направлением взгл€да и направлением движени€
                    float angle = Vector3.SignedAngle(currentForward, desiredDirection, Vector3.up);
                    if (Mathf.Abs(angle) > 30f)
                    {
                        if (!_isTurning)
                        {
                            _isTurning = true;
                            //_navMeshAgent.speed *= 0.5f;
                        }
                    }
                    else
                    {
                        if (_isTurning)
                        {
                            //_navMeshAgent.speed *= 2f;
                            _isTurning = false;
                        }
                    }*/
                    _animator.SetBool("IsGrounded", _navMeshAgent.isOnNavMesh);
                    _animator.SetBool("IsFalling", !_navMeshAgent.isOnNavMesh);
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Collided with {other.name}");
        if (!_isWaitingForAnimation && other.TryGetComponent(out _currentDoorOpener))
        {
            Debug.Log($"Collided with DoorOPENER {other.name}");
            if (!_currentDoorOpener.IsOpen)
            {
                _isWaitingForAnimation = true;
                _navMeshAgent.isStopped = true;
                _animator.SetBool("IsMoving", false);
                _currentDoorOpener.Opened += OnDoorOpened;
                _currentDoorOpener.Open();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!_isWaitingForAnimation && other.TryGetComponent(out _currentDoorOpener))
        {
            Debug.Log($"CollidedExit with {other.name}");
            if (_currentDoorOpener.IsOpen)
            {
                _isWaitingForAnimation = true;
                _navMeshAgent.isStopped = true;
                _animator.SetBool("IsMoving", false);
                _currentDoorOpener.Closed += OnDoorClosed;
                _currentDoorOpener.Close();
            }
        }
    }
    private bool _IsTargetSet => _isTargetSet;// _navMeshAgent.destination == _target;
    private bool _IsTargetReached => !_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance;//Vector3.Distance(transform.position, _target) <= _navMeshAgent.stoppingDistance;
    private void UpdateTarget()
    {
        if (!_IsTargetSet || _IsTargetReached)
        {
            if (_waypath != null)
            {
                _target = _waypath.GetNextPoint(out _isPathEnds);
            }
            else
            {
                _target = _waypoint.position;
            }
            _navMeshAgent.destination = _target;
            _isTargetSet = true;
        }
        if (_waypoint != null)
        {
            if ((_navMeshAgent.gameObject.transform.position - _waypoint.position).magnitude <= StopDistance)
            {
                if (!_isReachedWaypoint)
                {
                    _isReachedWaypoint = true;
                    _animator.SetBool("IsMoving", true);
                    _navMeshAgent.isStopped = true;
                    WaypointReached?.Invoke();
                }
            }
            else
            {
                _navMeshAgent.isStopped = false;
                _isReachedWaypoint = false;
                _animator.SetBool("IsMoving", false);
            }
        }
        if (_isPathEnds)
        {
            _animator.SetBool("IsMoving", false);
            WaypathCompleted?.Invoke();
        }
    }
    private void OnDoorOpened()
    {
        if (_currentDoorOpener != null)
        {
            _currentDoorOpener.Opened -= OnDoorOpened;
            _currentDoorOpener.Closed -= OnDoorClosed;
            _currentDoorOpener = null;
            _navMeshAgent.isStopped = false;
            _animator.SetBool("IsMoving", true);
            StartCoroutine(WaitForPassThrough());
        }
    }
    private void OnDoorClosed()
    {
        if (_currentDoorOpener != null)
        {
            _currentDoorOpener.Opened -= OnDoorOpened;
            _currentDoorOpener.Closed -= OnDoorClosed;
            _currentDoorOpener = null;
            _navMeshAgent.isStopped = false;
            _animator.SetBool("IsMoving", true);
            StartCoroutine(WaitForPassThrough());
        }
    }
    private IEnumerator WaitForPassThrough() // ещЄ один костыль, чтобы дверь не закрывалась сразу после откыти€, если NPC "случайно" отошЄл обратно
    {
        yield return new WaitForSeconds(1);
        _isWaitingForAnimation = false;
    }
}
