using Unity.Netcode;
using UnityEngine;
[RequireComponent(typeof(NetworkObject))]
public class Projectile : NetworkBehaviour
{
    [SerializeField] private Effect bloodHitEffect;
    [SerializeField] private ReferenceItself referenceItself;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 2f;
    private NetworkObjectPool pool;
    private int damage = 1;

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }
    private void Start()
    {
        pool = NetworkObjectPool.Singleton;
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
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void Deactivate()
    {

        pool.ReturnNetworkObject(GetComponent<NetworkObject>(), referenceItself.Prefab);
        ToggleGameObjectClientRpc(false);
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsServer) { return; }

        if (other.TryGetComponent(out Health health))
        {
            health.TakeDamage(damage);
            SpawnEffect();
        }


        Deactivate();
    }
    private void SpawnEffect()
    {
        var effectInstance = NetworkObjectPool.Singleton.
                                  GetNetworkObject(bloodHitEffect.gameObject,
                                      transform.position, transform.rotation);
        if (effectInstance.IsSpawned)
        {

            var effect = effectInstance.GetComponent<Effect>();
            effect.ToggleGameObjectClientRpc(true);
            effect.SetPositionClientRpc(transform.position);
            effect.SetRotationClientRpc(transform.rotation);
        }
        else
        {
            effectInstance.Spawn(true);
        }
    }
}
