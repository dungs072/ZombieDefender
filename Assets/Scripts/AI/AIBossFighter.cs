using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBossFighter : AIFighter
{
    [SerializeField] private Transform shootPos;
    [SerializeField] private float maxTimeToSpawn = 10f;
    [SerializeField] private float meleeDistance = 3;
    [Header("Nail Fighter")]
    [SerializeField] private Projectile leftNailPrefab;
    [SerializeField] private Projectile rightNailPrefab;
    [SerializeField] private Transform leftNailShootPos;
    [SerializeField] private Transform rightNailShootPos;
    [Header("Arm Fighter")]
    [SerializeField] private Projectile armPrefab;
    [SerializeField] private Transform armShootPos;
    [Header("Sounds")]
    [SerializeField] private BossZombieSoundData bossZombieSoundData;

    private AIBossAnimator bossAnimator;
    private float currentTime = 0;
    private void Start()
    {
        bossAnimator = animator as AIBossAnimator;
    }

    public override void Attack(Transform target, AudioSource audioSource, ZombieSoundData zombieSoundData)
    {
        bossAnimator.ToggleWalkAnimation(false);
        if (IsInDistance(target.position, meleeDistance))
        {

            bossAnimator.ToggleAttackAnimation(true);
            if (audioSource.isPlaying) return;
            audioSource.PlayOneShot(zombieSoundData.GetAttack());

        }
        else
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0)
            {
                bossAnimator.ToggleAttackAnimation(false);
                PlayRandomShootAnimation(audioSource, zombieSoundData);
                currentTime = maxTimeToSpawn;


            }
        }

    }
    public void SpawnProjectile(Projectile projectilePrefab, Transform shootTransform)
    {
        if (!bossAnimator.IsServer) return;
        var projectileInstance = NetworkObjectPool.Singleton.
                                     GetNetworkObject(projectilePrefab.gameObject,
                                         shootTransform.position, shootTransform.rotation);
        projectileInstance.GetComponent<Projectile>().SetDamage(Damage);
        if (projectileInstance.IsSpawned)
        {
            Projectile projectile = projectileInstance.GetComponent<Projectile>();
            projectile.ToggleGameObjectClientRpc(true);
            projectile.SetPositionClientRpc(shootTransform.position);
            projectile.SetRotationClientRpc(shootTransform.rotation);
        }
        else
        {
            projectileInstance.Spawn(true);
        }

    }
    public void SpawnNailProjectiles()
    {
        SpawnProjectile(leftNailPrefab, leftNailShootPos);
        SpawnProjectile(rightNailPrefab, rightNailShootPos);
    }
    public void SpawnArmProjectile()
    {
        SpawnProjectile(armPrefab, armShootPos);
    }

    public void PlayRandomShootAnimation(AudioSource audioSource, ZombieSoundData zombieSoundData)
    {

        float randomValue = UnityEngine.Random.Range(0, 1f);
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
        if (audioSource.isPlaying) return;
        audioSource.PlayOneShot(zombieSoundData.GetThrow());
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
