using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main_MainRoutine : MonoBehaviour {
    [SerializeField]
    private Main_DataFileManager _DataFileManager;

    [SerializeField]
    private Assets_ChallengeList _ChallengeList;

    [SerializeField]
    private Main_ChallengeViewer _ChallengeViewer;

    [SerializeField]
    private Assets_ItemList _ItemList;

    [SerializeField]
    private Main_ItemViewer _ItemViewer;

    [SerializeField]
    private Main_PictureBookViewer _PictureBookViewer;

    [SerializeField]
    private Assets_CharacterList _CharacterList;

    [SerializeField]
    private Main_NoticeViewer _NoticeViewer;

    [SerializeField]
    private bool _isMainScene;

    [SerializeField]
    private float _Seconds_FadeIn;

    [SerializeField]
    private Image _FadeImage;

    private void Awake()
    {
        _FadeImage.gameObject.SetActive(true);
        _FadeImage.color = Color.black;
        if (_isMainScene)
        {
            var savedata = _DataFileManager.Load_SaveData();
			Debug.Log(savedata);
            if (savedata.isAlreadyTutorial == false)
            {
                SceneManager.LoadScene("Tutorial");
            }
        }

        Main_ChallengeManager.Init(_DataFileManager, _ChallengeViewer, _ChallengeList, _NoticeViewer);
        Main_ItemManager.Init(_DataFileManager, _ItemViewer, _ItemList);
        Main_PictureBookManager.Init(_DataFileManager, _PictureBookViewer, _CharacterList);
    }

    private void Start()
    {
        StartCoroutine(Routine_FadeIn());
    }

    private IEnumerator Routine_FadeIn()
    {
        _FadeImage.color = Color.black;
        var endcolor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        Color b;
        for (float t= 0.0f; t < _Seconds_FadeIn; t += Time.deltaTime)
        {
            float e = t / _Seconds_FadeIn;
            b = Color.Lerp(Color.black, endcolor, e);
            _FadeImage.color = Color.Lerp(Color.black, b, e);
            yield return null;
        }
        _FadeImage.gameObject.SetActive(false);
    }
}
