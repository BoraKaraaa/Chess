using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RigidbodyExtensionBase
{
    public static void ChangeDirection(this Rigidbody rb, Vector3 direction)
    {
        rb.velocity = direction.normalized * rb.velocity.magnitude;
    }
    
    public static void NormalizeVelocity(this Rigidbody rb, float magnitude = 1)
    {
        rb.velocity = rb.velocity.normalized * magnitude;
    }
}
