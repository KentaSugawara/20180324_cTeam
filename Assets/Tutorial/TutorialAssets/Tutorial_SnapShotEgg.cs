﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_SnapShotEgg : TutorialMethod {
    [SerializeField]
    private Main_Tutorial _Tutorial;

    [SerializeField]
    private Button _Button_SnapShot;

    [SerializeField]
    private Tutorial_ButtonDummy _ButtonDummy;

    [SerializeField]
    private Image _Image_Information;

    [SerializeField]
    private Text _Text_Information;

    [SerializeField]
    private string _String_Information;

    [SerializeField]
    private float _Seconds_Information;

    [SerializeField]
    private float _Seconds_DelayEnd;

    public override void Method(System.Action endcallback)
    {
        StartCoroutine(Routine_WaitSnapShot(endcallback));
    }

    public void CheckSnapShot(List<KeyValuePair<GameObject, SnapShotInfo>> SnapShots)
    {
        foreach (var egg in SnapShots)
        {
            if (egg.Value.CharaCloseIndex == 0)
            {
                _getSnapShot = true;
            }
        }
    }

    private bool _getSnapShot;
    private IEnumerator Routine_WaitSnapShot(System.Action endcallback)
    {
        _ButtonDummy.SetActive(true);

        _ButtonDummy.Init(_Button_SnapShot);

        yield return StartCoroutine(Routine_ViewInformation());
        _getSnapShot = false;
        while (_isActive)
        {
            if (_getSnapShot)
            {
                break;
            }
            yield return null;
        }
        _ButtonDummy.SetActive(false);

        yield return StartCoroutine(Routine_HideInformation());

        endcallback();
    }

    private IEnumerator Routine_ViewInformation()
    {
        Color start = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        Color end1 = Color.white;
        Color end2 = new Color(0.2f, 0.2f, 0.2f, 1.0f);
        Color b;
        float e;
        _Image_Information.gameObject.SetActive(true);
        _Text_Information.text = _String_Information;
        for (float t = 0.0f; t < _Seconds_Information; t += Time.deltaTime)
        {
            e = t / _Seconds_Information;
            b = Color.Lerp(start, end1, e);
            _Image_Information.color = Color.Lerp(b, end1, e);

            e = t / _Seconds_Information;
            b = Color.Lerp(start, end2, e);
            _Text_Information.color = Color.Lerp(b, end2, e);
            yield return null;
        }
        _Image_Information.color = end1;
        _Text_Information.color = end2;
    }

    private IEnumerator Routine_HideInformation()
    {
        Color start1 = Color.white;
        Color start2 = new Color(0.2f, 0.2f, 0.2f, 1.0f);
        Color end = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        Color b;
        float e;
        for (float t = 0.0f; t < _Seconds_Information; t += Time.deltaTime)
        {
            e = t / _Seconds_Information;
            b = Color.Lerp(start1, end, e);
            _Image_Information.color = Color.Lerp(b, end, e);

            e = t / _Seconds_Information;
            b = Color.Lerp(start2, end, e);
            _Text_Information.color = Color.Lerp(b, end, e);
            yield return null;
        }
        _Image_Information.color = end;
        _Text_Information.color = end;

        _Image_Information.gameObject.SetActive(false);
    }
}
