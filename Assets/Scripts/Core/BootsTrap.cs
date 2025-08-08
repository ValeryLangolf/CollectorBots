using UnityEngine;

public class BootsTrap : MonoBehaviour
{
    [SerializeField] private DiamondDispatcher _diamondDispatcher;
    [SerializeField] private BuildingDispatcher _buildingDispatcher;

    private void Awake()
    {
        _diamondDispatcher.Init(this);
        _buildingDispatcher.Init(_diamondDispatcher);

        _diamondDispatcher.Subscribe();
    }

    private void Start() =>
        _diamondDispatcher.StarSpawn();

    private void OnDestroy()
    {
        _diamondDispatcher.Unsubscribe();
        _buildingDispatcher.Unsubscribe();
    }
}