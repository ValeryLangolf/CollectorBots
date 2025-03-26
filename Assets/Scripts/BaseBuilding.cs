using System.Collections.Generic;
using UnityEngine;

public class BaseBuilding : MonoBehaviour
{
    [SerializeField] private Spawner _spawner;
    [SerializeField] private Counter _counter;
    [SerializeField] private List<Collector> _collectors;

    private readonly List<Diamond> _availableDiamonds = new();
    private readonly Dictionary<Collector, Diamond> _collectorsDictionary = new();

    private void Awake()
    {
        foreach (Collector collector in _collectors)
            _collectorsDictionary[collector] = null;
    }

    private void OnEnable()
    {
        _spawner.Spawned += OnSpawned;

        foreach (Collector collector in _collectors)
        {
            collector.DiamondCollided += OnDiamondCollided;
            collector.Delivered += OnDelivered;
        }
    }

    private void OnDisable()
    {
        _spawner.Spawned -= OnSpawned;

        foreach (Collector collector in _collectors)
        {
            collector.DiamondCollided -= OnDiamondCollided;
            collector.Delivered -= OnDelivered;
        }
    }

    private void OnSpawned(Diamond diamond)
    {
        _availableDiamonds.Add(diamond);
        SendRob();
    }

    private void OnDiamondCollided(Collector botCollector, Diamond diamond)
    {
        if (_collectorsDictionary[botCollector] != diamond)
            return;

        botCollector.Collect(diamond);
        botCollector.MoveTo(transform.position);
    }

    private void OnDelivered(Collector botCollector, Diamond diamond)
    {
        botCollector.ClearCargo();
        diamond.Deactivate();
        _collectorsDictionary[botCollector] = null;
        SendRob();

        _counter.Increase();
    }

    private void SendRob()
    {
        List<Diamond> availableDiamonds = new(_availableDiamonds);

        foreach (Diamond availableDiamond in availableDiamonds)
            Reserve(availableDiamond);
    }

    private void Reserve(Diamond diamond)
    {
        if (diamond == null)
            return;

        foreach (Collector collector in _collectorsDictionary.Keys)
            if (TrySendToDiamond(collector, diamond))
                return;
    }

    private bool TrySendToDiamond(Collector collector, Diamond diamond)
    {
        if (_collectorsDictionary[collector] != null)
            return false;

        _availableDiamonds.Remove(diamond);
        _collectorsDictionary[collector] = diamond;
        collector.MoveTo(diamond.transform.position);

        return true;
    }
}