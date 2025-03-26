using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    private readonly Diamond _prefab;
    private readonly Stack<Diamond> _diamonds = new Stack<Diamond>();
    private readonly Transform _parent;
    private readonly int _maxCount;

    public Pool(Diamond prefab, Transform parent, int maxCount)
    {
        _prefab = prefab;
        _parent = parent;
        _maxCount = maxCount;
    }

    public Diamond Get()
    {
        if (_diamonds.Count >= _maxCount)
            return null;

        Diamond diamond = _diamonds.Count > 0 ? _diamonds.Pop() : Create();
        diamond.gameObject.SetActive(true);
        diamond.Deactivated += Return;

        return diamond;
    }

    private void Return(Diamond diamond)
    {
        diamond.transform.SetParent(_parent);
        diamond.Deactivated -= Return;
        diamond.gameObject.SetActive(false);
        _diamonds.Push(diamond);
    }

    private Diamond Create() =>
        Object.Instantiate(_prefab, _parent);
}