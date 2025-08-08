using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour
{
    [SerializeField] private HoldingPoint _holdingPoint;
    [SerializeField] private Detector _detector;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private PathDrawer _pathDrawer;

    private Diamond _cargoDiamond;
    private Diamond _desiredDiamond;
    private PointBuildIndicator _pointBuildIndicator;

    public event Action<Unit> HasFree;
    public event Action<Unit> DiamondCollected;
    public event Action<Unit, Diamond> Delivered;
    public event Action<Unit> PointBuildArrived;

    public bool IsWorking =>
        _cargoDiamond != null || _desiredDiamond != null || _pointBuildIndicator != null;

    public PointBuildIndicator PointBuildIndicator => _pointBuildIndicator;

    private void Update() =>
        _pathDrawer.Draw();

    private void OnEnable() =>
        _detector.Detected += OnCollision;

    private void OnDisable() =>
        _detector.Detected -= OnCollision;

    public void Init() =>
        _pathDrawer.Init(_agent);

    public void ResetPointBuildIndicator()
    {
        _pointBuildIndicator = null;
        _pathDrawer.SetCollectColor();
    }

    public void SetPointBuild(PointBuildIndicator pointBuildIndicator)
    {
        _pathDrawer.SetBuildColor();
        _pointBuildIndicator = pointBuildIndicator;
    }

    public void SetDesiredDiamond(Diamond diamond)
    {
        _desiredDiamond = diamond;
        diamond.Picked += OnPickUp;
    }

    public void MoveTo(Vector3 position) =>
        _agent.SetDestination(position);

    public void ClearCargo()
    {
        _cargoDiamond = null;
        _desiredDiamond = null;
    }

    private void OnCollision(Collider other)
    {
        if (other.TryGetComponent(out Building _) && _cargoDiamond != null)
            Delivered?.Invoke(this, _cargoDiamond);

        if (other.TryGetComponent(out Diamond diamond) && diamond == _desiredDiamond)
            Collect(diamond);

        if (other.TryGetComponent(out PointBuildIndicator indicator) && indicator == _pointBuildIndicator)
            PointBuildArrived(this);
    }

    private void OnPickUp(Diamond diamond)
    {
        diamond.Picked -= OnPickUp;
        HasFree?.Invoke(this);
    }

    private void Collect(Diamond diamond)
    {
        diamond.Picked -= OnPickUp;
        _cargoDiamond = diamond;
        diamond.PickUp();
        diamond.transform.parent = transform;
        diamond.transform.position = _holdingPoint.transform.position;
        DiamondCollected?.Invoke(this);
    }
}