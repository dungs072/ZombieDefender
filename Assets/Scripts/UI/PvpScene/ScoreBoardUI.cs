using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoardUI : MonoBehaviour
{
    [SerializeField] private ScorePlayerUI scorePlayerUIPrefab;
    [SerializeField] private Transform content;
    [SerializeField] private RectTransform scoreBoard;
    [SerializeField] private GameObject blackout;
    [SerializeField] private GameObject mainGamePanel;
    public void SpawnScorePlayerUI(string playerName, string scoreText)
    {
        var instance = Instantiate(scorePlayerUIPrefab, content);
        instance.SetPlayerName(playerName);
        instance.SetScoreText(scoreText);
    }
    public void TogglePVPGameOver(bool state)
    {
        if (mainGamePanel != null)
        {
            mainGamePanel.gameObject.SetActive(!state);
        }

        blackout.SetActive(state);
        if (state)
        {
            scoreBoard.gameObject.SetActive(state);

            if (scoreBoard.localScale != Vector3.zero)
            {
                scoreBoard.localScale = Vector3.zero;
            }
            LeanTween.scale(scoreBoard, Vector3.one, 0.3f).
                    setEase(LeanTweenType.easeInOutSine);
        }
        else
        {
            LeanTween.scale(scoreBoard, Vector3.zero, 0.3f).setEase(LeanTweenType.easeInOutSine).setOnComplete(() =>
            {
                scoreBoard.gameObject.SetActive(state);
            });
        }
    }
}
