using System.Collections.Generic;
using UnityEngine;

public class Waypath : MonoBehaviour
{
    public Vector3 GetNextPoint(out bool isLastPoint)
    {
        isLastPoint = false;
        _currentPoint++;
        if (_isCyclic)
        {
            _currentPoint %= _points.Count;
        }
        else if(_currentPoint >= _points.Count)
        {
            isLastPoint = true;
            _currentPoint = _points.Count - 1;
        }
        return _points[_currentPoint].position;
    }
    public void ResetPath() => _currentPoint = -1;

    [SerializeField]
    private List<Transform> _points = new();
    [SerializeField]
    private bool _isCyclic = false;
    private int _currentPoint = -1;
}
