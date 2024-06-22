using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootWeapon : Weapon
{
    [SerializeField] private Projectile projectile;
    [SerializeField] private Transform shootPos;

    protected override void Attack()
    {
        base.Attack();
        canAttack = false;
        GameObject projectile = ProjectileManager.Instance.GetObject();
        if (projectile != null)
        {
            projectile.transform.position = shootPos.position;
            projectile.transform.rotation = shootPos.rotation;
        }
    }

}
