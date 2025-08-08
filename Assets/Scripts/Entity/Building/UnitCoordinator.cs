using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitCoordinator
{
    private readonly DiamondDispatcher _diamondDispatcher;
    private Vector3 _buildingPosition;
    private readonly List<Unit> _units = new();
    private Unit _unitBuilder;

    public event Action<Diamond> Delivered;
    public event Action<Unit> UnitPointBuildArrived;

    public int Count => _units.Count;

    public Unit UnitBuilder => _unitBuilder;

    public UnitCoordinator(DiamondDispatcher diamondDispatcher, Vector3 buildingPosition)
    {
        _diamondDispatcher = diamondDispatcher;
        _buildingPosition = buildingPosition;

        _diamondDispatcher.DiamondSpawned += OnSpawnDiamond;

        foreach (Unit unit in _units)
            SubscribeUnit(unit);
    }

    public void AddUnit(Unit unit)
    {
        _units.Add(unit);
        SubscribeUnit(unit);

        if (unit.IsWorking == false)
            SendUnitCollectDiamond(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        _units.Remove(unit);
        UnsubscribeUnit(unit);
    }

    public void SendFreeUnitsCollectDiamonds()
    {
        foreach (Unit unit in _units)
            if (unit.IsWorking == false)
                SendUnitCollectDiamond(unit);
    }

    public void SendFreeUnitBuildBuilding(PointBuildIndicator indicator)
    {
        if (_unitBuilder != null)
        {
            _unitBuilder.MoveTo(indicator.transform.position);
            return;
        }

        foreach (Unit unit in _units)
        {
            if (unit.IsWorking == false)
            {
                _unitBuilder = unit;
                _unitBuilder.SetPointBuild(indicator);
                _unitBuilder.MoveTo(indicator.transform.position);
                return;
            }
        }
    }

    public void ResetBuildUnit()
    {
        _unitBuilder.ResetPointBuildIndicator();
        _unitBuilder = null;
    }

    private void SubscribeUnit(Unit unit)
    {
        unit.DiamondCollected += OnCollected;
        unit.Delivered += OnDelivered;
        unit.PointBuildArrived += OnArrivalPointBuild;
        unit.HasFree += OnFreeUnit;
    }

    private void UnsubscribeUnit(Unit unit)
    {
        unit.DiamondCollected -= OnCollected;
        unit.Delivered -= OnDelivered;
        unit.PointBuildArrived -= OnArrivalPointBuild;
        unit.HasFree -= OnFreeUnit;
    }

    private void OnSpawnDiamond() =>
        SendFreeUnitsCollectDiamonds();

    private void OnDelivered(Unit unit, Diamond diamond)
    {
        unit.ClearCargo();
        Delivered?.Invoke(diamond);
    }

    private void OnArrivalPointBuild(Unit unit) =>
        UnitPointBuildArrived?.Invoke(unit);

    private void OnCollected(Unit unit) =>
        unit.MoveTo(_buildingPosition);

    private void OnFreeUnit(Unit unit) =>
        SendUnitCollectDiamond(unit);

    private void SendUnitCollectDiamond(Unit unit)
    {
        if (_diamondDispatcher.TryGetClosestTo(unit.transform.position, out Diamond closestDiamond))
        {
            unit.SetDesiredDiamond(closestDiamond);
            unit.MoveTo(closestDiamond.transform.position);
        }
    }
}