using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAnimator : MonoBehaviour
{
    private readonly int IdleHash = Animator.StringToHash("Idle");
    private readonly int WalkHash = Animator.StringToHash("canMove");
    private readonly int AttackHash = Animator.StringToHash("canAttack");
    private readonly int DeathHash = Animator.StringToHash("Death");
    private readonly float CrossFadeInFixedTime = 0.1f;
    [SerializeField] private Animator animator;

    public void PlayIdleAnimation()
    {
        animator.CrossFadeInFixedTime(IdleHash, CrossFadeInFixedTime);
    }

    public void ToggleWalkAnimation(bool state)
    {
        animator.SetBool(WalkHash, state);
    }
    public void ToggleAttackAnimation(bool state)
    {
        animator.SetBool(AttackHash, state);
    }
    public void PlayDeathAnimation()
    {
        animator.CrossFadeInFixedTime(DeathHash, CrossFadeInFixedTime);
    }
}
