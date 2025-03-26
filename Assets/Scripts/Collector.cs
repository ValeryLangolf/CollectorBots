using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Collector : MonoBehaviour
{
    [SerializeField] private HoldingPoint _holdingPoint;
    [SerializeField] private Detector _detector;

    private Diamond _collectedDiamond;
    private NavMeshAgent _agent;

    public event Action<Collector, Diamond> DiamondCollided;
    public event Action<Collector, Diamond> Delivered;

    private void Awake() =>
        _agent = GetComponent<NavMeshAgent>();

    private void OnEnable() =>
        _detector.Detected += OnCollision;

    private void OnDisable() =>
        _detector.Detected -= OnCollision;

    public void MoveTo(Vector3 position) =>
        _agent.SetDestination(position);

    public void Collect(Diamond diamond)
    {
        _collectedDiamond = diamond;
        diamond.DisableCollider();
        diamond.transform.parent = transform;
        diamond.transform.position = _holdingPoint.transform.position;
    }

    public void ClearCargo() =>
        _collectedDiamond = null;

    private void OnCollision(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out BaseBuilding _) && _collectedDiamond != null)
            Delivered?.Invoke(this, _collectedDiamond);

        if (gameObject.TryGetComponent(out Diamond diamond) && _collectedDiamond == null)
            DiamondCollided?.Invoke(this, diamond);
    }
}