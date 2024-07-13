using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AIController : NetworkBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private AIFighter fighter;
    [SerializeField] private AIMovement movement;
    [SerializeField] private AIAnimator animator;
    [SerializeField] private ReferenceItself referenceItself;

    [SerializeField] private float disappearTimeWhenDie = 5f;
    private Transform target;
    private NetworkObjectPool pool;


    private void Start()
    {
        pool = NetworkObjectPool.Singleton;
    }
    public override void OnNetworkSpawn()
    {
        health.CharacterDied += Die;
    }
    private void OnEnable()
    {
        animator.PlayIdleAnimation();
        movement.ToggleStop(false);
        movement.SetCanMove(true);
        GetComponent<Collider2D>().enabled = true;

    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void UpdateAI()
    {
        if (target == null) { return; }
        if (health.IsDead)
        {

            return;
        }
        if (IsNearTarget(fighter.FighterDistance))
        {
            AttackTarget();
        }
        else
        {
            MoveToTarget();
        }

    }
    public void MoveToTarget()
    {
        animator.ToggleAttackAnimation(false);
        animator.ToggleWalkAnimation(true);
        movement.MoveToTarget(target.position);
    }

    public void AttackTarget()
    {
        animator.ToggleWalkAnimation(false);
        animator.ToggleAttackAnimation(true);
        movement.ToggleStop(true);
        movement.RotateToTarget(target.position);
        fighter.Attack();
    }

    public void Die()
    {
        animator.PlayDeathAnimation();
        movement.ToggleStop(true);
        GetComponent<Collider2D>().enabled = false;
        Invoke(nameof(HandleDie), disappearTimeWhenDie);
    }
    private void HandleDie()
    {
        movement.SetCanMove(false);
        pool.ReturnNetworkObject(GetComponent<NetworkObject>(), referenceItself.Prefab);

    }
    private bool IsNearTarget(float distance)
    {
        if (transform == null) { return false; }
        if (target == null) { return false; }
        return (target.position - transform.position).sqrMagnitude <= distance * distance;
    }



}
