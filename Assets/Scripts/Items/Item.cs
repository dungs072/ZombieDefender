using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
public class Item : NetworkBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.TryGetComponent(out PlayerController player))
        {
            if (player.IsOwner)
            {
                player.GetComponent<PickupHandler>().AddItem(this);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            if (player.IsOwner)
            {
                player.GetComponent<PickupHandler>().RemoveItem(this);
            }
        }
    }
    public virtual void PickUpItem()
    {
        if (!IsServer) return;
        GetComponent<NetworkObject>().Despawn();
        Destroy(gameObject);
    }
}
