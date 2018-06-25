using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_SetItem : TutorialMethod {
    [SerializeField]
    private Main_ItemViewer _ItemViewer;

    [SerializeField]
    private GameObject _Yubi;

    [SerializeField]
    private AnimationCurve _Curve_Yubi;

    [SerializeField]
    private Tutorial_EventTriggerDummy _EventTriggerDummy;

    [SerializeField]
    private float _Seconds_YubiHide;

    public override void Method(System.Action endcallback)
    {
        StartCoroutine(Routine_Find(endcallback));
        StartCoroutine(Routine_Yubi());
    }

    private IEnumerator Routine_Find(System.Action endcallback)
    {
        _ItemViewer.StopClose = true;

        var eventtrigger = _ItemViewer.ScrollViewNodes[0].myEventTrigger;

        _EventTriggerDummy.SetActive(true);

        _EventTriggerDummy.Init(eventtrigger);

        while (_isActive)
        {
            eventtrigger = _ItemViewer.ScrollViewNodes[0].myEventTrigger;
            _EventTriggerDummy.Init(eventtrigger);
            if (_ItemViewer.CurrentItemInstance != null)
            {
                break;
            }
            yield return null;
        }

        yield return StartCoroutine(Routine_HideYubi());

        //アイテム置いたら
        _ItemViewer.Close();

        _ItemViewer.StopClose = false;

        endcallback();
    }

    [SerializeField]
    private Vector3 _Yubi_StartPos;

    [SerializeField]
    private Vector3 _Yubi_EndPos;

    private IEnumerator Routine_Yubi()
    {
        _Yubi.SetActive(true);
        while (true)
        {
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime)
            {
                _Yubi.transform.position = Vector3.Lerp(_Yubi_StartPos, _Yubi_EndPos, _Curve_Yubi.Evaluate(t));
                yield return null;
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    private IEnumerator Routine_HideYubi()
    {
        Vector3 scale = _Yubi.transform.localScale;
        Vector3 b;
        for (float t = 0.0f; t < _Seconds_YubiHide; t += Time.deltaTime)
        {
            float e = t / _Seconds_YubiHide;
            b = Vector3.Lerp(scale, Vector3.zero, e);
            _Yubi.transform.localScale = Vector2.Lerp(b, Vector3.zero, e);

            yield return null;
        }
        _Yubi.transform.localScale = Vector3.zero;

        _Yubi.gameObject.SetActive(false);
    }
}
