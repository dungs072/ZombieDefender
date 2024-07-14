using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    public event Action<int, int> HealthChanged;
    public event Action CharacterDied;
    [SerializeField] private int maxHealth = 100;
    private int currentHealth = 0;

    public bool IsDead { get; private set; } = false;
    private void OnEnable()
    {
        Reset();
    }
    private void Start()
    {
        Reset();
    }
    public void TakeDamage(int damage)
    {
        if (!IsServer || !IsOwner) { return; }
        if (currentHealth == 0) { return; }
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        HealthChanged?.Invoke(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            CharacterDied?.Invoke();
            IsDead = true;
        }
    }
    public void Reset()
    {
        currentHealth = maxHealth;
        IsDead = false;
        HealthChanged?.Invoke(currentHealth, maxHealth);
    }


}
