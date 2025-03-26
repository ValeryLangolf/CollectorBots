using System;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public event Action<GameObject> Detected;

    private void OnTriggerEnter(Collider other) =>
        Detected?.Invoke(other.gameObject);
}