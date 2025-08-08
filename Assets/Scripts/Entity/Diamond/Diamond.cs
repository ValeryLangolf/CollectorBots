using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Diamond : MonoBehaviour 
{
    private Collider _collider;

    public event Action<Diamond> Deactivated;
    public event Action<Diamond> Picked;

    private void Awake() =>
        _collider = GetComponent<Collider>();

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Building _))
        {
            Picked?.Invoke(this);
            Deactivated?.Invoke(this);
        }
    }

    public void Deactivate() =>
        Deactivated?.Invoke(this);

    public void EnableCollider() =>
        _collider.enabled = true;

    public void PickUp()
    {
        _collider.enabled = false;
        Picked?.Invoke(this);
    }
}