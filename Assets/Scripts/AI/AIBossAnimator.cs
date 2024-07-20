using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBossAnimator : AIAnimator
{
    private readonly int ShootToHash = Animator.StringToHash("Shoot2");
    private readonly int FlameAttackHash = Animator.StringToHash("FlameAttack");

    public void PlayShootToAnimation()
    {
        animator.CrossFadeInFixedTime(ShootToHash, CrossFadeInFixedTime);
    }
    public void PlayFlameAttackAnimation()
    {
        animator.CrossFadeInFixedTime(FlameAttackHash, CrossFadeInFixedTime);
    }

}
