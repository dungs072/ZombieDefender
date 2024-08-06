using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyRoomHandler : NetworkBehaviour
{
    [SerializeField] private MapData mapData;
    public static event Action ExitingLobby;
    private readonly Dictionary<ulong, bool> _playersInLobby = new();
    public static event Action<Dictionary<ulong, bool>> LobbyPlayersUpdated;
    public static event Action<bool> ClientConnected;
    private bool isReady = false;
    public override void OnNetworkSpawn()
    {

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;

            _playersInLobby.Add(NetworkManager.Singleton.LocalClientId, false);
            UpdateInterface();
        }

        // Client uses this in case host destroys the lobby
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;
        PlayerController.PlayerDataChanged += UpdateInterface;

    }
    public void SetReady()
    {
        isReady = !isReady;
        var ownerPlayer = ((CustomNetworkManager)NetworkManager.Singleton).OwnerPlayer;
        ownerPlayer.SetReady(isReady);

    }

    private void OnClientConnectedCallback(ulong playerId)
    {
        if (!IsServer) return;

        // Add locally
        if (!_playersInLobby.ContainsKey(playerId)) _playersInLobby.Add(playerId, true);

        PropagateToClients();

        UpdateInterface();
    }

    private void PropagateToClients()
    {
        if (MatchMaking.GetCurrentLobby() == null) return;
        if (_playersInLobby == null) return;
        if (_playersInLobby.Count == 1) return;
        foreach (var player in _playersInLobby)
        {
            UpdatePlayerClientRpc(player.Key, player.Value);
        }

    }

    [Rpc(SendTo.Everyone)]
    private void UpdatePlayerClientRpc(ulong clientId, bool isReady)
    {

        if (IsServer) return;
        if (_playersInLobby == null) return;
        if (!_playersInLobby.ContainsKey(clientId)) _playersInLobby.Add(clientId, isReady);
        else _playersInLobby[clientId] = isReady;
        UpdateInterface();
    }

    private void OnClientDisconnectCallback(ulong playerId)
    {
        if (IsServer)
        {
            // Handle locally
            if (_playersInLobby.ContainsKey(playerId)) _playersInLobby.Remove(playerId);

            // Propagate all clients
            RemovePlayerClientRpc(playerId);

            UpdateInterface();


        }
        if (OwnerClientId == playerId)
        {
            OnLobbyLeft();
            ExitingLobby?.Invoke();
        }


    }

    [ClientRpc]
    private void RemovePlayerClientRpc(ulong clientId)
    {
        if (IsServer) return;

        if (_playersInLobby.ContainsKey(clientId)) _playersInLobby.Remove(clientId);
        UpdateInterface();
    }

    public void OnReadyClicked()
    {
        SetReadyServerRpc(NetworkManager.Singleton.LocalClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetReadyServerRpc(ulong playerId)
    {
        _playersInLobby[playerId] = true;
        PropagateToClients();
        UpdateInterface();
    }

    private void UpdateInterface()
    {
        LobbyPlayersUpdated?.Invoke(_playersInLobby);
        ClientConnected?.Invoke(IsClient && !IsHost);
    }

    private async void OnLobbyLeft()
    {
        _playersInLobby.Clear();
        NetworkManager.Singleton.Shutdown();
        await MatchMaking.LeaveLobby();
    }

    public override void OnDestroy()
    {

        base.OnDestroy();
        // CreateLobbyScreen.LobbyCreated -= CreateLobby;
        // LobbyRoomPanel.LobbySelected -= OnLobbySelected;
        // RoomScreen.LobbyLeft -= OnLobbyLeft;
        // RoomScreen.StartPressed -= OnGameStart;

        // We only care about this during lobby
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnectCallback;
        }

    }

    public async void OnGameStart()
    {
        await MatchMaking.LockLobby();
        Lobby lobby = MatchMaking.GetCurrentLobby();
        int mapId = GetMapId(Constants.MapIdKey);
        string typeGame = GetType(Constants.GameTypeKey);
        MapItemData mapItemData = mapData.GetMapItemData(mapId);
        StartCoroutine(SceneController.Instance.StartMyServer(false, mapItemData.sceneName + typeGame));

        int GetMapId(string key)
        {
            return int.Parse(lobby.Data[key].Value);
        }
        string GetType(string key)
        {
            return lobby.Data[key].Value;
        }
    }
    public void OnGameExit()
    {
        OnClientDisconnectCallback(OwnerClientId);
    }
}
