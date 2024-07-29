using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AIManager : NetworkBehaviour
{
    public event Action BossSpawned;
    private List<AIController> aIControllers;
    private List<AIController> bossControllers;
    private List<PlayerController> players;


    private bool isBossSpawned = false;

    public bool startZombie { get; set; } = false;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) { return; }
        aIControllers = new List<AIController>();
        bossControllers = new List<AIController>();
        CustomNetworkManager.PlayerAdded += RegisterPlayers;

    }

    private void RegisterPlayers()
    {
        players = ((CustomNetworkManager)NetworkManager.Singleton).Players;


    }
    private void Update()
    {
        if (!IsServer) { return; }
        if (players == null) { return; }
        if (!startZombie) { return; }
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

    public void AddNetworkAI(NetworkObject obj)
    {
        if (obj.TryGetComponent(out AIController aIController))
        {
            AddAI(aIController);
        }

    }
    public void AddNetworkBossAI(NetworkObject obj)
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
        int randomIndex = UnityEngine.Random.Range(0, players.Count);
        SetTargets(players[randomIndex].transform);

    }
    private void RemoveAI(AIController ai)
    {
        aIControllers.Remove(ai);
        if (aIControllers.Count == 0 && !isBossSpawned)
        {
            isBossSpawned = true;
            BossSpawned?.Invoke();
        }
    }
    public void ResetZombies()
    {
        for (int i = 0; i < aIControllers.Count; i++)
        {
            if (aIControllers[i].gameObject.activeSelf)
            {
                aIControllers[i].Die();
            }
        }
        for (int i = 0; i < bossControllers.Count; i++)
        {
            if (bossControllers[i].gameObject.activeSelf)
            {
                bossControllers[i].Die();
            }
        }

        aIControllers.Clear();
        bossControllers.Clear();
    }
}
