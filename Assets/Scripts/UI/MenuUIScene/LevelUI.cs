using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelUI : MonoBehaviour
{

    [SerializeField] private RectTransform levelUI;
    [SerializeField] private List<Level> levels;
    [SerializeField] private TMP_Text mapName;
    private void Awake()
    {
        LevelManager.LevelSelected += SetMapName;
    }
    private void Start()
    {
        SetMapName("");
    }

    public void HandleSinglePlayClick()
    {
        ToggleLevelUI(true);
    }
    public void ToggleLevelUI(bool state)
    {
        if (state)
        {
            levelUI.gameObject.SetActive(state);
            if (levelUI.localScale != Vector3.zero)
            {
                levelUI.localScale = Vector3.zero;
            }
            LeanTween.scale(levelUI, Vector3.one, 0.3f).setEase(LeanTweenType.easeInOutSine);
        }
        else
        {
            LeanTween.scale(levelUI, Vector3.zero, 0.3f).setEase(LeanTweenType.easeInOutSine).setOnComplete(() =>
            {
                levelUI.gameObject.SetActive(state);
            });
        }
    }
    public void SetMapName(string text)
    {
        mapName.text = text;
    }
}
