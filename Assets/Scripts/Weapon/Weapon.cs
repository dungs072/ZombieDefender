using System;
using Unity.Netcode;
using UnityEngine;
public class Weapon : NetworkBehaviour, IProduct
{
    public event Action AttackingWeapon;
    [SerializeField] protected int damage;
    [SerializeField] protected float attackRate;
    [SerializeField] private AnimatorOverrideController overrideAnimator;
    protected bool canAttack = true;

    public AnimatorOverrideController GetAnimatorOverrideController()
    {
        return overrideAnimator;
    }
    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void Operation()
    {
        if (canAttack)
        {
            canAttack = false;
            Attack();
            Invoke(nameof(ResetAttack), attackRate);
        }

    }
    protected virtual void Attack()
    {
        AttackingWeapon?.Invoke();
    }
    private void ResetAttack()
    {
        canAttack = true;
    }


}
