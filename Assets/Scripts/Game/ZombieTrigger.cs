using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieTrigger : MonoBehaviour
{
    public static event Action<ZombieTrigger> ZombieTriggered;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            ZombieTriggered?.Invoke(this);
            gameObject.SetActive(false);
        }
    }
}
