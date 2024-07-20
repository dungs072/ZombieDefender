using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AIController : NetworkBehaviour
{
    [SerializeField] private HandColliderManager handColliderManager;
    [SerializeField] private Health health;
    [SerializeField] private AIFighter fighter;
    [SerializeField] private AIMovement movement;
    [SerializeField] private AIAnimator animator;
    [SerializeField] private SpriteRenderer model;
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
        if (IsServer)
        {
            ToggleColliderClientRpc(true);
            ToggleGameobjectClientRpc(true);
        }

        handColliderManager.ToggleHandColliders(true);

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
        movement.ToggleStop(true);
        movement.RotateToTarget(target.position);
        fighter.Attack(target);
    }

    public void Die()
    {
        animator.PlayDeathAnimation();
        movement.ToggleStop(true);
        handColliderManager.ToggleHandColliders(false);
        ToggleColliderClientRpc(false);

        Invoke(nameof(HandleDie), disappearTimeWhenDie);
    }
    private void HandleDie()
    {
        movement.SetCanMove(false);

        ToggleGameobjectClientRpc(false);

        Invoke(nameof(HandleReturnToPool), 1f);
    }
    private void HandleReturnToPool()
    {
        pool.ReturnNetworkObject(GetComponent<NetworkObject>(), referenceItself.Prefab);
    }
    private bool IsNearTarget(float distance)
    {
        if (transform == null) { return false; }
        if (target == null) { return false; }
        return (target.position - transform.position).sqrMagnitude <= distance * distance;
    }
    [Rpc(SendTo.Everyone)]
    private void ToggleGameobjectClientRpc(bool state)
    {
        gameObject.SetActive(state);
    }

    [Rpc(SendTo.Everyone)]
    private void ToggleColliderClientRpc(bool state)
    {
        GetComponent<Collider2D>().enabled = state;
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void ToggleGameObjectClientRpc(bool state)
    {
        if (IsServer) { return; }
        gameObject.SetActive(state);
    }
    [Rpc(SendTo.ClientsAndHost)]
    public void SetPositionClientRpc(Vector3 position)
    {
        if (IsServer) { return; }
        transform.position = position;
    }
    [Rpc(SendTo.ClientsAndHost)]
    public void SetRotationClientRpc(Quaternion rotation)
    {
        if (IsServer) { return; }
        transform.rotation = rotation;
    }


}
