using System;
using System.Collections.Generic;
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

    private List<Effect> effects = new List<Effect>();
    private void Awake()
    {
        effects.Add(damageHitEffect);
        if (hitEffect)
        {
            effects.Add(hitEffect);
        }


    }
    private int damage = 1;

    public void SetDamage(int damage)
    {
        if (trail != null)
        {
            trail.Clear();
        }
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
        if (!IsOwner) return;
        NetworkObjectPool.Singleton.ReturnNetworkObject(GetComponent<NetworkObject>(), referenceItself.Prefab);
        gameObject.SetActive(false);
        ToggleGameObjectClientRpc(false);


        if (hitEffect != null)
        {
            var effect = effects[1];
            SpawnEffect(effect, transform.position, transform.rotation);
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
        if (trail != null)
        {
            trail.Clear();
        }
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
    [Rpc(SendTo.ClientsAndHost)]
    public void SetDamageRpc(int damage)
    {
        if (IsServer) { return; }
        SetDamage(damage);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.TryGetComponent(out Health health))
        {
            if (!health.IsOwner) return;
            if (isMakingDamage)
            {
                health.TakeDamage(damage);
                SpawnEffectServerRpc(0, health.transform.position, health.transform.rotation);
            }
        }

        Deactivate();
    }
    [Rpc(SendTo.Server)]
    private void SpawnEffectServerRpc(int prefabIndex, Vector3 position, Quaternion quaternion)
    {
        Effect effectPrefab = effects[prefabIndex];
        SpawnEffect(effectPrefab, position, quaternion);
    }

    protected virtual void SpawnEffect(Effect effectPrefab, Vector3 position, Quaternion quaternion)
    {
        var effectInstance = NetworkObjectPool.Singleton.
                                  GetNetworkObject(effectPrefab.gameObject,
                                      position, quaternion);
        if (effectInstance.IsSpawned)
        {

            if (effectInstance.TryGetComponent(out Effect effect))
            {

                effect.ToggleGameObjectClientRpc(true);
                effect.SetPositionClientRpc(position);
                effect.SetRotationClientRpc(quaternion);
            }

        }
        else
        {
            effectInstance.Spawn(true);
        }
    }
}
