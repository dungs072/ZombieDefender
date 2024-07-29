using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUI : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private GameObject selectedRing;
    [SerializeField] private Sprite icon;
    [SerializeField] private string note;
    public int Id { get { return id; } }
    public Sprite Sprite { get { return icon; } }
    public string Note { get { return note; } }
    public void ToggleSelectedRing(bool state)
    {
        selectedRing.SetActive(state);

    }

}
