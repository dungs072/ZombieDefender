using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScorePlayerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text playerName;

    [SerializeField] private TMP_Text scoreText;

    public void SetPlayerName(string text)
    {
        playerName.text = text;
    }
    public void SetScoreText(string text)
    {
        scoreText.text = text;
    }
}
