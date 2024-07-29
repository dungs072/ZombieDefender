using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class AIController : NetworkBehaviour
{
    public static event Action<AIController> AIDead;
    [SerializeField] private HandColliderManager handColliderManager;
    [SerializeField] private Health health;
    [SerializeField] private AIFighter fighter;
    [SerializeField] private AIMovement movement;
    [SerializeField] private AIAnimator animator;
    [SerializeField] private SpriteRenderer model;
    [SerializeField] private ReferenceItself referenceItself;
    [SerializeField] private bool isRandomSpeed = true;

    [SerializeField] private float disappearTimeWhenDie = 5f;

    [Header("Sounds")]
    [SerializeField] private ZombieSoundData zombieSoundData;
    private Transform target;
    private NetworkObjectPool pool;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }


    private void Start()
    {
        pool = NetworkObjectPool.Singleton;
    }
    public override void OnNetworkSpawn()
    {
        health.CharacterDied += Die;
        health.TakingDamage += PlayHitSounds;
        if (!IsServer) return;
        if (isRandomSpeed)
        {
            movement.AIPath.maxSpeed = UnityEngine.Random.Range(Const.minZombieSpeed, Const.maxZombieSpeed);
        }
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
        if (audioSource.isPlaying) return;
        audioSource.PlayOneShot(zombieSoundData.GetAlert());
    }

    public void AttackTarget()
    {
        movement.ToggleStop(true);
        movement.RotateToTarget(target.position);
        animator.ToggleWalkAnimation(false);
        fighter.Attack(target, audioSource, zombieSoundData);
    }

    public void Die()
    {
        AIDead?.Invoke(this);
        animator.PlayDeathAnimation();
        movement.ToggleStop(true);
        handColliderManager.ToggleHandColliders(false);
        ToggleColliderClientRpc(false);

        Invoke(nameof(HandleDie), disappearTimeWhenDie);
        audioSource.PlayOneShot(zombieSoundData.GetDeath());
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
    public void PlayHitSounds()
    {
        audioSource.PlayOneShot(zombieSoundData.GetBulletHit());
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
