using UnityEngine;

[System.Serializable]
public class UnitFactory
{
    [SerializeField] private Unit _prefab;
    [SerializeField] private Transform _parent;

    public Unit CreateUnit(Vector3 position)
    {
        Unit unit = Object.Instantiate(_prefab, position, Quaternion.identity, _parent);
        unit.Init();

        return unit;
    }
}