using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_PictureBookNew : MonoBehaviour {
    [SerializeField]
    private RectTransform _New;

    [SerializeField]
    private float _OpenNeedSeconds;

    [SerializeField]
    private float _ViewSeconds;

    [SerializeField]
    private float _HideNeedSeconds;

    IEnumerator RoutineNew;

    public void HideNew()
    {
        if (RoutineNew != null) StopCoroutine(RoutineNew);
        _New.localScale = Vector3.zero;
        _New.gameObject.SetActive(false);
    }

    public void ViewNew()
    {
        if (RoutineNew != null) StopCoroutine(RoutineNew);
        RoutineNew = Routine_New();
        _New.gameObject.SetActive(true);
        StartCoroutine(RoutineNew);
    }

    private IEnumerator Routine_New()
    {
        _New.localScale = Vector3.zero;
        Vector3 b;
        for (float t = 0.0f; t < _OpenNeedSeconds; t += Time.deltaTime)
        {
            float e = t / _OpenNeedSeconds;
            b = Vector3.Lerp(Vector3.zero, Vector3.one, e);
            _New.localScale = Vector3.Lerp(b, Vector3.one, e);
            yield return null;
        }
        _New.localScale = Vector3.one;

        yield return new WaitForSeconds(_ViewSeconds);

        for (float t = 0.0f; t < _HideNeedSeconds; t += Time.deltaTime)
        {
            float e = t / _HideNeedSeconds;
            b = Vector3.Lerp(Vector3.one, Vector3.zero, e);
            _New.localScale = Vector3.Lerp(b, Vector3.zero, e);
            yield return null;
        }
        _New.localScale = Vector3.zero;
        _New.gameObject.SetActive(true);
    }
}
