using Unity.Netcode;
using UnityEngine;

public class ShootWeapon : Weapon
{
    [SerializeField] private Projectile projectile;
    [SerializeField] private Transform shootPos;

    protected override void Attack()
    {
        base.Attack();
        canAttack = false;
        if (IsServer)
        {
            SpawnProjectile();
        }
        else
        {
            SpawnProjectileServerRpc();
        }


    }
    private void SpawnProjectile()
    {
        var projectileInstance = NetworkObjectPool.Singleton.
                                     GetNetworkObject(projectile.gameObject,
                                         shootPos.position, shootPos.rotation);
        if (projectileInstance.IsSpawned)
        {
            Projectile projectile = projectileInstance.GetComponent<Projectile>();
            projectile.ToggleGameObjectClientRpc(true);
            projectile.SetPositionClientRpc(shootPos.position);
            projectile.SetRotationClientRpc(shootPos.rotation);
        }
        else
        {
            projectileInstance.Spawn(true);
        }

    }
    [Rpc(SendTo.Server)]
    private void SpawnProjectileServerRpc()
    {
        SpawnProjectile();
    }



}
