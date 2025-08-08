using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Detector : MonoBehaviour
{
    public event Action<Collider> Detected;

    private void OnTriggerEnter(Collider other) =>
        Detected?.Invoke(other);
}