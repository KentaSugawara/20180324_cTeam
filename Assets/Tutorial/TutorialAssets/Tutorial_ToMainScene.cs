using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tutorial_ToMainScene : TutorialMethod {
    [SerializeField]
    private Main_DataFileManager _DataFileManager;

    [SerializeField]
    private string _NextSceneName;

    [SerializeField]
    private float _Seconds_FadeIn;

    [SerializeField]
    private Image _FadeImage;

    private bool _isMoving = false;

    public override void Method(Action endcallback)
    {
        NextScene();
    }

    public void NextScene()
    {
        if (!_isMoving)
        {
            StartCoroutine(Routine_NextScene());
        }  
    }

    private IEnumerator Routine_NextScene()
    {
        _isMoving = true;

        yield return StartCoroutine(Routine_FadeIn());

        var savedata = new Json_SaveData();
        savedata.isAlreadyTutorial = true;

        _DataFileManager.Save_SaveData(savedata);

        SceneManager.LoadScene(_NextSceneName);
    }

    private IEnumerator Routine_FadeIn()
    {
        _FadeImage.color = Color.black;
        var endcolor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        Color b;
        for (float t = 0.0f; t < _Seconds_FadeIn; t += Time.deltaTime)
        {
            float e = t / _Seconds_FadeIn;
            b = Color.Lerp(Color.black, endcolor, e);
            _FadeImage.color = Color.Lerp(Color.black, b, e);
            yield return null;
        }
        _FadeImage.gameObject.SetActive(false);
    }
}
