using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBossFighter : AIFighter
{
    [SerializeField] private Projectile projectile;
    [SerializeField] private Transform shootPos;
    [SerializeField] private float maxTimeToSpawn = 10f;
    [SerializeField] private float meleeDistance = 3;

    private AIBossAnimator bossAnimator;
    private float currentTime = 0;
    private void Start()
    {
        bossAnimator = animator as AIBossAnimator;
    }

    public override void Attack(Transform target)
    {
        bossAnimator.ToggleWalkAnimation(false);
        if (IsInDistance(target.position, meleeDistance))
        {

            bossAnimator.ToggleAttackAnimation(true);

        }
        else
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0)
            {
                bossAnimator.ToggleAttackAnimation(false);
                PlayRandomShootAnimation();
                currentTime = maxTimeToSpawn;

            }
        }

    }
    public void SpawnProjectile()
    {
        if (!bossAnimator.IsServer) return;
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

    public void PlayRandomShootAnimation()
    {
        float randomValue = UnityEngine.Random.Range(0, 1f);
        Debug.Log(randomValue);
        if (randomValue < 0.3)
        {
            bossAnimator.PlayShootAnimation();
        }
        else if (randomValue > 0.7)
        {
            bossAnimator.PlayFlameAttackAnimation();
        }
        else
        {
            bossAnimator.PlayShootToAnimation();
        }
    }
    private bool IsInDistance(Vector3 targetPos, float distance)
    {
        if (transform == null) { return false; }
        return (targetPos - transform.position).sqrMagnitude <= distance * distance;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, meleeDistance);
    }
}
