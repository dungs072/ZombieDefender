using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Spawner : NetworkBehaviour
{
    public event Action<NetworkObject> AISpawned;
    [SerializeField] private List<GameObject> prefabs;
    [SerializeField] private float maxTimeToSpawn = 5f;
    [SerializeField] private float radius = 10f;

    private float currentTime;
    private void Start()
    {
        currentTime = maxTimeToSpawn;
    }
    private void Update()
    {
        if (!IsServer) return;
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            SpawnObject();
            currentTime = maxTimeToSpawn;
        }
    }
    private void SpawnObject()
    {
        Vector3 randomPosition = GetRandomPosition();
        randomPosition.z = 0;
        int randomIndex = UnityEngine.Random.Range(0, prefabs.Count);
        var instance = NetworkObjectPool.Singleton.
                                    GetNetworkObject(prefabs[randomIndex],
                                        randomPosition, transform.rotation);
        AISpawned?.Invoke(instance);
        if (instance.IsSpawned)
        {
            AIController aiController = instance.GetComponent<AIController>();
            aiController.ToggleGameObjectClientRpc(true);
            aiController.SetPositionClientRpc(randomPosition);
            aiController.SetRotationClientRpc(transform.rotation);
        }
        else
        {
            instance.Spawn(true);
        }
    }
    private Vector3 GetRandomPosition()
    {
        var randomDirection = UnityEngine.Random.insideUnitSphere.normalized;
        var randomRadius = UnityEngine.Random.Range(0, radius);
        return transform.position + randomDirection * randomRadius;
    }
    private void OnDrawGizmos()
    {
        // Handles.color = Color.red;
        // Handles.DrawWireDisc(transform.position, Vector3.forward, radius);
    }
}
