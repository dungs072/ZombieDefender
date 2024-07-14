using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAnimator : NetworkBehaviour
{
    private readonly int LocomotionValueHash = Animator.StringToHash("Locomotion");
    private readonly int AttackHash = Animator.StringToHash("Attack");
    private readonly float CrossFadeInFixedTime = 0.1f;
    [SerializeField] private Animator animator;
    [SerializeField] private List<AnimatorOverrideNetwork> overrideNetworks;

    public void PlayLocomtionAnimation(bool isMoving)
    {
        animator.SetFloat(LocomotionValueHash, isMoving ? 1 : 0);
    }
    public void PlayAttackAnimation()
    {
        animator.CrossFadeInFixedTime(AttackHash, CrossFadeInFixedTime);
    }
    public void SetAnimatorOverride(int Id, AnimatorOverrideController animatorOverride)
    {
        if (animatorOverride != null)
        {
            animator.runtimeAnimatorController = animatorOverride;
            ChangeOverrideAnimatorClientRpc(Id);
        }
    }
    [Rpc(SendTo.Everyone)]
    private void ChangeOverrideAnimatorClientRpc(int Id)
    {
        foreach (var network in overrideNetworks)
        {
            if (network.Id == Id)
            {
                animator.runtimeAnimatorController = network.overrideController;
            }
        }
    }

}
[Serializable]
class AnimatorOverrideNetwork
{
    public int Id;
    public AnimatorOverrideController overrideController;
}