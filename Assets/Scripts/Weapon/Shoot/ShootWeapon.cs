using System;
using System.Collections;
using Cinemachine;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class ShootWeapon : Weapon
{
    public static event Action ReloadingTimeStartChanged;
    public static event Action<float> ReloadingTimeChanged;
    [SerializeField] private float reloadingTime = 5f;
    [SerializeField] private AudioClip reloadingSound;

    [SerializeField] private Projectile projectile;
    [SerializeField] private Transform shootPos;

    [SerializeField] private int maxBullets;
    [SerializeField] private int maxBulletsInMag;
    [SerializeField] private bool autoReload;
    [Header("Guns Type")]
    [SerializeField] private int numberBulletSpawned = 1;
    [SerializeField] private float spread = 10;
    [SerializeField] private bool reloadPerBullet = false;
    //[SerializeField] private bool isTap = false;
    private bool isReloadingFinished = true;

    private int bulletLeft = 0;

    private int currentBulletInMag = 0;
    private void Start()
    {
        bulletLeft = maxBullets;
        currentBulletInMag = maxBulletsInMag;
    }

    public override void UpdateDetailsUI()
    {
        ChangeDetailWeapon(currentBulletInMag, bulletLeft);
    }

    protected override void Attack()
    {
        base.Attack();

        canAttack = false;
        if (IsOwner)
        {
            if (!HandleBullets())
            {
                isPauseAttacking = true;
            }

        }

    }
    private bool HandleBullets()
    {
        if (currentBulletInMag == 0) return false;
        currentBulletInMag = Mathf.Max(currentBulletInMag - 1, 0);
        ChangeDetailWeapon(currentBulletInMag, bulletLeft);
        if (currentBulletInMag == 0 && autoReload)
        {
            ReloadBullet();
        }
        SpawnProjectileServerRpc();
        TriggerShake();
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
        if (reloadPerBullet)
        {
            if (currentBulletInMag < maxBulletsInMag)
            {
                if (bulletLeft > 0)
                {
                    bulletLeft--;
                    currentBulletInMag++;
                }
            }

        }
        else
        {
            if (currentBulletInMag > 0)
            {
                int numberBulletNeed = maxBulletsInMag - currentBulletInMag;
                if (bulletLeft - numberBulletNeed >= 0)
                {
                    bulletLeft -= numberBulletNeed;
                    currentBulletInMag += numberBulletNeed;
                }
                else
                {
                    currentBulletInMag += numberBulletNeed;
                    bulletLeft = 0;
                }

            }
            else
            {
                if (bulletLeft - maxBulletsInMag >= 0)
                {
                    bulletLeft -= maxBulletsInMag;
                    currentBulletInMag = maxBulletsInMag;
                }
                else
                {
                    currentBulletInMag = bulletLeft;
                    bulletLeft = 0;
                }

            }
            if (currentBulletInMag > 0)
            {
                isPauseAttacking = false;
            }
        }


        ChangeDetailWeapon(currentBulletInMag, bulletLeft);
        isReloadingFinished = true;
        isPauseAttacking = !isReloadingFinished;

    }
    private void SpawnProjectile()
    {

        for (int i = 0; i < numberBulletSpawned; i++)
        {
            float currentSpread = 0;
            if (i > 0)
            {
                currentSpread = UnityEngine.Random.Range(-spread, spread);
            }
            var projectileInstance = NetworkObjectPool.Singleton.
                                   GetNetworkObject(projectile.gameObject,
                                       shootPos.position, shootPos.rotation);
            projectileInstance.GetComponent<Projectile>().SetDamage(damage);

            projectileInstance.transform.Rotate(0, 0, currentSpread);
            if (projectileInstance.IsSpawned)
            {
                Projectile projectile = projectileInstance.GetComponent<Projectile>();
                projectile.SetPositionClientRpc(shootPos.position);
                projectile.SetRotationClientRpc(shootPos.rotation);
                projectile.SetAngleClientRpc(currentSpread);
                projectile.SetDamageRpc(damage);
                projectile.ToggleGameObjectClientRpc(true);

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


}
