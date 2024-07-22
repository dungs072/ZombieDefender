using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UnlockSkill : MonoBehaviour
{
    [SerializeField] private int price = 100;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Button unlockButton;
    private void Start()
    {
        priceText.text = price.ToString();
    }
    public void HandleUnlockSkill()
    {
        var player = ((CustomNetworkManager)NetworkManager.Singleton).OwnerPlayer;
        var moneyManager = player.GetComponent<MoneyManager>();
        if (moneyManager.IsEnoughMoney(price))
        {
            moneyManager.MinusMoney(price);
            unlockButton.interactable = false;
        }
    }
}
