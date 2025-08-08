using UnityEngine;

[System.Serializable]
public class DiamondFactory
{
    [SerializeField] private Diamond _prefab;
    [SerializeField] private Transform _parent;

    private Pool<Diamond> _pool;

    public void Init() =>
        _pool = new(_prefab, _parent);

    public Diamond Create(Vector3 position)
    {
        Diamond diamond = _pool.Get();
        diamond.transform.position = position;

        return diamond;
    }

    public void ReturnInPool(Diamond diamond) =>
        _pool.Return(diamond);
}