
using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CustomNetworkManager : NetworkManager
{
    public static event Action PlayerAdded;
    private List<PlayerController> players = new List<PlayerController>();
    public PlayerController OwnerPlayer { get; private set; }
    public List<PlayerController> Players { get { return players; } }
    public void AddPlayer(PlayerController player)
    {
        if (player.IsOwner)
        {
            OwnerPlayer = player;
            Debug.Log(OwnerPlayer);
        }
        players.Add(player);
        PlayerAdded?.Invoke();
    }
    public void RemovePlayer(PlayerController player)
    {
        players.Remove(player);
    }
}
