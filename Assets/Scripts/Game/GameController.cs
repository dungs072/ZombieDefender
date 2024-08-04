using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameController : NetworkBehaviour
{
    public static event Action GameWin;
    [SerializeField] private List<Spawner> spawners;
    [SerializeField] private List<Spawner> bossSpawners;
    [SerializeField] private AIManager aIManager;
    [SerializeField] private InGameUI inGameUI;
    [SerializeField] private float timeToSpawn = 2f;
    [Header("Existing zombies")]
    [SerializeField] private List<Transform> existingZombiePos;
    [SerializeField] private Spawner existingSpawner;
    [Header("Zombie Trigger")]
    [SerializeField] private List<ZombieTrigger> zombieTriggers;
    [Header("Sounds")]
    [SerializeField] private AudioClip hornSound;
    [SerializeField] private AudioSource tankSourceSound;
    private float currentTime = 0;
    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        aIManager.BossSpawned += SpawnBoss;

        foreach (var spawner in spawners)
        {
            spawner.AISpawned += aIManager.AddNetworkAI;
        }
        foreach (var bossSpawner in bossSpawners)
        {
            bossSpawner.AISpawned += aIManager.AddNetworkBossAI;
        }

        existingSpawner.AISpawned += aIManager.AddNetworkAI;
        ZombieTrigger.ZombieTriggered += TriggerZombie;
        currentTime = 0;
        Invoke(nameof(SpawnExistingZombies), Const.ReloadingTimeAdded);
    }
    private void SpawnExistingZombies()
    {
        foreach (var tempTransform in existingZombiePos)
        {
            existingSpawner.SpawnObject(tempTransform.position);
        }
    }
    private void TriggerZombie(ZombieTrigger zombieTrigger)
    {
        if (audioSource == null) return;
        audioSource.PlayOneShot(hornSound);
        aIManager.startZombie = true;
    }

    private void Update()
    {
        if (!IsServer) return;
        if (!aIManager.startZombie) return;
        currentTime += Time.deltaTime;
        if (currentTime >= timeToSpawn)
        {
            foreach (var spawner in spawners)
            {
                spawner.UpdateSpawner();
            }
            currentTime = timeToSpawn;
        }
    }


    public void ResetPlayer()
    {
        tankSourceSound.Stop();
        var player = ((CustomNetworkManager)NetworkManager.Singleton).OwnerPlayer;
        player.ResetPlayer();
        player.GetComponent<Achievement>().ResetKill();
        inGameUI.ToggleGameOver(false);
        aIManager.ResetZombies();
        SpawnExistingZombies();


        foreach (var spawner in spawners)
        {
            spawner.ResetSpawner();
        }
        foreach (var spawner in bossSpawners)
        {
            spawner.ResetSpawner();
        }
        foreach (var zombieTrigger in zombieTriggers)
        {
            zombieTrigger.gameObject.SetActive(true);
        }

    }
    public void SpawnBoss()
    {
        foreach (var boss in bossSpawners)
        {
            var bossController = boss.SpawnObject(Vector3.zero);
            bossController.GetComponent<Health>().CharacterDied += () =>
            {
                ToggleTankSourceSound(false);
                // foreach (var spawner in spawners)
                // {
                //     spawner.ResetSpawner();
                // }
                GameWin?.Invoke();

            };
        }
        ToggleTankSourceSound(true);

    }
    public void ToggleTankSourceSound(bool state)
    {
        if (state)
        {
            tankSourceSound.Play();
        }
        else
        {
            tankSourceSound.Stop();
        }
    }
}
