using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private List<Weapon> weapons;
    public Weapon CurrentWeapon { get; private set; }

    private int currentWeaponIndex = 0;
    private void Start()
    {
        //CurrentWeapon = weapons[0];
        SetCurrentWeapon(weapons[currentWeaponIndex]);
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


    private void SetCurrentWeapon(Weapon weapon)
    {
        if (CurrentWeapon != null)
        {
            CurrentWeapon.AttackingWeapon -= playerAnimator.PlayAttackAnimation;
        }
        CurrentWeapon = weapon;
        CurrentWeapon.AttackingWeapon += playerAnimator.PlayAttackAnimation;
        playerAnimator.SetCurrentLocomotion(weapon.GetLocomotionAnimName());
    }

}
