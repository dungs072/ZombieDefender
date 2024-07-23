using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
public class Item : NetworkBehaviour
{
    private PlayerController pickablePlayer;
    public bool IsOccupied { get { return isOccupied.Value; } }
    private NetworkVariable<bool> isOccupied = new NetworkVariable<bool>(default,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            if (player.IsOwner)
            {
                player.GetComponent<PickupHandler>().AddItem(this);
            }
            if (IsServer && pickablePlayer == null)
            {
                pickablePlayer = player;
                isOccupied.Value = true;
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
            if (IsServer && pickablePlayer == player)
            {
                pickablePlayer = null;
                isOccupied.Value = false;
            }
        }
    }
    public virtual void PickUpItem(PlayerController player)
    {
        if (!IsServer) return;
        GetComponent<NetworkObject>().Despawn();
        Destroy(gameObject);
    }
    protected GameObject LoadWeapon(string weaponName)
    {
        return Resources.Load<GameObject>("Weapons/" + weaponName);
    }

}
