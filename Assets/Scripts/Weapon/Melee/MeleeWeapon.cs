using UnityEngine;
public class MeleeWeapon : Weapon
{
    [SerializeField] private Effect bloodHitEffect;

    public override void UpdateDetailsUI()
    {
        ChangeDetailWeapon(0, 0);
    }
    protected override void Attack()
    {
        base.Attack();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsServer) { return; }
        if (other.TryGetComponent(out Health health))
        {
            health.TakeDamage(NetworkObjectId, damage);
            SpawnEffect(health.transform.position);
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
