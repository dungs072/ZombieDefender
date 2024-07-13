using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private readonly int LocomotionValueHash = Animator.StringToHash("Locomotion");
    private readonly int AttackHash = Animator.StringToHash("Attack");
    private readonly float CrossFadeInFixedTime = 0.1f;
    [SerializeField] private Animator animator;


    public void PlayLocomtionAnimation(bool isMoving)
    {
        animator.SetFloat(LocomotionValueHash, isMoving ? 1 : 0);
    }
    public void PlayAttackAnimation()
    {
        animator.CrossFadeInFixedTime(AttackHash, CrossFadeInFixedTime);
    }
    public void SetAnimatorOverride(AnimatorOverrideController animatorOverride)
    {
        if (animatorOverride != null)
        {
            animator.runtimeAnimatorController = animatorOverride;
        }
    }




}
