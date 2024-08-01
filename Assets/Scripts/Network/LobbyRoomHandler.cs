using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyRoomHandler : NetworkBehaviour
{
    private readonly Dictionary<ulong, bool> _playersInLobby = new();
    public static event Action<Dictionary<ulong, bool>> LobbyPlayersUpdated;
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
        foreach (var player in _playersInLobby) UpdatePlayerClientRpc(player.Key, player.Value);
    }

    [ClientRpc]
    private void UpdatePlayerClientRpc(ulong clientId, bool isReady)
    {
        if (IsServer) return;

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
        else
        {
            // This happens when the host disconnects the lobby
            // _roomScreen.gameObject.SetActive(false);
            // _mainLobbyScreen.gameObject.SetActive(true);
            OnLobbyLeft();
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
        SceneController.Instance.StartMyServer(false, GetStringValue(Constants.MapNameKey));

        string GetStringValue(string key)
        {
            return lobby.Data[key].Value;
        }
    }
}