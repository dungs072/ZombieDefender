using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ZombieSoundData", menuName = "ZombieSoundData", order = 0)]
public class ZombieSoundData : ScriptableObject
{
    [Header("Run in dirt")]
    [SerializeField] private List<AudioClip> runDirts;

    [Header("Attack")]
    [SerializeField] private List<AudioClip> attacks;

    [Header("Idle")]
    [SerializeField] private List<AudioClip> idles;
    [Header("Being attack")]
    [SerializeField] private List<AudioClip> bulletHits;
    [SerializeField] private List<AudioClip> meleeHits;
    [Header("Death")]
    [SerializeField] private List<AudioClip> deaths;
    [Header("Alert")]
    [SerializeField] private List<AudioClip> alerts;
    [Header("Throw")]
    [SerializeField] private List<AudioClip> throws;

    public AudioClip GetRunDirt()
    {
        int randomIndex = Random.Range(0, runDirts.Count);
        return runDirts[randomIndex];
    }

    public AudioClip GetAttack()
    {
        int randomIndex = Random.Range(0, attacks.Count);
        return attacks[randomIndex];
    }
    public AudioClip GetIdle()
    {
        int randomIndex = Random.Range(0, idles.Count);
        return idles[randomIndex];
    }
    public AudioClip GetBulletHit()
    {
        int randomIndex = Random.Range(0, bulletHits.Count);
        return bulletHits[randomIndex];
    }
    public AudioClip GetMeleeHit()
    {
        int randomIndex = Random.Range(0, meleeHits.Count);
        return meleeHits[randomIndex];
    }

    public AudioClip GetDeath()
    {
        int randomIndex = Random.Range(0, deaths.Count);
        return deaths[randomIndex];
    }
    public AudioClip GetAlert()
    {
        int randomIndex = Random.Range(0, alerts.Count);
        return alerts[randomIndex];
    }
    public AudioClip GetThrow()
    {
        int randomIndex = Random.Range(0, throws.Count);
        return throws[randomIndex];
    }
}
