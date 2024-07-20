using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AIAnimator : NetworkBehaviour
{
    private readonly int IdleHash = Animator.StringToHash("Idle");
    private readonly int WalkHash = Animator.StringToHash("canMove");
    private readonly int CanAttackHash = Animator.StringToHash("canAttack");
    private readonly int DeathHash = Animator.StringToHash("Death");
    private readonly int AttackHash = Animator.StringToHash("Shoot");
    private readonly float CrossFadeInFixedTime = 0.1f;
    [SerializeField] private Animator animator;

    public void PlayIdleAnimation()
    {
        animator.CrossFadeInFixedTime(IdleHash, CrossFadeInFixedTime);
    }
    public void PlayAttackAnimation()
    {
        animator.CrossFadeInFixedTime(AttackHash, CrossFadeInFixedTime);
    }

    public void ToggleWalkAnimation(bool state)
    {
        animator.SetBool(WalkHash, state);
    }
    public void ToggleAttackAnimation(bool state)
    {
        animator.SetBool(CanAttackHash, state);
    }
    public void PlayDeathAnimation()
    {
        animator.CrossFadeInFixedTime(DeathHash, CrossFadeInFixedTime);
    }
}
