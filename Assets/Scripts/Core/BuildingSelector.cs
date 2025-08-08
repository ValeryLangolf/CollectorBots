using UnityEngine;

public class BuildingSelector : MonoBehaviour
{
    [SerializeField] private PointBuildIndicator _prefab;
    [SerializeField] private BuildingSelectedIndicator _indicator;

    private Pool<PointBuildIndicator> _pool;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
        _pool = new(_prefab, transform);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            HandleLeftClick();

        if (Input.GetMouseButtonDown(1))
            HandleRightClick();
    }

    private void HandleLeftClick()
    {
        if (Utils.IsMouseHit(_camera, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out Building building))
                _indicator.Follow(building);
            else
                _indicator.StopFollow();
        }
    }

    private void HandleRightClick()
    {
        Building building = _indicator.Building;

        if (building == null)
            return;

        if (Utils.IsMouseHit(_camera, out RaycastHit hit) && building.EnoughUnitsToBuild)
        {
            PointBuildIndicator indicator = building.BuildIndicator;

            if (building.BuildIndicator == null)
            {
                indicator = _pool.Get();
                indicator.Deactivated += OnDeactivationBuildIndicator;
            }
            else
            {
                building.ResetArrivalPointBuild();
            }

                indicator.transform.position = hit.point;
            building.SetBuildIndicator(indicator);
        }
    }

    private void OnDeactivationBuildIndicator(PointBuildIndicator indicator)
    {
        indicator.Deactivated -= OnDeactivationBuildIndicator;
        _pool.Return(indicator);
    }
}