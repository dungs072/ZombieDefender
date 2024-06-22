using System;
using UnityEngine;
public class Weapon : MonoBehaviour, IProduct
{
    public event Action<string> AttackingWeapon;
    [SerializeField] protected int damage;
    [SerializeField] protected float attackRate;
    [SerializeField] protected string attackAnimName;
    protected bool canAttack = true;

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
        AttackingWeapon?.Invoke(attackAnimName);
    }
    private void ResetAttack()
    {
        canAttack = true;
    }


}
