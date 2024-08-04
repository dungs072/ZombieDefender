using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine.SceneManagement;
public class InGameUI : MonoBehaviour
{
    public static event Action GameOverPVPUIOn;
    [Header("Weapon")]
    [SerializeField] private Image weaponIcon;
    [SerializeField] private TMP_Text weaponDetail;
    [Header("Health bar")]
    [SerializeField] private RectTransform fontHealthBar;
    [SerializeField] private TMP_Text healthBarStats;
    [Header("Achievement")]
    [SerializeField] private TMP_Text score;
    [Header("Hold")]
    [SerializeField] private RectTransform holdingMenu;
    [SerializeField] private GameObject blackout;
    [Header("Reloading")]
    [SerializeField] private RectTransform reloadingBar;
    [SerializeField] private RectTransform reloadingFont;
    [SerializeField] private TMP_Text reloadingText;
    [Header("Energy")]
    [SerializeField] private RectTransform fontEnergyBar;
    [SerializeField] private TMP_Text energyStats;
    [Header("Pick up")]
    [SerializeField] private GameObject pickupImage;
    [SerializeField] private TMP_Text nameItemText;
    [Header("Money")]
    [SerializeField] private TMP_Text moneyText;
    [Header("Blood Damage")]
    [SerializeField] private Image bloodDamageImage;
    [SerializeField] private float fadeInValue;
    [SerializeField] private float fadeDuration = 0.4f;
    [Header("Game Over")]
    [SerializeField] private RectTransform gameOverPanel;
    [SerializeField] private TMP_Text killText;
    [SerializeField] private RectTransform gameOverPVPPanel;
    [SerializeField] private TMP_Text spawnTimeText;
    [Header("Main Game")]
    [SerializeField] private RectTransform mainGamePanel;
    [Header("Game Win")]
    [SerializeField] private RectTransform gameWinPanel;
    [SerializeField] private TMP_Text winKillText;
    [Header("Loading scene")]
    [SerializeField] private LoadingUI loadingUI;

    private void Start()
    {
        var ownedPlayer = ((CustomNetworkManager)NetworkManager.Singleton).OwnerPlayer;
        HandlePlayerSpawned(ownedPlayer);

        PickupHandler.ItemPicked += TogglePickupImage;
        PickupHandler.HoldingPickup += SetReloadingBar;
        MoneyManager.MoneyChanged += SetMoneyText;
        Achievement.KillsChanged += SetScore;

        WeaponManager.WeaponChanged += SetWeaponIcon;
        Weapon.DetailChanged += SetWeaponDetail;

        ShootWeapon.ReloadingTimeChanged += SetReloadingBar;

        TimeManager.SpawnTimeFinished += () =>
        {
            TogglePVPGameOver(false);
        };
        TimeManager.CountingDownSpawnTime += SetGameOverRespawnText;
        SetScore("0");

        SceneController.Instance.LoadingUI = loadingUI;
        GameController.GameWin += () =>
        {
            ToggleGameWin(true);
            SetWinKillText(score.text);
        };

    }

    private void OnDestroy()
    {
        PickupHandler.ItemPicked -= TogglePickupImage;
        PickupHandler.HoldingPickup -= SetReloadingBar;
        MoneyManager.MoneyChanged -= SetMoneyText;
        Achievement.KillsChanged -= SetScore;

        WeaponManager.WeaponChanged -= SetWeaponIcon;
        Weapon.DetailChanged -= SetWeaponDetail;
        ShootWeapon.ReloadingTimeChanged -= SetReloadingBar;
    }
    private void TogglePickupImage(bool state, string nameItem)
    {
        pickupImage.SetActive(state);
        SetNameItemText(nameItem);
    }
    private void SetNameItemText(string text)
    {
        nameItemText.text = text;
    }

    private void SetWeaponIcon(Sprite icon)
    {
        weaponIcon.sprite = icon;
    }
    private void SetWeaponDetail(int detail1, int detail2)
    {
        weaponDetail.text = detail1.ToString() + "/" + detail2.ToString();
    }

