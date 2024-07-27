using System;
using Unity.Netcode;
using UnityEngine;
public enum WeaponName
{
    Knife,
    Pistol,
    Riffle,
    GrenadeLauncher,
    ShotGun,
    Sniper
}
public class Weapon : NetworkBehaviour, IProduct
{
    public static event Action<int, int> DetailChanged;
    public event Action AttackingWeapon;
    [SerializeField] private WeaponName weaponName;
    [SerializeField] protected int damage;
    [SerializeField] protected float attackRate;
    [SerializeField] private int animatorOverrideId;
    [SerializeField] private AnimatorOverrideController overrideAnimator;
    [SerializeField] private Sprite weaponIcon;
    [SerializeField] private AudioClip attackSound;
    protected bool canAttack = true;
    protected bool isPauseAttacking = false;

    protected AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public AnimatorOverrideController GetAnimatorOverrideController()
    {
        return overrideAnimator;
    }
    public int GetAnimatorOverrideId()
    {
        return animatorOverrideId;
    }
    public GameObject GetGameObject()
    {
        return gameObject;
    }
    public Sprite GetWeaponIcon()
    {
        return weaponIcon;
    }
    public WeaponName GetWeaponName()
    {
        return weaponName;
    }

    protected void ChangeDetailWeapon(int detail1, int detail2)
    {
        DetailChanged?.Invoke(detail1, detail2);
    }

    public void Operation()
    {
        if (canAttack && !isPauseAttacking)
        {
            canAttack = false;
            Attack();
            if (attackSound)
            {
                audioSource.PlayOneShot(attackSound);
            }

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
    public virtual void UpdateDetailsUI()
    {

    }


}
