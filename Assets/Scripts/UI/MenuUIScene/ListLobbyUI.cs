using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class ListLobbyUI : MonoBehaviour
{
    [SerializeField] private LobbyUI lobbyUI;
    [SerializeField] private RectTransform listLobbyPanel;
    [SerializeField] private LobbyItem lobbyPrefab;

    [SerializeField] private Transform content;
    private List<LobbyItem> lobbyItems = new List<LobbyItem>();
    private void Start()
    {
        LobbyRoomHandler.LobbyPlayersUpdated += UpdateLobby;
    }
    public async void FindLobbies()
    {
        foreach (var item in lobbyItems)
        {
            Destroy(item.gameObject);

        }
        lobbyItems.Clear();
        var allLobbies = await MatchMaking.GatherLobbies();

        var lobbyIds = allLobbies.Where(l => l.HostId != Authentication.PlayerId).Select(l => l.Id);

        foreach (var lobby in allLobbies)
        {
            var panel = Instantiate(lobbyPrefab, content);
            panel.Init(lobby);
            panel.LobbyJoined += JoinLobby;
            lobbyItems.Add(panel);
        }
        TogglePanel(true);
    }
    public async void JoinLobby(Lobby lobby)
    {
        try
        {
            await MatchMaking.JoinLobbyWithAllocation(lobby.Id);

            NetworkManager.Singleton.StartClient();
            lobbyUI.TogglePanel(true);
            TogglePanel(false);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public void TogglePanel(bool state)
    {

        if (state)
        {
            listLobbyPanel.gameObject.SetActive(state);
            if (listLobbyPanel.localScale != Vector3.zero)
            {
                listLobbyPanel.localScale = Vector3.zero;
            }
            LeanTween.scale(listLobbyPanel, Vector3.one, 0.3f).setEase(LeanTweenType.easeInOutSine);
        }
        else
        {
            LeanTween.scale(listLobbyPanel, Vector3.zero, 0.3f).setEase(LeanTweenType.easeInOutSine).setOnComplete(() =>
            {
                listLobbyPanel.gameObject.SetActive(state);
            });
        }
    }
    private void UpdateLobby(Dictionary<ulong, bool> players)
    {

    }

}

