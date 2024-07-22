using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsUI : MonoBehaviour
{
    [SerializeField] private RectTransform skillPanel;
    [SerializeField] private GameObject blackout;
    public void ToggleSkillPanel(bool state)
    {
        blackout.SetActive(state);
        if (state)
        {
            skillPanel.gameObject.SetActive(state);
            if (skillPanel.localScale != Vector3.zero)
            {
                skillPanel.localScale = Vector3.zero;
            }
            LeanTween.scale(skillPanel, Vector3.one, 0.3f).setEase(LeanTweenType.easeInOutSine);
        }
        else
        {
            LeanTween.scale(skillPanel, Vector3.zero, 0.3f).setEase(LeanTweenType.easeInOutSine).setOnComplete(() =>
            {
                skillPanel.gameObject.SetActive(state);
            });
        }
    }
}
