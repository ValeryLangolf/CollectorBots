using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class DiamondSpawner
{
    private const float Deviation = 0.45f;

    [SerializeField] private DiamondFactory _factory;
    [SerializeField] private List<SpawnPoint> _points;
    [SerializeField] private float _delay;
    [SerializeField] private int _maxCount;

    private MonoBehaviour _monoBehaviour;    
    private WaitForSeconds _wait;
    private readonly bool _isSpawn = true;
    private int _currentCount = 0;

    public event Action<Diamond> Spawned;

    public void Init(MonoBehaviour monoBehaviour)
    {
        _factory.Init();
        _monoBehaviour = monoBehaviour;        
        _wait = new WaitForSeconds(_delay);
    }

    public void StartSpawn() =>
        _monoBehaviour.StartCoroutine(SpawningDiamonds());

    private IEnumerator SpawningDiamonds()
    {
        while (_isSpawn)
        {
            yield return _wait;

            Spawn();
        }
    }

    private void Spawn()
    {
        if (_currentCount >= _maxCount)
            return;

        Diamond diamond = _factory.Create(GetRandomPosition());
        diamond.EnableCollider();
        diamond.Deactivated += OnDeactivation;

        _currentCount++;
        Spawned?.Invoke(diamond);
    }

    private Vector3 GetRandomPosition()
    {
        SpawnPoint point = _points[Random.Range(0, _points.Count)];

        return point.Position + Utils.GetRandomDeviationXZ(Deviation);
    }

    private void OnDeactivation(Diamond diamond)
    {
        diamond.Deactivated -= OnDeactivation;
        _factory.ReturnInPool(diamond);
        _currentCount--;
    }
}