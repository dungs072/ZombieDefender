
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviour
{
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private Image checkMark;

    public void SetPlayerName(string name)
    {
        playerName.text = name;
    }
    public void ToggleCheckMark(bool state)
    {
        checkMark.gameObject.SetActive(state);
    }
}
