using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TimeManager : NetworkBehaviour
{
    public static event Action TimeOut;
    public static event Action SpawnTimeFinished;
    public static event Action<string> CountingDownSpawnTime;
    [Header("Time in whole game")]
    [SerializeField] private int maxMinute = 5;
    [Range(0, 60)]
    [SerializeField] private int maxSecond = 15;
    [SerializeField] private TimeManagerUI timeManagerUI;
    [Header("Time to spawn")]
    [SerializeField] private float maxTimeSpawn = 5f;
    private int currentMinute = 0;
    private int currentSecond = 0;
    private float currentTimeSpawn = 0;

    public override void OnNetworkSpawn()
    {


        InGameUI.GameOverPVPUIOn += () =>
        {
            StartCoroutine(CountSpawnTimeDown());
        };
        if (!IsServer) return;
        currentMinute = maxMinute;
        currentSecond = maxSecond;
        StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        while (currentMinute > 0 || currentSecond > 0)
        {
            currentSecond--;
            if (currentSecond == 0)
            {


                if (currentMinute == 0)
                {
                    break;
                }
                else
                {
                    currentSecond = 60;
                }
                currentMinute--;
            }
            CountDownClientsRpc(currentMinute, currentSecond);
            timeManagerUI.SetTimeText(currentMinute, currentSecond);
            yield return new WaitForSeconds(1f);
        }
        CountDownClientsRpc(currentMinute, currentSecond);
        timeManagerUI.SetTimeText(currentMinute, currentSecond);
        TimeOut?.Invoke();
    }
    [Rpc(SendTo.Everyone)]
    private void CountDownClientsRpc(int currentMinute, int currentSecond)
    {
        if (IsServer) return;
        this.currentMinute = currentMinute;
        this.currentSecond = currentSecond;
        timeManagerUI.SetTimeText(currentMinute, currentSecond);
        if (currentMinute == 0 && currentSecond == 0)
        {
            TimeOut?.Invoke();
        }

    }

    private IEnumerator CountSpawnTimeDown()
    {
        currentTimeSpawn = maxTimeSpawn;
        while (currentTimeSpawn >= 0)
        {
            currentTimeSpawn -= Time.deltaTime;
            CountingDownSpawnTime?.Invoke("You will be spawned in " + currentTimeSpawn.ToString("0") + "s");
            yield return null;
        }
        SpawnTimeFinished?.Invoke();
    }
}
