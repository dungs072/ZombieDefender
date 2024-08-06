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
    [SerializeField] private TMP_Dropdown typeDropDown;
    [SerializeField] private Image mapImage;
    [SerializeField] private TMP_Text mapNameText;
    [SerializeField] private MapData mapData;

    private int mapId;
    private string typeGame = "PVP";

    private void Awake()
    {
        MapSelectionUI.MapChose += OnMapChose;
    }
    private void OnMapChose(int id)
    {
        MapItemData mapItem = mapData.GetMapItemData(id);
        if (mapItem == null) return;
        mapImage.sprite = mapItem.mapIcon;
        mapNameText.text = mapItem.mapName;
        mapId = id;
    }
    public void OnTypeGameChanged()
    {
        typeGame = typeDropDown.captionText.text;
    }

    public async void CreateLobby()
    {
        await Authentication.Login();
        string lobbyName = lobbyNameInput.text == "" ? "No name" : lobbyNameInput.text.Trim();
        string type = typeGame;
        LobbyData lobbyData = new LobbyData()
        {
            Name = lobbyName,
            Difficulty = 0,
            MaxPlayers = int.Parse(maxPlayerInput.text.Trim()),
            Type = type,
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
