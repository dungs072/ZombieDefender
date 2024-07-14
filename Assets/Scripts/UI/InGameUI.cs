using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InGameUI : MonoBehaviour
{
    [SerializeField] private RectTransform fontHealthBar;
    [SerializeField] private TMP_Text healthBarStats;

    private void Start()
    {
        PlayerController.PlayerSpawned += HandlePlayerSpawned;
        PlayerController.PlayerDespawned += HandlePlayerDespawned;
    }

    private void HandlePlayerSpawned(PlayerController player)
    {
        player.GetComponent<Health>().HealthChanged += UpdateHealthBar;
    }
    private void HandlePlayerDespawned(PlayerController player)
    {
        player.GetComponent<Health>().HealthChanged -= UpdateHealthBar;
    }
    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        SetHealthBarStats(currentHealth.ToString() + "/" + maxHealth.ToString());
        float factor = (float)currentHealth / (float)maxHealth;
        fontHealthBar.localScale = new Vector3(factor, 1, 1);
    }
    public void SetHealthBarStats(string text)
    {
        healthBarStats.text = text;
    }
}
