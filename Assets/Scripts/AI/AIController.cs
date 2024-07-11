using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AIController : NetworkBehaviour
{
    [SerializeField] private AIMovement movement;
    [SerializeField] private ReferenceItself referenceItself;
    private Transform target;
    private NetworkObjectPool pool;

    private void Start()
    {
        pool = NetworkObjectPool.Singleton;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
    public void MoveToTarget()
    {
        if (target == null) { return; }
        movement.MoveToTarget(target.position);
    }

    public void Die()
    {
        pool.ReturnNetworkObject(GetComponent<NetworkObject>(), referenceItself.Prefab);
    }
}
