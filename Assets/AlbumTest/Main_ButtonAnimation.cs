using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Main_ButtonAnimation : MonoBehaviour {
    private Vector3 _Scale;

    [SerializeField]
    private Vector3 _EndScale = Vector3.zero;

    [SerializeField]
    private float _HalfTime = 0.5f;

    private void OnEnable()
    {
        transform.localScale = _Scale;
    }

    private void Awake()
    {
        EventTrigger currentTrigger = gameObject.AddComponent<EventTrigger>();
        currentTrigger.triggers = new List<EventTrigger.Entry>();

        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((x) => PointerDown());

            currentTrigger.triggers.Add(entry);
        }

        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerUp;
            entry.callback.AddListener((x) => PointerUp());

            currentTrigger.triggers.Add(entry);
        }

        _Scale = transform.localScale;
    }

    public void PointerDown()
    {
        StopAllCoroutines();
        StartCoroutine(Routine_Down());
    }

    private IEnumerator Routine_Down()
    {
        Vector3 b;
        for (float t = 0.0f; t < _HalfTime; t += Time.deltaTime)
        {
            float e = t / _HalfTime;
            b = Vector3.Lerp(_Scale, _EndScale, e);
            transform.localScale = Vector3.Lerp(b, _EndScale, e);
            yield return null;
        }

        transform.localScale = _EndScale;
    }

    public void PointerUp()
    {
        StopAllCoroutines();
        StartCoroutine(Routine_Up());
    }

    private IEnumerator Routine_Up()
    {
        Vector3 b;
        for (float t = 0.0f; t < _HalfTime; t += Time.deltaTime)
        {
            float e = t / _HalfTime;
            b = Vector3.Lerp(_EndScale, _Scale, e);
            transform.localScale = Vector3.Lerp(b, _Scale, e);
            yield return null;
        }
        transform.localScale = _Scale;
    }
}
