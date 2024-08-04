
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    [SerializeField] private GameObject selectingIcon;
    [SerializeField] private Sprite notAchievedSprite;
    [SerializeField] private Sprite achievedSprite;
    [SerializeField] private Image icon;
    [SerializeField] private bool isAchieved;
    [SerializeField] private string sceneName;

    public string SceneName { get { return sceneName; } }
    public bool IsAchieved { get { return isAchieved; } }

    private void Start()
    {
        if (icon == null) return;
        if (isAchieved)
        {
            icon.sprite = achievedSprite;
        }
        else
        {
            icon.sprite = notAchievedSprite;
        }

    }
    public void SetAchieve(bool state)
    {
        isAchieved = state;
        if (state)
        {
            icon.sprite = achievedSprite;
        }
        else
        {
            icon.sprite = notAchievedSprite;
        }
    }
    public void ToggleSelectingIcon(bool state)
    {
        selectingIcon.SetActive(state);
    }
}
