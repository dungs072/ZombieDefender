using System;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
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
    public void TakeDamage(int damage)
    {
        if (!IsServer || !IsOwner) { return; }
        if (currentHealth == 0) { return; }
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        HealthChanged?.Invoke(currentHealth, maxHealth);

        CreateDamageUI(damage);
        if (currentHealth <= 0)
        {
            CharacterDied?.Invoke();
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
        var damageUIInstance = Instantiate(damageUIPrefab, transform.position, Quaternion.identity); ;
        damageUIInstance.SetDamageText("-" + damage.ToString());
        LeanTween.move(damageUIInstance.gameObject, transform.position + Vector3.up * 2f, Time.deltaTime * 10f);

        LeanTween.alphaCanvas(damageUIInstance.GetCanvasGroup(), 0, Time.deltaTime * 20).setOnComplete(() =>
        {
            OnFadeComplete();
        });
    }

    private void OnFadeComplete()
    {
        Destroy(damageUIInstance, 1f);
        damageUIInstance = null;
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

}
