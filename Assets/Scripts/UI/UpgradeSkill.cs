using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSkill : MonoBehaviour
{
    [SerializeField] private int price = 100;
    [SerializeField] private Sprite selectedSprite;
    [SerializeField] private List<Image> unselectedImages;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TMP_Text priceText;
    private int currentIndex = 0;
    private void Start()
    {
        SetPriceText(price.ToString());
    }
    public void HandleUpgradeSkill()
    {
        var player = ((CustomNetworkManager)NetworkManager.Singleton).OwnerPlayer;
        var moneyManager = player.GetComponent<MoneyManager>();
        if (moneyManager.IsEnoughMoney(price))
        {
            moneyManager.MinusMoney(price);
            unselectedImages[currentIndex].sprite = selectedSprite;
            currentIndex++;


            if (currentIndex == unselectedImages.Count)
            {
                upgradeButton.interactable = false;
            }
            else
            {
                price = price * 2;
                SetPriceText(price.ToString());
            }
        }
    }
    public void SetPriceText(string text)
    {
        priceText.text = text;
    }
}
