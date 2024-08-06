using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAnimator : NetworkBehaviour
{
    private readonly int LocomotionValueHash = Animator.StringToHash("Locomotion");
    private readonly int AttackHash = Animator.StringToHash("Attack");
    private readonly int DeathHash = Animator.StringToHash("Death");
    private readonly int MainLocomotionHash = Animator.StringToHash("MainLocomotion");
    private readonly float CrossFadeInFixedTime = 0.1f;
    [SerializeField] private Animator animator;
    [SerializeField] private List<AnimatorOverrideNetwork> overrideNetworks;
    [SerializeField] private RuntimeAnimatorController female;
    [SerializeField] private RuntimeAnimatorController male;
    private int characterId = 1;
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        characterId = PlayerPrefs.GetInt("Id");
        if (characterId == 1)
        {
            animator.runtimeAnimatorController = female;
        }
        else if (characterId == 2)
        {
            animator.runtimeAnimatorController = male;
        }
    }

    public void PlayLocomtionAnimation(bool isMoving)
    {
        animator.SetFloat(LocomotionValueHash, isMoving ? 1 : 0);
    }
    public void PlayAttackAnimation()
    {
        animator.CrossFadeInFixedTime(AttackHash, CrossFadeInFixedTime);
    }
    public void PlayDeathAnimation()
    {
        animator.CrossFadeInFixedTime(DeathHash, CrossFadeInFixedTime);
    }
    public void PlayIdleAnimation()
    {
        animator.CrossFadeInFixedTime(MainLocomotionHash, CrossFadeInFixedTime);
    }
    public void SetAnimatorOverride(int Id, AnimatorOverrideController animatorOverride)
    {
        if (animatorOverride != null)
        {
            animator.runtimeAnimatorController = animatorOverride;
            ChangeOverrideAnimatorClientRpc(Id, characterId);
        }
    }
    [Rpc(SendTo.Everyone)]
    private void ChangeOverrideAnimatorClientRpc(int Id, int typeId)
    {
        foreach (var network in overrideNetworks)
        {
            if (network.Id == Id)
            {
                if (typeId == 1)
                {
                    animator.runtimeAnimatorController = network.femaleOverrideController;
                }
                else if (typeId == 2)
                {
                    animator.runtimeAnimatorController = network.maleOverrideController;
                }

            }
        }
    }

}
[Serializable]
class AnimatorOverrideNetwork
{
    public int Id;
    public AnimatorOverrideController maleOverrideController;
    public AnimatorOverrideController femaleOverrideController;
}