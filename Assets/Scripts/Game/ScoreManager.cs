using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : NetworkBehaviour
{
    [SerializeField] private ScoreBoardUI scoreBoard;
    private Dictionary<ulong, int> playerScores = new Dictionary<ulong, int>();
    private void Awake()
    {
        TimeManager.TimeOut += () =>
        {
            scoreBoard.TogglePVPGameOver(true);
            var sortedDict = playerScores.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            foreach (var pair in sortedDict)
            {
                scoreBoard.SpawnScorePlayerUI(pair.Key.ToString(), pair.Value.ToString());
            }
        };
    }
    public override void OnNetworkSpawn()
    {
        List<PlayerController> players = ((CustomNetworkManager)NetworkManager.Singleton).Players;
        foreach (var player in players)
        {
            playerScores[player.NetworkObjectId] = 0;
        }
        Achievement.ScoreChanged += AddScoreRpc;
    }
    [Rpc(SendTo.Server)]
    public void AddScoreRpc(ulong playerId, int score)
    {
        playerScores[playerId] += score;
        AddScoreClientRpc(playerId, score);
    }
    [Rpc(SendTo.ClientsAndHost)]
    public void AddScoreClientRpc(ulong playerId, int score)
    {
        if (IsServer) return;
        playerScores[playerId] += score;
    }

    public void ClickNextButton()
    {
        if (NetworkManager.Singleton.IsListening)
        {
            NetworkManager.Singleton.SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            NetworkManager.Singleton.Shutdown();
        }
        else
        {
            SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
        }

    }
}
