using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class AIManager : NetworkBehaviour
{
    [SerializeField] private List<Spawner> spawners;
    [SerializeField] private List<Spawner> bossSpawners;
    private List<AIController> aIControllers;
    private List<AIController> bossControllers;
    private List<PlayerController> players;

    private bool isBossSpawned = false;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) { return; }
        aIControllers = new List<AIController>();
        bossControllers = new List<AIController>();
        CustomNetworkManager.PlayerAdded += RegisterPlayers;
        foreach (var spawner in spawners)
        {
            spawner.AISpawned += AddNetworkAI;
        }
        foreach (var bossSpawner in bossSpawners)
        {
            bossSpawner.AISpawned += AddNetworkBossAI;
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
        foreach (var ai in bossControllers)
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
        foreach (var bossController in bossControllers)
        {
            bossController.SetTarget(target);
        }
    }

    private void AddNetworkAI(NetworkObject obj)
    {
        if (obj.TryGetComponent(out AIController aIController))
        {
            AddAI(aIController);
        }

    }
    private void AddNetworkBossAI(NetworkObject obj)
    {
        if (obj.TryGetComponent(out AIController aIController))
        {
            AddAI(aIController, true);
        }

    }
    public void AddAI(AIController ai, bool isBoss = false)
    {
        if (isBoss)
        {
            bossControllers.Add(ai);
        }
        else
        {
            AIController.AIDead += RemoveAI;
            aIControllers.Add(ai);
        }

        if (players == null)
        {
            RegisterPlayers();
        }
        int randomIndex = Random.Range(0, players.Count);
        SetTargets(players[randomIndex].transform);

    }
    private void RemoveAI(AIController ai)
    {
        aIControllers.Remove(ai);
        if (aIControllers.Count == 0 && !isBossSpawned)
        {
            isBossSpawned = true;
            foreach (var boss in bossSpawners)
            {
                boss.SpawnObject();
            }
        }
    }
}
