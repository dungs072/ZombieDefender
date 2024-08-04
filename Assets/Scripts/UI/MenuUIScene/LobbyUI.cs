using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private LobbyRoomHandler lobbyRoomHandler;
    [SerializeField] private RectTransform lobbyPanel;
    [SerializeField] private TMP_Text lobbyName;

    [SerializeField] List<PlayerItem> playerItems;
    [SerializeField] private Button playeReadyButton;
    [SerializeField] private TMP_Text playReadyButtonText;
    [SerializeField] private GameObject chooseMapButton;

    private void Start()
    {
        LobbyRoomHandler.LobbyPlayersUpdated += UpdateLobby;
        LobbyRoomHandler.ClientConnected += SetClientSide;
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
        StartCoroutine(UpdateLobbyAsync(players));

    }

    // fix theree 
    private IEnumerator UpdateLobbyAsync(Dictionary<ulong, bool> players)
    {
        int i = 0;
        var playerss = ((CustomNetworkManager)NetworkManager.Singleton).Players;
        foreach (var player in playerss)
        {
            var playerData = player.PlayerData;
            while (playerData == null)
            {
                yield return null;
            }

            playerItems[i].SetPlayerName(playerData.playerName);
            playerItems[i].ToggleCheckMark(playerData.isReady);
            i++;
        }

    }


    public void SetPlayReadyButtonText(string text)
    {
        if (playReadyButtonText == null) return;
        playReadyButtonText.text = text;
    }
    public void SetClientSide(bool isClient)
    {
        if (chooseMapButton == null) return;
        //playeReadyButton.onClick.RemoveAllListeners();
        if (isClient)
        {
            SetPlayReadyButtonText("Ready");
            chooseMapButton.SetActive(false);

            //playeReadyButton.onClick.AddListener(lobbyRoomHandler.SetReady);

        }
        else
        {
            SetPlayReadyButtonText("Play");
            chooseMapButton.SetActive(true);
            //playeReadyButton.onClick.AddListener(lobbyRoomHandler.OnGameStart);
        }
    }


}
