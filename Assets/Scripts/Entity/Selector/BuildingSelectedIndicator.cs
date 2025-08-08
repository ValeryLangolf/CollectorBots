using UnityEngine;

public class BuildingSelectedIndicator : MonoBehaviour
{
    private Building _building;

    public Building Building => _building;

    private void Update()
    {
        if (_building == null)
        {
            gameObject.SetActive(false);
            return;
        }

        transform.position = _building.transform.position;
    }

    public void Follow(Building target)
    {
        _building = target;
        gameObject.SetActive(true);
    }

    public void StopFollow() =>
        _building = null;
}