using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private const float Deviation = 0.45f;

    [SerializeField] private Diamond _prefab;
    [SerializeField] private List<PointSpawn> _points;
    [SerializeField] private float _delay;
    [SerializeField] private int _maxCount;

    private Pool _pool;
    private WaitForSeconds _waiting;
    private bool _isSpawning = true;

    public event Action<Diamond> Spawned;

    private void Awake()
    {
        _pool = new(_prefab, transform, _maxCount);
        _waiting = new WaitForSeconds(_delay);
    }

    private void Start() =>
        StartCoroutine(SpawningDiamonds());

    private IEnumerator SpawningDiamonds()
    {
        while(_isSpawning)
        {
            yield return _waiting;

            Spawn();
        }
    }

    private void Spawn()
    {
        Diamond diamond = _pool.Get();

        if(diamond == null)
            return;

        diamond.transform.position = GetRandomPosition();
        diamond.EnableCollider();
        Spawned?.Invoke(diamond);
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 position = _points[UnityEngine.Random.Range(0, _points.Count)].transform.position;
        Vector3 deviation = new (UnityEngine.Random.Range(-Deviation, Deviation), 0, UnityEngine.Random.Range(-Deviation, Deviation));

        return position + deviation;
    }
}