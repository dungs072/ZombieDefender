using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{

    [SerializeField] private LobbyRoomHandler lobbyRoomHandler;
    [SerializeField] private RectTransform lobbyPanel;
    [SerializeField] private TMP_Text lobbyName;

    [SerializeField] List<PlayerItem> playerItems;
    [SerializeField] private Button playButton;
    [SerializeField] private Button readyButton;
    [SerializeField] private TMP_Text playReadyButtonText;
    [SerializeField] private GameObject chooseMapButton;
    [Header("Map")]
    [SerializeField] private TMP_Text mapNameText;
    [SerializeField] private Image mapImage;
    [SerializeField] private MapData mapData;

    private void Start()
    {
        LobbyRoomHandler.LobbyPlayersUpdated += UpdateLobby;
        LobbyRoomHandler.ClientConnected += SetClientSide;
        LobbyRoomHandler.ExitingLobby += () => TogglePanel(false);
        CustomNetworkManager.PlayerRemoved += UpdateLobby;
        CustomNetworkManager.PlayerAdded += UpdateLobby;
    }
    private void OnDestroy()
    {
        CustomNetworkManager.PlayerRemoved -= UpdateLobby;
        CustomNetworkManager.PlayerAdded -= UpdateLobby;
    }
    private void UpdateLobby()
    {
        if (this == null) return;
        StartCoroutine(UpdateLobbyAsync(null));
    }
    private int currentIndex = 0;

    public void SetPlayerInfo(string playerName)
    {
        playerItems[currentIndex].SetPlayerName(playerName);
        playerItems[currentIndex].ToggleCheckMark(true);
        currentIndex++;

    }
    public void SetLobbyName(string lobbyName)
    {
        this.lobbyName.text = lobbyName;
    }
    public void TogglePanel(bool state)
    {
        Lobby lobby = MatchMaking.GetCurrentLobby();
        if (state)
        {
            int mapId = GetMapId(Constants.MapIdKey);
            var mapItemData = mapData.GetMapItemData(mapId);
            mapImage.sprite = mapItemData.mapIcon;
            mapNameText.text = mapItemData.mapName;
            lobbyPanel.gameObject.SetActive(state);
            if (lobbyPanel.localScale != Vector3.zero)
            {
                lobbyPanel.localScale = Vector3.zero;
            }
            LeanTween.scale(lobbyPanel, Vector3.one, 0.3f).setEase(LeanTweenType.easeInOutSine);

        }
        else
        {
            if (lobbyPanel == null) return;
            LeanTween.scale(lobbyPanel, Vector3.zero, 0.3f).setEase(LeanTweenType.easeInOutSine).setOnComplete(() =>
            {
                lobbyPanel.gameObject.SetActive(state);
            });
        }

        int GetMapId(string key)
        {
            return int.Parse(lobby.Data[key].Value);
        }
    }
    private void UpdateLobby(Dictionary<ulong, bool> players)
    {
        if (this == null) return;
        StartCoroutine(UpdateLobbyAsync(players));

    }


    // fix theree 
    private IEnumerator UpdateLobbyAsync(Dictionary<ulong, bool> players)
    {

        int i = 0;
        var playerss = ((CustomNetworkManager)NetworkManager.Singleton).Players;
        bool isAllReady = true;
        foreach (var player in playerss)
        {
            var playerData = player.PlayerData;
            while (playerData == null)
            {
                yield return null;
            }
            if (!playerData.isReady)
            {
                isAllReady = false;
            }

            playerItems[i].SetPlayerName(playerData.playerName);
            playerItems[i].ToggleCheckMark(playerData.isReady);
            i++;
        }
        for (; i < playerItems.Count; i++)
        {
            playerItems[i].SetPlayerName("Waiting player ...");
            playerItems[i].ToggleCheckMark(false);
        }
        playButton.interactable = isAllReady;
    }


    public void SetPlayReadyButtonText(string text)
    {
        if (playReadyButtonText == null) return;
        playReadyButtonText.text = text;
    }
    public void SetClientSide(bool isClient)
    {
        if (chooseMapButton == null) return;
        playButton.gameObject.SetActive(!isClient);
        readyButton.gameObject.SetActive(isClient);
        chooseMapButton.SetActive(!isClient);
    }


}
