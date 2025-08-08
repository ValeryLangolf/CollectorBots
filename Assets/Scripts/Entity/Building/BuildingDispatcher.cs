using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuildingDispatcher
{
    [SerializeField] private BuildingFactory _buildingFactory;
    [SerializeField] private UnitFactory _unitFactory;
    [SerializeField] private Transform _initialPosition;
    [SerializeField] private int _initialCountUnits;

    private DiamondDispatcher _diamondDispatcher;
    private readonly List<Building> _buildings = new();

    public void Init(DiamondDispatcher diamondDispatcher)
    {
        _diamondDispatcher = diamondDispatcher;
        Vector3 position = _initialPosition.transform.position;
        Building building = _buildingFactory.Create(position);

        building.Init(diamondDispatcher, _unitFactory);

        for (int i = 0; i < _initialCountUnits; i++)
            building.CreateUnit();

        _buildings.Add(building);
        Subscribe(building);
    }

    public void Unsubscribe()
    {
        foreach(Building building in _buildings)
            building.UnitPointBuildArrived -= OnReadyBuild;

        _buildings.Clear();
    }

    private void Subscribe(Building building) =>
        building.UnitPointBuildArrived += OnReadyBuild;

    private void OnReadyBuild(Building oldBuilding)
    {
        Unit unit = oldBuilding.UnitBuilder;
        PointBuildIndicator pointBuildIndicator = unit.PointBuildIndicator;
        Vector3 position = pointBuildIndicator.transform.position;

        pointBuildIndicator.Deactivate();
        oldBuilding.ResetBuildParams();
        oldBuilding.RemoveUnit(unit);

        Building newBuilding = _buildingFactory.Create(position);

        Subscribe(newBuilding);
        newBuilding.Init(_diamondDispatcher, _unitFactory);
        newBuilding.AddUnit(unit);
    }
}