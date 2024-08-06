using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static event Action<string> LevelSelected;
    [SerializeField] private List<Level> levels;
    private static int currentLevelIndex = -1;

    public void ChooseLevel(Level level)
    {
        for (int i = 0; i < levels.Count; i++)
        {
            if (levels[i] == level)
            {
                if (i - 1 < 0)
                {
                    currentLevelIndex = i;
                    SelectLevel(level);

                }
                else
                {

                    if (levels[i - 1].IsAchieved)
                    {
                        currentLevelIndex = i;
                        SelectLevel(level);
                    }
                }
            }
        }
    }
    public void SelectLevel(Level selectedLevel)
    {
        foreach (Level level in levels)
        {
            level.ToggleSelectingIcon(selectedLevel == level);
            if (selectedLevel == level)
            {
                LevelSelected?.Invoke(level.LevelName);
            }
        }
    }

    public void PlayLevel()
    {
        if (currentLevelIndex == -1) currentLevelIndex = 0;
        StartCoroutine(SceneController.Instance.StartMyServer(true, levels[currentLevelIndex].SceneName));
    }
    public void PlayLevelAgain()
    {
        if (currentLevelIndex == -1) currentLevelIndex = 0;

        SceneController.Instance.LoadScene(levels[currentLevelIndex].SceneName);
    }
    public void PlayNextLevel()
    {
        if (currentLevelIndex == -1) currentLevelIndex = 0;
        currentLevelIndex = GetNextLevel();

        StartCoroutine(SceneController.Instance.StartMyServer(true, levels[currentLevelIndex].SceneName));
    }

    public static void SaveFinishLevel()
    {
        PlayerPrefs.SetInt("level", currentLevelIndex);
    }
    public static int GetNextLevel()
    {
        if (!PlayerPrefs.HasKey("level"))
        {
            return 0;
        }
        return PlayerPrefs.GetInt("level") + 1;
    }

}
