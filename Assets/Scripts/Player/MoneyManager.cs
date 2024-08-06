using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static event Action<string> MoneyChanged;
    [SerializeField] private int initialMoney = 10;
    private int currentMoney = 0;
    public int CurrentMoney { get { return currentMoney; } }
    public void InitMoney()
    {
        AddMoney(initialMoney);
    }
    public void AddMoney(int amount)
    {
        currentMoney += amount;
        MoneyChanged?.Invoke(currentMoney.ToString());
    }
    public void MinusMoney(int amount)
    {
        currentMoney -= amount;
        MoneyChanged?.Invoke(currentMoney.ToString());
    }
    public bool IsEnoughMoney(int amount)
    {
        return currentMoney >= amount;
    }
}
