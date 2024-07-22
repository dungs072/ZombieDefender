using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievement : MonoBehaviour
{
    public static event Action<string> KillsChanged;
    private int currentKills = 0;
    public int CurrentMoney { get { return currentKills; } }

    private void Start()
    {
        AIController.AIDead += AddKillByOne;
        Invoke(nameof(InitKills), 2);

    }
    private void OnDestroy()
    {
        AIController.AIDead -= AddKillByOne;
    }
    private void AddKillByOne(AIController aIController)
    {
        AddKill(1);
    }
    private void InitKills()
    {
        AddKill(0);
    }
    public void AddKill(int amount)
    {
        currentKills += amount;
        KillsChanged?.Invoke(currentKills.ToString());
    }
    public void MinusKill(int amount)
    {
        currentKills -= amount;
        KillsChanged?.Invoke(currentKills.ToString());
    }
}
