using System.Collections.Generic;
using UnityEngine;

public static class VectorExtensionBase
{
    // All purpose methods //
    public static Vector2 GetClosestVector(this Vector2 vec2, List<Vector2> others)
    {
        if (others.Count == 0)
        {
            HMDebug.LogWarning("Vector list is empty");
        }

        float minDistance = Vector2.Distance(vec2, others[0]);
        Vector2 closestVector = others[0];

        foreach (var vector in others)
        {
            if (Vector2.Distance(vec2, vector) < minDistance)
            {
                minDistance = Vector2.Distance(vec2, vector);
                closestVector = vector;
            }
        }

        return closestVector;
    }

    // Vector 2 methods //
    public static void SetX(ref this Vector2 vec2, float x)
    {
        vec2 = Vector2.right * x + Vector2.up * vec2.y;
    }

    public static void SetY(ref this Vector2 vec2, float y)
    {
        vec2 = Vector2.right * vec2.x + Vector2.up * y;
    }

    // Vector 3 methods //
    public static void SetX(ref this Vector3 vec3, float x)
    {
        vec3 = Vector3.right * x + Vector3.up * vec3.y + Vector3.forward * vec3.z;
    }

    public static void SetY(ref this Vector3 vec3, float y)
    {
        vec3 = Vector3.right * vec3.x + Vector3.up * y + Vector3.forward * vec3.z;
    }

    public static void SetZ(ref this Vector3 vec3, float z)
    {
        vec3 = Vector3.right * vec3.x + Vector3.up * vec3.y + Vector3.forward * z;
    }
}