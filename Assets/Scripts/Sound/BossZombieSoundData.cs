using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossZombieSoundData : ZombieSoundData
{
    [Header("Attack 2")]
    [SerializeField] private List<AudioClip> attack2s;

    public AudioClip GetAttack2()
    {
        int randomIndex = Random.Range(0, attack2s.Count);
        return attack2s[randomIndex];
    }
}
