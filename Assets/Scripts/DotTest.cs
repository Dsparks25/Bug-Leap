using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DotTest
{
    private static LayerMask layerMask = LayerMask.GetMask("Default");

    public static bool Dot(this Transform transform, Transform other, Vector2 testDirection)
    {
        // Calculate the direction vector from 'transform' to 'other'
        Vector2 direction = other.position - transform.position;

        // Check if the dot product between the normalized direction and 'testDirection' is greater than 0.25f
        return Vector2.Dot(direction.normalized, testDirection) > 0.25f;
    }
}
