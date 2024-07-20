using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AIAnimator : NetworkBehaviour
{
    private readonly int IdleHash = Animator.StringToHash("Idle");
    private readonly int CanWalkHash = Animator.StringToHash("canMove");
    private readonly int CanAttackHash = Animator.StringToHash("canAttack");
    private readonly int DeathHash = Animator.StringToHash("Death");
    private readonly int AttackHash = Animator.StringToHash("Shoot");
    protected readonly float CrossFadeInFixedTime = 0.1f;
    [SerializeField] protected Animator animator;

    public void PlayIdleAnimation()
    {
        animator.CrossFadeInFixedTime(IdleHash, CrossFadeInFixedTime);
    }
    public void PlayShootAnimation()
    {
        animator.CrossFadeInFixedTime(AttackHash, CrossFadeInFixedTime);
    }
    public void PlayDeathAnimation()
    {
        animator.CrossFadeInFixedTime(DeathHash, CrossFadeInFixedTime);
    }

    public void ToggleWalkAnimation(bool state)
    {
        animator.SetBool(CanWalkHash, state);
    }
    public void ToggleAttackAnimation(bool state)
    {
        animator.SetBool(CanAttackHash, state);
    }

}
