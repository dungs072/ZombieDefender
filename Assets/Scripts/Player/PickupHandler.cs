using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PickupHandler : NetworkBehaviour
{
    public static event Action<bool> ItemPicked;
    public static event Action<float> HoldingPickup;
    [SerializeField] private float maxTimeHolding = 5f;
    private List<Item> items = new List<Item>();
    private float currentTime = 0;
    public void AddItem(Item item)
    {

        if (!IsOwner) return;
        if (items.Contains(item)) return;

        items.Add(item);
        ItemPicked?.Invoke(items.Count > 0);
    }
    public void RemoveItem(Item item)
    {
        if (!IsOwner) return;
        if (!items.Contains(item)) return;
        items.Remove(item);
        ItemPicked?.Invoke(items.Count > 0);
    }

    public void HandleHoldingPickup(bool state)
    {
        if (items.Count == 0) return;
        if (state)
        {
            currentTime += Time.deltaTime;
            if (currentTime > maxTimeHolding)
            {
                currentTime = 0;
                Item item = items[items.Count - 1];
                items.RemoveAt(items.Count - 1);
                item.PickUpItem();
                ItemPicked?.Invoke(items.Count > 0);
                // process multiple pick up here

            }
        }
        else
        {
            currentTime = 0;
        }
        if (currentTime == 0)
        {
            HoldingPickup?.Invoke(0);
        }
        else
        {
            HoldingPickup?.Invoke(1 - (currentTime / maxTimeHolding));
        }

    }


}
