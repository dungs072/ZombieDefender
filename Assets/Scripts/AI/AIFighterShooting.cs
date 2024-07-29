using UnityEngine;

public class AIFighterShooting : AIFighter
{
    [SerializeField] private Projectile projectile;
    [SerializeField] private Transform shootPos;
    [SerializeField] private float maxTimeToSpawn = 10f;
    [SerializeField] private float meleeDistance = 3;
    private float currentTime = 0;

    public override void Attack(Transform target, AudioSource audioSource, ZombieSoundData zombieSoundData)
    {
        animator.ToggleWalkAnimation(false);
        if (IsInDistance(target.position, meleeDistance))
        {

            animator.ToggleAttackAnimation(true);

        }
        else
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0)
            {
                animator.ToggleAttackAnimation(false);
                animator.PlayShootAnimation();
                currentTime = maxTimeToSpawn;
                if (audioSource.isPlaying) return;
                audioSource.PlayOneShot(zombieSoundData.GetAttack());

            }
        }

    }
    public void SpawnProjectile()
    {
        if (!animator.IsServer) return;
        var projectileInstance = NetworkObjectPool.Singleton.
                                     GetNetworkObject(projectile.gameObject,
                                         shootPos.position, shootPos.rotation);
        projectileInstance.GetComponent<Projectile>().SetDamage(Damage);
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
    private bool IsInDistance(Vector3 targetPos, float distance)
    {
        if (transform == null) { return false; }
        return (targetPos - transform.position).sqrMagnitude <= distance * distance;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, meleeDistance);
    }
}
