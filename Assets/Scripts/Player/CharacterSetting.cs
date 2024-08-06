using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterSetting : NetworkBehaviour
{
    [SerializeField] private CharacterData femaleData;
    [SerializeField] private CharacterData maleData;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        int characterId = PlayerPrefs.GetInt("Id");
        SetUp(characterId);
        SetUpServerRpc(characterId);
    }
    [Rpc(SendTo.Server)]
    private void SetUpServerRpc(int characterId)
    {
        SetUp(characterId);
    }

    private void SetUp(int characterId)
    {
        if (characterId == 1)
        {
            GetComponent<Health>().SetMaxHealth(femaleData.MaxHealth);
            GetComponent<Movement>().SetMovementSpeed(femaleData.MovementSpeed);
        }
        else if (characterId == 2)
        {
            GetComponent<Health>().SetMaxHealth(maleData.MaxHealth);
            GetComponent<Movement>().SetMovementSpeed(maleData.MovementSpeed);
        }
    }
}
