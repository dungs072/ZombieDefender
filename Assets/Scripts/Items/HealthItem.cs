using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : Item
{
    [SerializeField] private int healthAmount = 50;
    public override void PickUpItem(PlayerController player)
    {
        player.GetComponent<Health>().AddCurrentHealth(healthAmount);
        base.PickUpItem(player);
    }
}
