using System;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class PathDrawer
{
    [SerializeField] private Color _buildColor;
    [SerializeField] private Color _collectColor;

    private LineRenderer _lineRenderer;
    private Material _material;
    private NavMeshAgent _agent;

    public void Init(NavMeshAgent agent)
    {
        _agent = agent;

        if (_agent.TryGetComponent(out _lineRenderer) == false)
            throw new ArgumentException("PathDrawer: не удалось получить компонент LineRenderer");

        _material = _lineRenderer.material;

        SetCollectColor();
    }

    public void SetBuildColor() =>
        _material.SetColor("_EmissionColor", _buildColor);

    public void SetCollectColor() =>
        _material.SetColor("_EmissionColor", _collectColor);

    public void Draw()
    {
        if (_agent == null)
            return;

        Vector3[] corners = _agent.path.corners;

        if (corners.Length > 0)
        {
            _lineRenderer.positionCount = corners.Length;
            _lineRenderer.SetPositions(corners);
        }
    }
}