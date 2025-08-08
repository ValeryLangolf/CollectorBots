using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : MonoBehaviour
{
    private readonly T _prefab;
    private readonly Transform _transform;
    private readonly Stack<T> _elements = new();

    public Pool(T prefab, Transform transform)
    {
        _prefab = prefab;
        _transform = transform;
    }

    public T Get()
    {
        T element = _elements.Count > 0 ? _elements.Pop() : Object.Instantiate(_prefab, _transform);
        element.gameObject.SetActive(true);

        return element;
    }

    public void Return(T element)
    {
        element.gameObject.SetActive(false);
        element.transform.SetParent(_transform);
        _elements.Push(element);
    }
}