using Unity.Netcode;
using UnityEngine;

public class PooledPrefabInstanceHandler : INetworkPrefabInstanceHandler
{
    private GameObject m_Prefab;
    private NetworkObjectPool m_Pool;
    public PooledPrefabInstanceHandler(GameObject prefab, NetworkObjectPool pool)
    {
        m_Prefab = prefab;
        m_Pool = pool;
    }
    public void Destroy(NetworkObject networkObject)
    {
        m_Pool.ReturnNetworkObject(networkObject, m_Prefab);
    }

    public NetworkObject Instantiate(ulong ownerClientId, Vector3 position, Quaternion rotation)
    {
        return m_Pool.GetNetworkObject(m_Prefab, position, rotation);
    }

}
