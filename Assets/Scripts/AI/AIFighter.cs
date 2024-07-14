using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFighter : MonoBehaviour
{
    [field: SerializeField] public float FighterDistance { get; private set; } = 0.5f;
    [field: SerializeField] public int Damage { get; private set; } = 5;

    public void Attack()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, FighterDistance);
    }
}
