using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DiamondDispatcher
{
    [SerializeField] private DiamondSpawner _spawner;

    private readonly List<Diamond> _availableDiamonds = new();
    private readonly PathFinder _pathfinder = new();

    public event Action DiamondSpawned;

    public void Init(MonoBehaviour monoBehaviour) =>
        _spawner.Init(monoBehaviour);

    public void Subscribe() =>
        _spawner.Spawned += OnSpawnDiamond;

    public void Unsubscribe() =>
        _spawner.Spawned -= OnSpawnDiamond;

    public void StarSpawn() =>
        _spawner.StartSpawn();

    public bool TryGetClosestTo(Vector3 position, out Diamond diamond) =>
        _pathfinder.TryGetClosestDiamond(position, _availableDiamonds.AsReadOnly(), out diamond);

    private void OnSpawnDiamond(Diamond diamond)
    {
        _availableDiamonds.Add(diamond);
        diamond.Picked += OnDiamondPickUp;
        DiamondSpawned?.Invoke();
    }

    private void OnDiamondPickUp(Diamond diamond)
    {
        diamond.Picked -= OnDiamondPickUp;
        _availableDiamonds.Remove(diamond);
    }
}