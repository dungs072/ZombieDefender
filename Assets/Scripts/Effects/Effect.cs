using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Effect : NetworkBehaviour
{
    [SerializeField] private float lifetime = 4;
    [SerializeField] private ReferenceItself referenceItself;
    [SerializeField] private Animator animator;
    private NetworkObjectPool pool;
    private void Start()
    {
        pool = NetworkObjectPool.Singleton;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        lifetime = stateInfo.length;
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
}
