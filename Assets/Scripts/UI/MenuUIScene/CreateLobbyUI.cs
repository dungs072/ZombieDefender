using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CreateLobbyUI : MonoBehaviour
{
    [SerializeField] private LobbyUI lobbyUI;
    [SerializeField] private RectTransform createLobbyPanel;
    [SerializeField] private GameObject blackout;
    [SerializeField] private TMP_InputField lobbyNameInput;
    [SerializeField] private TMP_InputField maxPlayerInput;
    [SerializeField] private Toggle pvpToggle;
    [SerializeField] private Image mapImage;

    private int mapId = -1;

    public async void CreateLobby()
    {
        string lobbyName = lobbyNameInput.text == "" ? "No name" : lobbyNameInput.text.Trim();
        LobbyData lobbyData = new LobbyData()
        {
            Name = lobbyName,
            Difficulty = 0,
            MaxPlayers = int.Parse(maxPlayerInput.text.Trim()),
            Type = pvpToggle.isOn ? "PVP" : "PVE",
            MapId = mapId,
        };
        await MatchMaking.CreateLobbyWithAllocation(lobbyData, () =>
        {
            TogglePanel(false);
            lobbyUI.TogglePanel(true);
            lobbyUI.SetPlayerInfo(PlayerPrefs.GetString("Name"));
            lobbyUI.SetLobbyName(lobbyName);
        });
        NetworkManager.Singleton.StartHost();
    }
    public void TogglePanel(bool state)
    {
        blackout.SetActive(state);
        if (state)
        {
            createLobbyPanel.gameObject.SetActive(state);
            if (createLobbyPanel.localScale != Vector3.zero)
            {
                createLobbyPanel.localScale = Vector3.zero;
            }
            LeanTween.scale(createLobbyPanel, Vector3.one, 0.3f).setEase(LeanTweenType.easeInOutSine);
        }
        else
        {
            LeanTween.scale(createLobbyPanel, Vector3.zero, 0.3f).setEase(LeanTweenType.easeInOutSine).setOnComplete(() =>
            {
                createLobbyPanel.gameObject.SetActive(state);
            });
        }
    }

}
