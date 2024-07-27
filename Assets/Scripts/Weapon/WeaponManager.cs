using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static event Action<Sprite> WeaponChanged;
    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private List<Weapon> weapons;
    [SerializeField] private Transform shootPos;
    [SerializeField] private Transform weaponPack;
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
    public void AddWeapon(Weapon weapon)
    {
        weapons.Add(weapon);
        currentWeaponIndex = weapons.Count - 1;
        if (weapon is ShootWeapon)
        {
            var shootWeapon = (ShootWeapon)weapon;
            shootWeapon.SetShootPos(shootPos);
        }
        SetCurrentWeapon(weapons[currentWeaponIndex]);

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
