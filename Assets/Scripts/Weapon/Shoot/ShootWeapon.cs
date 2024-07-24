using System;
using System.Collections;
using Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class ShootWeapon : Weapon
{
    public static event Action ReloadingTimeStartChanged;
    public static event Action<float> ReloadingTimeChanged;
    [SerializeField] private float reloadingTime = 5f;
    [SerializeField] private AudioClip reloadingSound;

    [SerializeField] private Projectile projectile;
    [SerializeField] private Transform shootPos;

    [SerializeField] private int maxBullet;
    [SerializeField] private int maxBulletsInMag;

    [Header("Guns Type")]
    [SerializeField] private int numberBulletSpawned = 1;
    [SerializeField] private float leftRange = 200;
    [SerializeField] private float rightRange = 150;
    private bool isReloadingFinished = true;


    private int currentBulletInMag = 0;
    private void Start()
    {
        currentBulletInMag = maxBulletsInMag;
    }

    public override void UpdateDetailsUI()
    {
        ChangeDetailWeapon(currentBulletInMag, maxBullet);
    }

    protected override void Attack()
    {
        base.Attack();

        canAttack = false;
        if (IsOwner)
        {
            if (HandleBullets())
            {
                SpawnProjectileServerRpc();
                TriggerShake();
            }
            else
            {
                isPauseAttacking = true;
            }

        }

    }
    private bool HandleBullets()
    {
        currentBulletInMag = Mathf.Max(currentBulletInMag - 1, 0);
        ChangeDetailWeapon(currentBulletInMag, maxBullet);
        return currentBulletInMag > 0;
    }
    public void ReloadBullet()
    {
        if (!isReloadingFinished) return;
        if (currentBulletInMag == maxBulletsInMag) return;
        isReloadingFinished = false;
        isPauseAttacking = !isReloadingFinished;
        StartCoroutine(HandleReloadingMagazine());
    }
    private IEnumerator HandleReloadingMagazine()
    {
        audioSource.PlayOneShot(reloadingSound);
        ReloadingTimeStartChanged?.Invoke();
        float currentTime = reloadingTime;
        while (currentTime > 0)
        {
            ReloadingTimeChanged?.Invoke(currentTime / reloadingTime);
            currentTime -= Time.deltaTime;
            yield return null;
        }
        if (currentBulletInMag > 0)
        {
            int numberBulletNeed = maxBulletsInMag - currentBulletInMag;
            if (maxBullet - numberBulletNeed >= 0)
            {
                maxBullet -= numberBulletNeed;
                currentBulletInMag += numberBulletNeed;
            }
            else
            {
                currentBulletInMag += numberBulletNeed;
                maxBullet = 0;
            }

        }
        else
        {
            if (maxBullet - maxBulletsInMag >= 0)
            {
                maxBullet -= maxBulletsInMag;
                currentBulletInMag = maxBulletsInMag;
            }
            else
            {
                currentBulletInMag = maxBullet;
                maxBullet = 0;
            }

        }
        if (currentBulletInMag > 0)
        {
            isPauseAttacking = false;
        }

        ChangeDetailWeapon(currentBulletInMag, maxBullet);
        isReloadingFinished = true;
        isPauseAttacking = !isReloadingFinished;

    }
    private void SpawnProjectile()
    {
        for (int i = 0; i < numberBulletSpawned; i++)
        {
            Quaternion randomQuarternion = GetRandomDirection(shootPos.up);
            if (i == 0)
            {
                randomQuarternion = shootPos.rotation;
            }

            var projectileInstance = NetworkObjectPool.Singleton.
                                   GetNetworkObject(projectile.gameObject,
                                       shootPos.position, randomQuarternion);
            projectileInstance.GetComponent<Projectile>().SetDamage(damage);
            if (projectileInstance.IsSpawned)
            {
                Projectile projectile = projectileInstance.GetComponent<Projectile>();
                projectile.ToggleGameObjectClientRpc(true);
                projectile.SetPositionClientRpc(shootPos.position);
                projectile.SetRotationClientRpc(randomQuarternion);
            }
            else
            {
                projectileInstance.Spawn(true);
            }
        }


    }

    public void TriggerShake()
    {
        CameraController.Instance.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
    }

    Quaternion GetRandomDirection(Vector3 direction)
    {
        float randomAngle = UnityEngine.Random.Range(leftRange, rightRange);
        Quaternion rotation = Quaternion.AngleAxis(randomAngle, Vector3.forward);
        return rotation;
    }



    [Rpc(SendTo.Server)]
    private void SpawnProjectileServerRpc()
    {
        SpawnProjectile();
    }

    public void ReduceReloadingTime(float time)
    {
        reloadingTime = Mathf.Max(reloadingTime - time, 0);
    }

    public void SetShootPos(Transform shootPos)
    {
        this.shootPos = shootPos;
    }


    private void OnDrawGizmosSelected()
    {

    }


}
