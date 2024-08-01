using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Achievement : NetworkBehaviour
{
    public static event Action<ulong, int> ScoreChanged;
    public static event Action<string> KillsChanged;
    private int currentKills = 0;
    public int CurrentMoney { get { return currentKills; } }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        Health.CharacterKilled += HandleKillEnemy;
    }
    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;
        Health.CharacterKilled -= HandleKillEnemy;
    }
    public void HandleKillEnemy(ulong playerId)
    {
        SendKillScoreToServerRpc(playerId, 1);
    }
    [Rpc(SendTo.Server)]
    private void SendKillScoreToServerRpc(ulong playerId, int score)
    {
        var players = ((CustomNetworkManager)NetworkManager.Singleton).Players;
        foreach (var player in players)
        {
            if (player.NetworkObjectId == playerId)
            {

                SendKillScoreToSpecificClientRpc(playerId, score);
                ScoreChanged?.Invoke(playerId, score);

                break;
            }
        }

    }
    [Rpc(SendTo.Everyone)]
    private void SendKillScoreToSpecificClientRpc(ulong playerId, int score)
    {
        var players = ((CustomNetworkManager)NetworkManager.Singleton).Players;
        foreach (var player in players)
        {
            if (!player.IsOwner) continue;
            if (player.NetworkObjectId == playerId)
            {
                player.GetComponent<Achievement>().AddKill(score);

                break;
            }
        }
    }
    public void AddKill(int amount)
    {
        currentKills += amount;
        KillsChanged?.Invoke(currentKills.ToString());
    }
    public void MinusKill(int amount)
    {
        currentKills -= amount;
        KillsChanged?.Invoke(currentKills.ToString());
    }
}
