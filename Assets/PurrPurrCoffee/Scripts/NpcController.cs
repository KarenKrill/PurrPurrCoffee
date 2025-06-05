using UnityEngine;

using System.Collections;
using UnityEngine.AI;
using PurrPurrCoffee.Interactions;
using System;

[RequireComponent(typeof(NavMeshAgent))]
public class NpcController : MonoBehaviour
{
    public Waypath Waypath { get => _waypath; set { _waypath = value; _isPathEnds = false; } }
    public event Action WaypathCompleted;

    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private Waypath _waypath;
    private NavMeshAgent _navMeshAgent;
    private DoorOpener _currentDoorOpener;
    private bool _isWaitingForAnimation;
    private Vector3 _target;
    private bool _isTargetSet = false;
    private bool _isPathEnds = false;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        //_target = _waypath.GetNextPoint();
    }
    private void Update()
    {
        if (_waypath != null && !_isPathEnds && !_isWaitingForAnimation)
        {
            UpdateTarget();

            if (!_isPathEnds && _animator != null)
            {
                if (_navMeshAgent.desiredVelocity.magnitude < 0.001f) // NPC стоит
                {
                    _animator.SetBool("IsMoving", false);
                }
                else
                {
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
        if (!_IsTargetSet)
        {
            _target = _waypath.GetNextPoint(out _isPathEnds);
            _navMeshAgent.destination = _target;
            _isTargetSet = true;
        }
        else if (_IsTargetReached)
        {
            Debug.Log("Target reached!");
            _target = _waypath.GetNextPoint(out _isPathEnds);
            _navMeshAgent.destination = _target;
            _isTargetSet = true;
        }
        if (_isPathEnds)
        {
            Debug.Log("PathEnds!");
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
