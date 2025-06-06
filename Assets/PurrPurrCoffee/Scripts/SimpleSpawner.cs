using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SimpleSpawner : MonoBehaviour
{
    [Inject]
    public void Initialize(DiContainer diContainer)
    {
        _diContainer = diContainer;
    }

    [SerializeField]
    private GameObject _prefab;
    [SerializeField]
    private Vector3 _minPosition;
    [SerializeField]
    private Vector3 _maxPosition;
    private DiContainer _diContainer;
    private readonly List<GameObject> _objs = new();

    IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            Vector3 position = new(
                Random.Range(_minPosition.x, _maxPosition.x),
                Random.Range(_minPosition.y, _maxPosition.y),
                Random.Range(_minPosition.z, _maxPosition.z));
            var instance = _diContainer.InstantiatePrefab(_prefab, position, Quaternion.identity, transform);
            _objs.Add(instance);
        }
    }
    private void OnDestroy()
    {
        foreach (var obj in _objs)
        {
            Destroy(obj);
        }
    }
}
