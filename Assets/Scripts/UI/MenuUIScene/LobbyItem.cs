using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Unity.Services.Lobbies.Models;
using System;

public class LobbyItem : MonoBehaviour
{
    public event Action<Lobby> LobbyJoined;
    [SerializeField] private TMP_Text lobbyName;
    [SerializeField] private TMP_Text type;
    [SerializeField] private TMP_Text mapName;
    [SerializeField] private TMP_Text maxPlayers;

    public Lobby Lobby { get; set; }

    public void SetLobbyName(string tex)
    {
        lobbyName.text = tex;
    }
    public void SetType(string text)
    {
        type.text = text;
    }
    public void SetMapName(string text)
    {
        mapName.text = text;
    }
    public void SetMaxPlayers(string text)
    {
        maxPlayers.text = text;
    }
    public void JoinLobby()
    {
        LobbyJoined?.Invoke(Lobby);
    }

    public void Init(Lobby lobby)
    {
        lobbyName.text = lobby.Name;
        type.text = GetStringValue(Constants.GameTypeKey);


        maxPlayers.text = $"{lobby.Players.Count}/{lobby.MaxPlayers}";

        // int GetValue(string key)
        // {
        //     return int.Parse(lobby.Data[key].Value);
        // }
        string GetStringValue(string key)
        {
            return lobby.Data[key].Value;
        }
        Lobby = lobby;
    }
}
