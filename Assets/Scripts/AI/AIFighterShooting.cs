using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFighterShooting : AIFighter
{
    [SerializeField] private Projectile projectile;
    [SerializeField] private Transform shootPos;
    [SerializeField] private float maxTimeToSpawn = 10f;
    private float currentTime = 0;

    public override void Attack()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            animator.PlayAttackAnimation();
            currentTime = maxTimeToSpawn;

        }
        // else
        // {
        //     animator.ToggleAttackAnimation(false);
        // }

    }
    public void SpawnProjectile()
    {
        if (!animator.IsServer) return;
        var projectileInstance = NetworkObjectPool.Singleton.
                                     GetNetworkObject(projectile.gameObject,
                                         shootPos.position, shootPos.rotation);
        projectileInstance.GetComponent<Projectile>().SetDamage(Damage);
        if (projectileInstance.IsSpawned)
        {
            Projectile projectile = projectileInstance.GetComponent<Projectile>();
            projectile.ToggleGameObjectClientRpc(true);
            projectile.SetPositionClientRpc(shootPos.position);
            projectile.SetRotationClientRpc(shootPos.rotation);
        }
        else
        {
            projectileInstance.Spawn(true);
        }

    }
}
