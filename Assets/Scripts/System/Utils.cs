using UnityEngine;

public static class Utils
{
    public static Vector3 GetRandomDeviationXZ(float deviation) =>
        new(Random.Range(-deviation, deviation), 0, Random.Range(-deviation, deviation));

    public static bool IsMouseHit(Camera camera, out RaycastHit hit)
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        return Physics.Raycast(ray, out hit, Mathf.Infinity);
    }
}