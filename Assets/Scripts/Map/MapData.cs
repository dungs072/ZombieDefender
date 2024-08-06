using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MapData", menuName = "MapData", order = 0)]
public class MapData : ScriptableObject
{
    [SerializeField] private List<MapItemData> mapItemDatas;

    public MapItemData GetMapItemData(int id)
    {
        foreach (var item in mapItemDatas)
        {
            if (item.Id == id)
            {
                return item;
            }
        }
        return null;
    }
}
[Serializable]
public class MapItemData
{
    public int Id;
    public Sprite mapIcon;
    public string mapName;
    public string sceneName;
}
