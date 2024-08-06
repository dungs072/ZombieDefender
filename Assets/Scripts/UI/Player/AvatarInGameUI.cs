using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class AvatarInGameUI : MonoBehaviour
{
    [SerializeField] private Sprite female;
    [SerializeField] private Sprite male;
    [SerializeField] private Image avatar;

    private void Start()
    {
        int characterId = PlayerPrefs.GetInt("Id");
        if (characterId == 1)
        {
            avatar.sprite = female;
        }
        else if (characterId == 2)
        {
            avatar.sprite = male;
        }
    }
}
