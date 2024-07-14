using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class AIManager : NetworkBehaviour
{
    [SerializeField] private List<Spawner> spawners;
    private List<AIController> aIControllers;
    private List<PlayerController> players;
    public override void OnNetworkSpawn()
    {
        if (!IsServer) { return; }
        aIControllers = new List<AIController>();
        CustomNetworkManager.PlayerAdded += RegisterPlayers;
        foreach (var spawner in spawners)
        {
            spawner.AISpawned += AddNetworkAI;
        }
    }

    private void RegisterPlayers()
    {
        players = ((CustomNetworkManager)NetworkManager.Singleton).Players;

    }
    private void Update()
    {
        if (!IsServer) { return; }
        if (players == null) { return; }
        foreach (var ai in aIControllers)
        {
            if (ai.isActiveAndEnabled)
            {
                ai.UpdateAI();
            }

        }

    }
    private void SetTargets(Transform target)
    {
        foreach (var controller in aIControllers)
        {
            controller.SetTarget(target);
        }
    }
    private void AddNetworkAI(NetworkObject obj)
    {
        if (obj.TryGetComponent(out AIController aIController))
        {
            AddAI(aIController);
        }

    }
    public void AddAI(AIController ai)
    {
        aIControllers.Add(ai);
        int randomIndex = Random.Range(0, players.Count);
        SetTargets(players[randomIndex].transform);
    }
    public void RemoveAI(AIController ai)
    {
        aIControllers.Remove(ai);
    }
}
