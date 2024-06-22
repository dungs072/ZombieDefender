using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private readonly int LocomotionHash = Animator.StringToHash("Locomotion");
    private readonly float CrossFadeInFixedTime = 0.1f;
    [SerializeField] private Animator animator;

    public void PlayLocomtionAnimation(bool isMoving)
    {
        animator.SetFloat(LocomotionHash, isMoving ? 1 : 0);
    }
    public void PlayAttackAnimation(string attackAnimName)
    {
        animator.CrossFadeInFixedTime(attackAnimName, CrossFadeInFixedTime);
    }


}
