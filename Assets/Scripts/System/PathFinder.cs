using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathFinder
{
    public bool TryGetClosestDiamond(Vector3 position, IReadOnlyList<Diamond> diamonds, out Diamond closestDiamond)
    {
        closestDiamond = null;

        if (diamonds.Count == 0)
            return false;

        NavMeshPath path = new();
        float closestDistance = float.MaxValue;

        foreach (Diamond diamond in diamonds)
        {
            if (diamond == null)
                continue;

            if (NavMesh.CalculatePath(position, diamond.transform.position, NavMesh.AllAreas, path) &&
                path.status == NavMeshPathStatus.PathComplete)
            {
                float distance = CalculatePathLength(path);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestDiamond = diamond;
                }
            }
        }

        return closestDiamond != null;
    }

    private float CalculatePathLength(NavMeshPath path)
    {
        if (path.status != NavMeshPathStatus.PathComplete)
            return float.MaxValue;

        float totalLength = 0f;

        for (int i = 0; i < path.corners.Length - 1; i++)
            totalLength += Vector3.Distance(path.corners[i], path.corners[i + 1]);

        return totalLength;
    }
}