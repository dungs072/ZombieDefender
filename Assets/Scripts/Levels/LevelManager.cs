using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<Level> levels;
    private int currentLevelIndex = -1;

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
        }
    }

    public void PlayLevel()
    {
        if (currentLevelIndex == -1) return;
        SceneController.Instance.StartMyServer(true, levels[currentLevelIndex].SceneName);
    }
}
