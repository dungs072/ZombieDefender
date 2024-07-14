using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HandCollider : MonoBehaviour
{
    public static event Action<Collider2D> HandTriggered;
    private void OnTriggerEnter2D(Collider2D other)
    {
        HandTriggered?.Invoke(other);
    }
}