    private void HandlePlayerSpawned(PlayerController player)
    {
        var health = player.GetComponent<Health>();
        var energy = player.GetComponent<Energy>();
        health.HealthChanged += UpdateHealthBar;
        health.TakingDamage += FadeBloodImageIn;
        energy.EnergyChanged += UpdateEnergyBar;
        player.CharacterDiedUI += TurnOnGameOver;

        health.TakeDamage(health.NetworkObjectId, 0);
        energy.UseEnergy(0);

    }
    private void HandlePlayerDespawned(PlayerController player)
    {
        player.GetComponent<Health>().HealthChanged -= UpdateHealthBar;
        player.GetComponent<Energy>().EnergyChanged -= UpdateEnergyBar;
    }
    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        SetHealthBarStats(currentHealth.ToString() + "/" + maxHealth.ToString());
        float factor = (float)currentHealth / (float)maxHealth;
        fontHealthBar.localScale = new Vector3(factor, 1, 1);
    }
    private void FadeBloodImageIn()
    {
        LeanTween.alpha(bloodDamageImage.rectTransform, fadeInValue, fadeDuration).setOnComplete(() =>
        {
            LeanTween.alpha(bloodDamageImage.rectTransform, 0f, fadeDuration);
        });
    }
    public void SetHealthBarStats(string text)
    {
        healthBarStats.text = text;
    }
    public void UpdateEnergyBar(int currentEnergy, int maxEnergy)
    {
        SetEnergyBarStats(currentEnergy.ToString() + "/" + maxEnergy.ToString());
        float factor = (float)currentEnergy / (float)maxEnergy;
        fontEnergyBar.localScale = new Vector3(factor, 1, 1);
    }
    public void SetEnergyBarStats(string text)
    {
        energyStats.text = text;
    }
    public void SetScore(string text)
    {
        score.text = "Kills: " + text;
    }
    public void SetMoneyText(string text)
    {
        moneyText.text = text;
    }
    public void ToggleHoldingMenu(bool state)
    {
        blackout.SetActive(state);
        if (state)
        {
            holdingMenu.gameObject.SetActive(state);
            if (holdingMenu.localScale != Vector3.zero)
            {
                holdingMenu.localScale = Vector3.zero;
            }
            LeanTween.scale(holdingMenu, Vector3.one, 0.3f).setEase(LeanTweenType.easeInOutSine);
        }
        else
        {
            LeanTween.scale(holdingMenu, Vector3.zero, 0.3f).setEase(LeanTweenType.easeInOutSine).setOnComplete(() =>
            {
                holdingMenu.gameObject.SetActive(state);
            });
        }
    }
    public void SetReloadingBar(float factor)
    {
        reloadingBar.gameObject.SetActive(factor > 0.1f);
        reloadingFont.localScale = new Vector3(factor, 1f, 1f);
    }
    public void BlinkReloadingText()
    {
        LeanTween.alphaText(reloadingText.rectTransform, 0f, 0.3f).setOnComplete(() =>
        {
            LeanTween.alphaText(reloadingText.rectTransform, 1f, 0.3f).setOnComplete(() =>
            {
                if (reloadingBar.gameObject.activeSelf)
                {
                    BlinkReloadingText();
                }
                else
                {
                    return;
                }

            });
        });
    }
    private void TurnOnGameOver()
    {
        Lobby lobby = MatchMaking.GetCurrentLobby();
        string mode = GetStringValue(Constants.GameTypeKey);

        string GetStringValue(string key)
        {
            if (lobby == null)
            {
                return "PVE";
            }
            return lobby.Data[key].Value;
        }
        //var players = ((CustomNetworkManager)NetworkManager.Singleton).Players;
        if (mode == "PVP")
        {
            TogglePVPGameOver(true);
        }
        else
        {
            ToggleGameOver(true);
            SetKillCountText(score.text);
        }

    }
    public void SetGameOverRespawnText(string text)
    {
        spawnTimeText.text = text;
    }
    public void ToggleGameOver(bool state)
    {
        mainGamePanel.gameObject.SetActive(!state);
        blackout.SetActive(state);
        if (state)
        {
            gameOverPanel.gameObject.SetActive(state);

            if (gameOverPanel.localScale != Vector3.zero)
            {
                gameOverPanel.localScale = Vector3.zero;
            }
            LeanTween.scale(gameOverPanel, Vector3.one, 0.3f).setEase(LeanTweenType.easeInOutSine);
        }
        else
        {
            LeanTween.scale(gameOverPanel, Vector3.zero, 0.3f).setEase(LeanTweenType.easeInOutSine).setOnComplete(() =>
            {
                gameOverPanel.gameObject.SetActive(state);
            });
        }
    }
    public void SetKillCountText(string text)
    {
        killText.text = text;
    }
    public void ToggleGameWin(bool state)
    {
        gameWinPanel.gameObject.SetActive(!state);
        blackout.SetActive(state);
        if (state)
        {
            gameWinPanel.gameObject.SetActive(state);

            if (gameWinPanel.localScale != Vector3.zero)
            {
                gameWinPanel.localScale = Vector3.zero;
            }
            LeanTween.scale(gameWinPanel, Vector3.one, 0.3f).setEase(LeanTweenType.easeInOutSine);
        }
        else
        {
            LeanTween.scale(gameWinPanel, Vector3.zero, 0.3f).setEase(LeanTweenType.easeInOutSine).setOnComplete(() =>
            {
                gameWinPanel.gameObject.SetActive(state);
            });
        }
    }
    public void SetWinKillText(string text)
    {
        winKillText.text = text;
    }
    public void TogglePVPGameOver(bool state)
    {
        mainGamePanel.gameObject.SetActive(!state);
        blackout.SetActive(state);
        if (state)
        {
            gameOverPVPPanel.gameObject.SetActive(state);

            if (gameOverPVPPanel.localScale != Vector3.zero)
            {
                gameOverPVPPanel.localScale = Vector3.zero;
            }
            LeanTween.scale(gameOverPVPPanel, Vector3.one, 0.3f).
                    setEase(LeanTweenType.easeInOutSine).setOnComplete(() =>
            {
                GameOverPVPUIOn?.Invoke();
            });
        }
        else
        {
            LeanTween.scale(gameOverPVPPanel, Vector3.zero, 0.3f).setEase(LeanTweenType.easeInOutSine).setOnComplete(() =>
            {
                gameOverPVPPanel.gameObject.SetActive(state);
            });
        }
    }
}
