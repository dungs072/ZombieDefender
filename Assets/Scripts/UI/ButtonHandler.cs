
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private UnityEvent buttonHoverIn;
    [SerializeField] private UnityEvent buttonHoverOut;
    [SerializeField] private UnityEvent buttonClick;
    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonHoverIn?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonHoverOut?.Invoke();
    }

    public void HandleButtonClick()
    {
        LeanTween.scale(gameObject, Vector3.one * 0.7f, 0.15f).setEase(LeanTweenType.easeInOutSine).setOnComplete(() =>
        {
            LeanTween.scale(gameObject, Vector3.one, 0.15f).setEase(LeanTweenType.easeInOutSine).setOnComplete(() =>
            {
                buttonClick?.Invoke();
            });

        });
    }


}
