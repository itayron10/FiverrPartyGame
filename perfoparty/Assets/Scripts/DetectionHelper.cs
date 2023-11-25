using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionHelper
{
    /// <summary>
    /// this method gets the closest transform in a list based on the distance from the currentPosition
    /// </summary>
    public static Transform GetClosest(List<Transform> transforms, Vector3 currentPosition)
    {
        if (transforms.Count < 1 || transforms == null) { return null; }

        float minDistance = float.MaxValue;
        Transform closestResource = null;

        foreach (Transform target in transforms)
        {
            if (target == null) { continue; }

            if (Vector3.Distance(target.transform.position, currentPosition) < minDistance)
            {
                minDistance = Vector3.Distance(target.transform.position, currentPosition);
                closestResource = target;
            }
        }

        return closestResource;
    }

}
