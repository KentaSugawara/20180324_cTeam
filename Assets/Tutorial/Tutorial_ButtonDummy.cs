using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tutorial_ButtonDummy : MonoBehaviour {
    [SerializeField]
    private Button _TargetButton;

    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }

    public void Init(Button button, bool isCameraCanvas = false, Camera TargetCamera = null)
    {
        _TargetButton = button;
        if (isCameraCanvas) ((RectTransform)transform).position = TargetCamera.WorldToScreenPoint(button.transform.position);
        else ((RectTransform)transform).position = button.transform.position;

        ((RectTransform)transform).sizeDelta = ((RectTransform)button.transform).sizeDelta;
    }

    public void OnPointerClick(BaseEventData eventdata)
    {
        _TargetButton.OnPointerClick((PointerEventData)eventdata);
    }

    public void OnPointerDown(BaseEventData eventdata)
    {
        _TargetButton.OnPointerDown((PointerEventData)eventdata);
    }


    public void OnPointerUp(BaseEventData eventdata)
    {
        _TargetButton.OnPointerUp((PointerEventData)eventdata);
    }

    public void OnPointerEnter(BaseEventData eventdata)
    {
        _TargetButton.OnPointerEnter((PointerEventData)eventdata);
    }

    public void OnPointerExit(BaseEventData eventdata)
    {
        _TargetButton.OnPointerExit((PointerEventData)eventdata);
    }
}
