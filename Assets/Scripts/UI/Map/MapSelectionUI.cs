using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSelectionUI : MonoBehaviour
{
    public static event Action<int> MapChose;
    [SerializeField] private List<MapItem> maps;
    [SerializeField] private GameObject blackout;
    [SerializeField] private RectTransform mapSelectionPanel;
    private MapItem currentMapItem;
    public void SelectMap(MapItem mapItem)
    {
        foreach (var map in maps)
        {
            map.ToggleSelectedIcon(mapItem == map);
            if (mapItem == map)
            {
                currentMapItem = map;
            }
        }
    }
    public void ChooseMap()
    {
        MapChose?.Invoke(currentMapItem.MapId);
    }
    public void TogglePanel(bool state)
    {
        blackout.SetActive(state);
        if (state)
        {
            mapSelectionPanel.gameObject.SetActive(state);
            if (mapSelectionPanel.localScale != Vector3.zero)
            {
                mapSelectionPanel.localScale = Vector3.zero;
            }
            LeanTween.scale(mapSelectionPanel, Vector3.one, 0.3f).setEase(LeanTweenType.easeInOutSine);
        }
        else
        {
            LeanTween.scale(mapSelectionPanel, Vector3.zero, 0.3f).setEase(LeanTweenType.easeInOutSine).setOnComplete(() =>
            {
                mapSelectionPanel.gameObject.SetActive(state);
            });
        }
    }

}
