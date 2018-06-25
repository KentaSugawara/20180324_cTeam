using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tutorial_EventTriggerDummy : MonoBehaviour {
    [SerializeField]
    private EventTrigger _TargetEventTrigger;

    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }

    public void Init(EventTrigger eventtrigger, bool isCameraCanvas = false, Camera TargetCamera = null)
    {
        _TargetEventTrigger = eventtrigger;
        if (isCameraCanvas) ((RectTransform)transform).position = TargetCamera.WorldToScreenPoint(eventtrigger.transform.position);
        else ((RectTransform)transform).position = eventtrigger.transform.position;

        ((RectTransform)transform).sizeDelta = ((RectTransform)eventtrigger.transform).sizeDelta;
    }

    public void OnPointerClick(BaseEventData eventdata)
    {
        _TargetEventTrigger.OnPointerClick((PointerEventData)eventdata);
    }

    public void OnPointerDown(BaseEventData eventdata)
    {
        _TargetEventTrigger.OnPointerDown((PointerEventData)eventdata);
    }


    public void OnPointerUp(BaseEventData eventdata)
    {
        _TargetEventTrigger.OnPointerUp((PointerEventData)eventdata);
    }

    public void OnPointerEnter(BaseEventData eventdata)
    {
        _TargetEventTrigger.OnPointerEnter((PointerEventData)eventdata);
    }

    public void OnPointerExit(BaseEventData eventdata)
    {
        _TargetEventTrigger.OnPointerExit((PointerEventData)eventdata);
    }

    public void OnBeginDrag(BaseEventData eventdata)
    {
        _TargetEventTrigger.OnBeginDrag((PointerEventData)eventdata);
    }

    public void OnDrag(BaseEventData eventdata)
    {
        _TargetEventTrigger.OnDrag((PointerEventData)eventdata);
    }

    public void OnEndDrag(BaseEventData eventdata)
    {
        _TargetEventTrigger.OnEndDrag((PointerEventData)eventdata);
    }
}
