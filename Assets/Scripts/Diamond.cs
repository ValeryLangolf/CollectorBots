using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Diamond : MonoBehaviour 
{
    private Collider _collider;

    private void Awake() =>
        _collider = GetComponent<Collider>();

    public event Action<Diamond> Deactivated;

    public void Deactivate() =>
        Deactivated?.Invoke(this);

    public void EnableCollider() =>
        _collider.enabled = true;

    public void DisableCollider() =>
        _collider.enabled = false;
}