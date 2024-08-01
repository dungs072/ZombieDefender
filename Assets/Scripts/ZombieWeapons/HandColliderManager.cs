using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HandColliderManager : NetworkBehaviour
{
    [SerializeField] private Effect bloodHitEffect;
    [SerializeField] private AIFighter fighter;
    [SerializeField] private List<HandCollider> handColliders;
    private void Start()
    {
        HandCollider.HandTriggered += HandTriggerEnter;
    }
    public void HandTriggerEnter(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            player.GetComponent<Health>().TakeDamage(NetworkObjectId, fighter.Damage);
            if (player.IsServer)
            {
                SpawnEffect(player.transform.position);
            }

        }
    }
    public void ToggleHandColliders(bool state)
    {
        foreach (HandCollider collider in handColliders)
        {
            collider.gameObject.SetActive(state);
        }
    }
    private void SpawnEffect(Vector3 spawnPos)
    {
        var effectInstance = NetworkObjectPool.Singleton.
                                  GetNetworkObject(bloodHitEffect.gameObject,
                                      spawnPos, transform.rotation);
        if (effectInstance.IsSpawned)
        {

            var effect = effectInstance.GetComponent<Effect>();
            effect.ToggleGameObjectClientRpc(true);
            effect.SetPositionClientRpc(spawnPos);
            effect.SetRotationClientRpc(transform.rotation);
        }
        else
        {
            effectInstance.Spawn(true);
        }
    }
}
