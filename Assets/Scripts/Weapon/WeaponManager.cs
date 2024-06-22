using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private List<Weapon> weapons;
    public Weapon CurrentWeapon { get; private set; }
    private void Start()
    {
        //CurrentWeapon = weapons[0];
        SetCurrentWeapon(weapons[0]);
    }
    private void SetCurrentWeapon(Weapon weapon)
    {
        if (CurrentWeapon != null)
        {
            CurrentWeapon.AttackingWeapon -= playerAnimator.PlayAttackAnimation;
        }
        CurrentWeapon = weapon;
        CurrentWeapon.AttackingWeapon += playerAnimator.PlayAttackAnimation;
    }

}
