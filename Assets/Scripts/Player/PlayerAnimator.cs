using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private readonly int LocomotionValueHash = Animator.StringToHash("Locomotion");
    private readonly int GunLocomotionHash = Animator.StringToHash("GunLocomotion");
    private readonly int LocomotionHash = Animator.StringToHash("KnifeLocomotion");
    private readonly float CrossFadeInFixedTime = 0.1f;
    [SerializeField] private Animator animator;


    public void PlayLocomtionAnimation(bool isMoving)
    {
        animator.SetFloat(LocomotionValueHash, isMoving ? 1 : 0);
    }
    public void PlayAttackAnimation(string attackAnimName)
    {
        animator.CrossFadeInFixedTime(attackAnimName, CrossFadeInFixedTime);
    }

    public void SetCurrentLocomotion(string locomotionValue)
    {
        int currentLocomotionHash = Animator.StringToHash(locomotionValue);
        animator.CrossFadeInFixedTime(currentLocomotionHash, CrossFadeInFixedTime);
    }


}
