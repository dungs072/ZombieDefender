using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class PVPGameManager : NetworkBehaviour
{

    [SerializeField] private List<Transform> spawnPositions;
    private int currentIndex = 0;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        var ownerPlayer = ((CustomNetworkManager)NetworkManager.Singleton).OwnerPlayer;
        ownerPlayer.transform.position = GetSpawnPos();
        var players = ((CustomNetworkManager)NetworkManager.Singleton).Players;
        foreach (var player in players)
        {
            SetUpPlayerPositionRpc(GetSpawnPos());
        }

    }

    public Vector3 GetSpawnPos()
    {
        currentIndex++;
        return spawnPositions[currentIndex].position;
    }
    [Rpc(SendTo.Everyone)]
    private void SetUpPlayerPositionRpc(Vector3 position)
    {
        if (IsServer) return;
        var ownerPlayer = ((CustomNetworkManager)NetworkManager.Singleton).OwnerPlayer;
        ownerPlayer.transform.position = position;
    }
}
