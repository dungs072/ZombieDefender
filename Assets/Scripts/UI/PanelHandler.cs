using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PanelHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private UnityEvent buttonHoverIn;
    [SerializeField] private UnityEvent buttonHoverOut;
    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonHoverIn?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonHoverOut?.Invoke();
    }

}
