
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
    private static CustomNetworkManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void AddPlayer(PlayerController player)
    {
        if (player.IsOwner)
        {
            OwnerPlayer = player;
        }
        players.Add(player);
        PlayerAdded?.Invoke();
    }
    public void RemovePlayer(PlayerController player)
    {
        players.Remove(player);
    }
}
