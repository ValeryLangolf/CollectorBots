using UnityEngine;

[System.Serializable]
public class BuildingFactory
{
    [SerializeField] private Building _prefab;
    [SerializeField] private Transform _parent;

    public Building Create(Vector3 position) =>
        Object.Instantiate(_prefab, position, Quaternion.identity, _parent);
}