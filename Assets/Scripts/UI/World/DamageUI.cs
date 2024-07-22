using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DamageUI : MonoBehaviour
{
    [SerializeField] private TMP_Text damageText;

    public void SetDamageText(string text)
    {
        damageText.text = text;
    }
    public CanvasGroup GetCanvasGroup()
    {
        return GetComponent<CanvasGroup>();
    }
}
