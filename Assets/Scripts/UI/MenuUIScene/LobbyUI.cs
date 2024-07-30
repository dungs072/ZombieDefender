using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private RectTransform lobbyPanel;
    [SerializeField] private TMP_Text lobbyName;

    [SerializeField] List<PlayerItem> playerItems;

    private void Start()
    {
        LobbyRoomHandler.LobbyPlayersUpdated += UpdateLobby;
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
        if (state)
        {
            lobbyPanel.gameObject.SetActive(state);
            if (lobbyPanel.localScale != Vector3.zero)
            {
                lobbyPanel.localScale = Vector3.zero;
            }
            LeanTween.scale(lobbyPanel, Vector3.one, 0.3f).setEase(LeanTweenType.easeInOutSine);
        }
        else
        {
            LeanTween.scale(lobbyPanel, Vector3.zero, 0.3f).setEase(LeanTweenType.easeInOutSine).setOnComplete(() =>
            {
                lobbyPanel.gameObject.SetActive(state);
            });
        }
    }
    private void UpdateLobby(Dictionary<ulong, bool> players)
    {
        for (int i = 0; i < players.Count; i++)
        {
            playerItems[i].SetPlayerName("Players: " + i.ToString());
        }
    }



}
