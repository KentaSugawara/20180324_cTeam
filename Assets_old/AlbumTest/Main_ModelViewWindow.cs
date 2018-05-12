using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Main_ModelViewWindow : MonoBehaviour {
    [SerializeField]
    private float _RotateScale;

    public void Init()
    {
        transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
    }

    private Vector2 _LastPoint;
    public void PointerDown(BaseEventData e)
    {
        PointerEventData point = (PointerEventData)e;

        _LastPoint = point.position;
    }

    public void Drag(BaseEventData e)
    {
        PointerEventData point = (PointerEventData)e;

        transform.localRotation *= Quaternion.Euler(0.0f, _RotateScale * (_LastPoint.x - point.position.x), 0.0f);
        _LastPoint = point.position;
    }
}
