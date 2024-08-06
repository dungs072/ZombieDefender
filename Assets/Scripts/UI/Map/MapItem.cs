using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapItem : MonoBehaviour
{
    [SerializeField] private int mapId;
    [SerializeField] private GameObject selectedIcon;
    public int MapId { get { return mapId; } }
    public void ToggleSelectedIcon(bool state)
    {
        selectedIcon.SetActive(state);
    }
}
