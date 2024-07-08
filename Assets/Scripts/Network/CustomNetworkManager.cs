
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CustomNetworkManager : NetworkManager
{
    private List<PlayerController> players = new List<PlayerController>();
    public PlayerController OwnerPlayer { get; private set; }
    public void AddPlayer(PlayerController player)
    {
        if (player.IsOwner)
        {
            OwnerPlayer = player;
        }
        players.Add(player);
    }
    public void RemovePlayer(PlayerController player)
    {
        players.Remove(player);
    }
}
