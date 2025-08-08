using System;
using UnityEngine;

public class Building : MonoBehaviour
{
    private const int UnitPrice = 3;
    private const int BuildingPrice = 5;
    private const int MinimumUnitsForConstruction = 2;

    [SerializeField] private WalletView _walletView;
    [SerializeField] private SpawnPoint _unitSpawnPoint;
    [SerializeField] private float _unitSpawnDeviation = 0.2f;

    private UnitCoordinator _unitCoordinator;
    private UnitFactory _unitFactory;
    private readonly Wallet _wallet = new();
    private PointBuildIndicator _buildIndicator;
    private bool _isArrivalPointBuild;

    public event Action<Building> UnitPointBuildArrived;

    public PointBuildIndicator BuildIndicator => _buildIndicator;

    public Unit UnitBuilder => _unitCoordinator.UnitBuilder;

    public bool EnoughUnitsToBuild => _unitCoordinator.Count >= MinimumUnitsForConstruction;

    private void OnDestroy() =>
        Unsubscribe();

    public void Init(DiamondDispatcher diamondDispatcher, UnitFactory unitFactory)
    {
        _unitCoordinator = new(diamondDispatcher, transform.position);
        _unitFactory = unitFactory;
        _walletView.Init(_wallet);

        Subscribe();
    }

    public void CreateUnit()
    {
        Vector3 position = _unitSpawnPoint.transform.position + Utils.GetRandomDeviationXZ(_unitSpawnDeviation);
        AddUnit(_unitFactory.CreateUnit(position));        
    }

    public void AddUnit(Unit unit)
    {
        _unitCoordinator.AddUnit(unit);
        _unitCoordinator.SendFreeUnitsCollectDiamonds();
    }

    public void RemoveUnit(Unit unit) =>
        _unitCoordinator.RemoveUnit(unit);

    public void ResetBuildParams()
    {
        ResetArrivalPointBuild();
        _buildIndicator = null;
        _unitCoordinator.ResetBuildUnit();
    }

    public void ResetArrivalPointBuild() =>
        _isArrivalPointBuild = false;

    public void SetBuildIndicator(PointBuildIndicator buildIndicator)
    {
        if (_unitCoordinator.Count >= MinimumUnitsForConstruction)
        {
            _buildIndicator = buildIndicator;
            _unitCoordinator.SendFreeUnitBuildBuilding(_buildIndicator);
        }
    }

    private void Subscribe()
    {
        _unitCoordinator.Delivered += OnDeliveredDiamond;
        _unitCoordinator.UnitPointBuildArrived += OnUnitArrivalPointBuild;
        _walletView.Subscribe();
    }

    private void Unsubscribe()
    {
        _unitCoordinator.Delivered -= OnDeliveredDiamond;
        _unitCoordinator.UnitPointBuildArrived -= OnUnitArrivalPointBuild;
        _walletView.Unsubscribe();
    }

    private void OnDeliveredDiamond(Diamond diamond)
    {
        if (diamond != null)
        {
            _wallet.Increase();
            diamond.Deactivate();

            TryHandleBuildOrUnit();
        }
    }

    private void OnUnitArrivalPointBuild(Unit unit)
    {
        _isArrivalPointBuild = true;
        HandlePayByild();
    }        

    private void TryHandleBuildOrUnit()
    {
        if (_buildIndicator != null)
            HandlePayByild();
        else
            HandlePayUnit();
    }

    private void HandlePayByild()
    {
        SetBuildIndicator(_buildIndicator);

        if (_isArrivalPointBuild && _wallet.TryPay(BuildingPrice))
            UnitPointBuildArrived?.Invoke(this);
    }

    private void HandlePayUnit()
    {
        if (_wallet.TryPay(UnitPrice))
            CreateUnit();
    }
}