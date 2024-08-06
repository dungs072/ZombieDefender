using System;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    public static event Action<ulong> CharacterKilled;
    public event Action TakingDamage;
    public event Action<int, int> HealthChanged;
    public event Action CharacterDied;

    [SerializeField] private int maxHealth = 100;
    [SerializeField] private DamageUI damageUIPrefab;

    private DamageUI damageUIInstance;
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
    public void SetDie()
    {
        TakeDamage(120000, currentHealth);
    }
    public void TakeDamage(ulong killerId, int damage)
    {
        if (!IsOwner) { return; }
        if (currentHealth == 0) { return; }
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        HealthChanged?.Invoke(currentHealth, maxHealth);

        CreateDamageUI(damage);
        if (currentHealth <= 0)
        {
            CharacterDied?.Invoke();
            CharacterKilled?.Invoke(killerId);
            IsDead = true;
        }
        if (damage > 0)
        {
            TakingDamage?.Invoke();
        }
    }
    public void Reset()
    {
        currentHealth = maxHealth;
        IsDead = false;
        HealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void CreateDamageUI(int damage)
    {
        damageUIInstance = Instantiate(damageUIPrefab, transform.position, Quaternion.identity); ;
        damageUIInstance.SetDamageText("-" + damage.ToString());
        LeanTween.move(damageUIInstance.gameObject, transform.position + Vector3.up * 2f, Time.deltaTime * 10f);
        Destroy(damageUIInstance.gameObject, Time.deltaTime * 20);
        LeanTween.alphaCanvas(damageUIInstance.GetCanvasGroup(), 0, Time.deltaTime * 20);
    }

    public void AddMaxHealth(int amount)
    {
        maxHealth += amount;
        HealthChanged?.Invoke(currentHealth, maxHealth);
    }
    public void AddCurrentHealth(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        HealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void SetMaxHealth(int amount)
    {
        maxHealth = amount;
        currentHealth = amount;
        HealthChanged?.Invoke(currentHealth, maxHealth);
    }

}
