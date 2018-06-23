﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_FindEgg : TutorialMethod {
    [SerializeField]
    private Image _Image_Information;

    [SerializeField]
    private Text _Text_Information;

    [SerializeField]
    private string _String_Information;

    [SerializeField]
    private float _Seconds_ViewInformation;

    public override void Method(System.Action endcallback)
    {
        StartCoroutine(Routine_Find(endcallback));
    }

    private IEnumerator Routine_Find(System.Action endcallback)
    {
        yield return StartCoroutine(Routine_ViewInformation());

        while (_isActive)
        {
            Debug.Log("serching");
            if (EggSpawnerARCore.EggList.Count > 0)
            {
                var egg = EggSpawnerARCore.EggList[0].GetComponent<EggBehaviour>();
                if (egg != null)
                {
                    if (egg.isInCamera) break;
                }
            }
            yield return null;
        }

        yield return StartCoroutine(Routine_HideInformation());

        endcallback();
    }

    private IEnumerator Routine_ViewInformation()
    {
        Color start = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        Color end = Color.white;
        Color b;
        float e;
        _Image_Information.gameObject.SetActive(true);
        _Text_Information.text = _String_Information;
        for (float t = 0.0f; t < _Seconds_ViewInformation; t += Time.deltaTime)
        {
            e = t / _Seconds_ViewInformation;
            b = Color.Lerp(start, end, e);
            _Image_Information.color = Color.Lerp(b, end, e);
            yield return null;
        }
        _Image_Information.color = end;
    }

    private IEnumerator Routine_HideInformation()
    {
        Color start = Color.white;
        Color end = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        Color b;
        float e;
        for (float t = 0.0f; t < _Seconds_ViewInformation; t += Time.deltaTime)
        {
            e = t / _Seconds_ViewInformation;
            b = Color.Lerp(start, end, e);
            _Image_Information.color = Color.Lerp(b, end, e);
            yield return null;
        }
        _Image_Information.color = end;

        _Image_Information.gameObject.SetActive(false);
    }
}
