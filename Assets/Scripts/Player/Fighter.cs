using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private WeaponManager weaponManager;
    public void Attack()
    {
        weaponManager.CurrentWeapon.Operation();
    }


    
}
