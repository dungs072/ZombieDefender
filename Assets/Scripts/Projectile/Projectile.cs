using System;
using Unity.Netcode;
using UnityEngine;
[RequireComponent(typeof(NetworkObject))]
public class Projectile : NetworkBehaviour
{
    [SerializeField] private Effect hitEffect;
    [SerializeField] private Effect damageHitEffect;

    [SerializeField] private ReferenceItself referenceItself;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 2f;
    [SerializeField] private bool isMakingDamage;
    [SerializeField] private TrailRenderer trail;
    private int damage = 1;

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer) { return; }
        Invoke("Deactivate", lifetime);
    }
    public override void OnNetworkDespawn()
    {
        if (!IsServer) { return; }
        CancelInvoke();
    }
    private void OnEnable()
    {
        if (!IsServer) { return; }
        Invoke("Deactivate", lifetime);
    }

    private void OnDisable()
    {
        if (!IsServer) { return; }
        CancelInvoke();
    }

    private void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }

    private void Deactivate()
    {

        NetworkObjectPool.Singleton.ReturnNetworkObject(GetComponent<NetworkObject>(), referenceItself.Prefab);
        gameObject.SetActive(false);
        ToggleGameObjectClientRpc(false);


        if (hitEffect != null)
        {
            SpawnEffect(hitEffect);
        }
        if (trail != null)
        {
            trail.Clear();
        }
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
    [Rpc(SendTo.ClientsAndHost)]
    public void SetAngleClientRpc(float angle)
    {
        if (IsServer) { return; }
        transform.Rotate(0, 0, angle);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsServer) { return; }

        if (other.TryGetComponent(out Health health))
        {
            if (isMakingDamage)
            {
                health.TakeDamage(damage);
                SpawnEffect(damageHitEffect, health.transform);
            }
        }

        Deactivate();
    }

    protected virtual void SpawnEffect(Effect effectPrefab, Transform target = null)
    {
        Transform newTransform = target == null ? transform : target;
        var effectInstance = NetworkObjectPool.Singleton.
                                  GetNetworkObject(effectPrefab.gameObject,
                                      newTransform.position, newTransform.rotation);
        if (effectInstance.IsSpawned)
        {

            if (effectInstance.TryGetComponent(out Effect effect))
            {

                effect.ToggleGameObjectClientRpc(true);
                effect.SetPositionClientRpc(newTransform.position);
                effect.SetRotationClientRpc(newTransform.rotation);
            }

        }
        else
        {
            effectInstance.Spawn(true);
        }
    }
}
