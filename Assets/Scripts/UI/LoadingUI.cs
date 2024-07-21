using TMPro;
using UnityEngine;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] private RectTransform loadingProgressBar;
    [SerializeField] private TMP_Text loadingProgressValue;
    [SerializeField] private TMP_Text loadingText;

    public void SetLoadingProgress(float factor)
    {
        loadingProgressBar.localScale = new Vector3(factor, 1, 1);
        loadingProgressValue.text = (factor * 100f).ToString("F2") + "%";

    }
    public void SetLoadingText(string text)
    {
        loadingText.text = text;
    }
    public void BlynkLoadingText()
    {
        LeanTween.alphaText(loadingText.rectTransform, 0f, 0.3f).setLoopPingPong();
    }
}
