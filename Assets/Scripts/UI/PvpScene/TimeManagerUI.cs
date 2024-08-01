using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeManagerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;

    public void SetTimeText(int minute, int second)
    {
        timeText.text = minute.ToString("00") + ":" + second.ToString("00");
    }
}
