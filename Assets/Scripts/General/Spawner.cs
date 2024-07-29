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
    [SerializeField] private int maxZombies;
    [SerializeField] private bool spawnOverTime = true;

    private List<GameObject> zombies = new List<GameObject>();

    public int MaxZombies { get { return maxZombies; } }

    private float currentTime;
    private void Start()
    {
        currentTime = 2;
    }
    public void UpdateSpawner()
    {
        if (!spawnOverTime) return;
        if (!IsServer) return;
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            SpawnObject(Vector3.zero);
            currentTime = maxTimeToSpawn;
        }
    }
    public AIController SpawnObject(Vector3 position)
    {
        if (zombies.Count == maxTimeToSpawn) return null;
        Vector3 randomPosition = GetRandomPosition();
        if (position != Vector3.zero)
        {
            randomPosition = position;
        }
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
        zombies.Add(instance.gameObject);
        return instance.GetComponent<AIController>();
    }
    private Vector3 GetRandomPosition()
    {
        var randomDirection = UnityEngine.Random.insideUnitSphere.normalized;
        var randomRadius = UnityEngine.Random.Range(0, radius);
        return transform.position + randomDirection * randomRadius;
    }
    public bool IsAllZombieDie()
    {
        foreach (var zombie in zombies)
        {
            if (zombie.gameObject.activeSelf)
            {
                return false;
            }
        }
        return true;
    }
    public void ResetSpawner()
    {
        zombies.Clear();
    }
    private void OnDrawGizmos()
    {
        // Handles.color = Color.red;
        // Handles.DrawWireDisc(transform.position, Vector3.forward, radius);
    }
}
