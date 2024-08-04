using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WeaponManager : NetworkBehaviour
{
    public static event Action<Sprite> WeaponChanged;
    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private List<Weapon> weapons;
    [SerializeField] private Transform shootPos;
    [SerializeField] private Transform weaponPack;
    [SerializeField] private List<Weapon> weaponResources;
    public Weapon CurrentWeapon { get; private set; }


    private int currentWeaponIndex = 0;
    private void Start()
    {
        //CurrentWeapon = weapons[0];

        SetCurrentWeapon(weapons[currentWeaponIndex]);
    }
    public Transform GetWeaponPack()
    {
        return weaponPack;
    }
    public void ChangeUpWeapon()
    {
        currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
        SetCurrentWeapon(weapons[currentWeaponIndex]);

    }
    public void ChangeDownWeapon()
    {
        currentWeaponIndex = currentWeaponIndex - 1 >= 0 ? currentWeaponIndex - 1 : weapons.Count - 1;
        SetCurrentWeapon(weapons[currentWeaponIndex]);
    }
    public void AddWeapon(WeaponName weaponName)
    {
        var weapon = GetWeapon(weaponName);
        weapons.Add(weapon);
        currentWeaponIndex = weapons.Count - 1;
        if (weapon is ShootWeapon)
        {
            var shootWeapon = (ShootWeapon)weapon;
            shootWeapon.SetShootPos(shootPos);
            if (shootWeapon.TryGetComponent(out Laser laser))
            {
                laser.SetLaserFirePoint(shootPos);
            }
        }
        SetCurrentWeapon(weapons[currentWeaponIndex]);

    }
    private Weapon GetWeapon(WeaponName weaponName)
    {
        foreach (var weapon in weaponResources)
        {
            if (weapon.GetWeaponName() == weaponName)
            {
                return weapon;
            }
        }
        return null;
    }
    public void RemoveLastWeapon()
    {
        currentWeaponIndex = 0;
        SetCurrentWeapon(weapons[0]);
        if (weapons.Count == 1) return;
        weapons.RemoveAt(1);
    }
    public void SpawnRandomWeapon()
    {
        int weaponIndex = UnityEngine.Random.Range(0, weaponResources.Count);
        AddWeapon(weaponResources[weaponIndex].GetWeaponName());

    }
    private IEnumerator DelaySetUp(Weapon weaponInstance)
    {
        while (!weaponInstance.TryGetComponent(out NetworkObject networkObject))
        {
            Debug.Log(networkObject);
            yield return null;
        }
        weaponInstance.GetComponent<NetworkObject>().Spawn(true);
    }


    private void SetCurrentWeapon(Weapon weapon)
    {
        if (CurrentWeapon != null)
        {
            CurrentWeapon.AttackingWeapon -= playerAnimator.PlayAttackAnimation;

        }
        CurrentWeapon = weapon;
        CurrentWeapon.AttackingWeapon += playerAnimator.PlayAttackAnimation;
        playerAnimator.SetAnimatorOverride(CurrentWeapon.GetAnimatorOverrideId(), CurrentWeapon.GetAnimatorOverrideController());
        WeaponChanged?.Invoke(CurrentWeapon.GetWeaponIcon());
        weapon.UpdateDetailsUI();
    }

    public void ReloadWeapon()
    {
        if (CurrentWeapon is ShootWeapon)
        {
            var shootWeapon = CurrentWeapon as ShootWeapon;
            shootWeapon.ReloadBullet();
        }
    }
    public void AddReduceReloadingTime(float time)
    {
        foreach (var weapon in weapons)
        {
            if (weapon is ShootWeapon)
            {
                var shootWepon = weapon as ShootWeapon;
                shootWepon.ReduceReloadingTime(time);
            }
        }
    }
    public void HandleAim(bool canAim)
    {
        if (CurrentWeapon.TryGetComponent(out Scope scope))
        {
            scope.AimToTarget(canAim);
            if (CurrentWeapon.TryGetComponent(out Laser laser))
            {
                laser.UpdateLaser(canAim);
            }

        }
    }


}
