using UnityEngine;

public class Counter : MonoBehaviour
{
    [SerializeField] private DiamondCountIndicator _countIndicator;

    private int _count;

    public void Increase() =>
        _countIndicator.UpdateCount(++_count);
}