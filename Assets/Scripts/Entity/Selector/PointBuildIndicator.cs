using System;
using UnityEngine;

public class PointBuildIndicator : MonoBehaviour 
{
    public event Action<PointBuildIndicator> Deactivated;

    public void Deactivate() =>
        Deactivated?.Invoke(this);
}