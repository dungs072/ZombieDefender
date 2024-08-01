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
        TimeManager.SpawnTimeFinished += () =>
        {
            var ownerPlayer = ((CustomNetworkManager)NetworkManager.Singleton).OwnerPlayer;
            ResetStatePlayer(ownerPlayer);
        };

        if (!IsServer) return;
        InitPlayerRpc();
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
        currentIndex = (currentIndex + 1) % spawnPositions.Count;
        return spawnPositions[currentIndex].position;
    }
    public void ResetStatePlayer(PlayerController player)
    {
        if (!player.IsOwner) return;
        player.ResetPlayer();
        player.GetComponent<WeaponManager>().RemoveLastWeapon();
        player.GetComponent<WeaponManager>().SpawnRandomWeapon();
        SetUpPositionsPlayerServerRpc(player.NetworkObjectId);
    }
    [Rpc(SendTo.Server)]
    private void SetUpPositionsPlayerServerRpc(ulong playerId)
    {
        var players = ((CustomNetworkManager)NetworkManager.Singleton).Players;
        foreach (var player in players)
        {
            if (player.NetworkObjectId != playerId) continue;
            SetUpPlayerPositionRpc(GetSpawnPos());
        }
    }

    [Rpc(SendTo.Everyone)]
    private void SetUpPlayerPositionRpc(Vector3 position)
    {
        if (IsServer) return;
        var ownerPlayer = ((CustomNetworkManager)NetworkManager.Singleton).OwnerPlayer;
        ownerPlayer.transform.position = position;

    }
    [Rpc(SendTo.Everyone)]
    private void InitPlayerRpc()
    {
        var ownerPlayer = ((CustomNetworkManager)NetworkManager.Singleton).OwnerPlayer;
        ResetStatePlayer(ownerPlayer);
    }
}
