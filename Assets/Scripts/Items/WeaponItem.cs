using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WeaponItem : Item
{
    [field: SerializeField] public WeaponName weaponName { get; private set; }

    public override void PickUpItem(PlayerController player)
    {
        var weaponManager = player.GetComponent<WeaponManager>();
        GameObject weaponPrefab = LoadWeapon(weaponName.ToString());
        var weaponInstance = Instantiate(weaponPrefab, weaponManager.GetWeaponPack());

        weaponInstance.GetComponent<NetworkObject>().Spawn(true);
        weaponManager.AddWeapon(weaponInstance.GetComponent<Weapon>());
        base.PickUpItem(player);
    }

}
