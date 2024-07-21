using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class InGameUI : MonoBehaviour
{
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


    private void Start()
    {
        PlayerController.PlayerSpawned += HandlePlayerSpawned;
        PlayerController.PlayerDespawned += HandlePlayerDespawned;

        WeaponManager.WeaponChanged += SetWeaponIcon;
        Weapon.DetailChanged += SetWeaponDetail;

        ShootWeapon.ReloadingTimeChanged += SetReloadingBar;
        // ShootWeapon.ReloadingTimeStartChanged += BlinkReloadingText;
    }
    private void OnDestroy()
    {
        PlayerController.PlayerSpawned -= HandlePlayerSpawned;
        PlayerController.PlayerDespawned -= HandlePlayerDespawned;

        WeaponManager.WeaponChanged -= SetWeaponIcon;
        Weapon.DetailChanged -= SetWeaponDetail;
        ShootWeapon.ReloadingTimeChanged -= SetReloadingBar;
        // ShootWeapon.ReloadingTimeStartChanged -= BlinkReloadingText;
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
    public void SetScore(string text)
    {
        score.text = text;
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
}
