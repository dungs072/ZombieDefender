using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowHandler : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform throwPos;
    public void ThrowGrenade()
    {
        var itemInstance = Instantiate(itemPrefab, throwPos.position, throwPos.rotation);
        Destroy(itemInstance, 10f);
    }
}
