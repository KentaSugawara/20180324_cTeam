using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleViewer : MonoBehaviour {
    [SerializeField]
    private GameObject _Help;

    [SerializeField]
    private ScrollRect _Help_ScrollView;

    [SerializeField]
    private GameObject _StartButton;

    [SerializeField]
    private RectTransform _BackGround1;

    [SerializeField]
    private RectTransform _BackGround2;

    [SerializeField]
    private float _ToOpenNeedSeconds;

    [SerializeField]
    private AudioSource _Audio_Start;

    [SerializeField]
    private AudioSource _Audio_Help;

    [SerializeField]
    private AudioSource _Audio_HelpClose;

    [SerializeField]
    private AudioSource _Audio_BGM;

    private Vector3 _ViewPosition1;
    private Vector3 _ViewPosition2;

    private void Awake()
    {
        _ViewPosition1 = _BackGround1.anchoredPosition;
        _ViewPosition2 = _BackGround2.anchoredPosition;
    }

    private void Start()
    {
        _Audio_BGM.Play();
        StartCoroutine(Routine_BGM_In());
    }

    private bool _isMoving = false;

    public void HelpOpenOrClose()
    {
        if (_Help.activeInHierarchy) HelpClose();
        else HelpOpen();
    }

    public void HelpOpen()
    {
        _Help_ScrollView.verticalNormalizedPosition = 1.0f;
        _Audio_Help.time = 0.5f;
        _Audio_Help.Play();
        
        _Help.SetActive(true);
    }

    public void HelpClose()
    {
        _Audio_HelpClose.time = 0.2f;
        _Audio_HelpClose.Play();
        _Help.SetActive(false);
    }

    public void Open()
    {
        if (!_isMoving)
        {
            StopAllCoroutines();
            _Audio_Start.Play();
            _StartButton.SetActive(false);
            BGM_Out();
            StartCoroutine(Routine_Open());
        }
    }

    public void Close()
    {
        if (!_isMoving)
        {
            StopAllCoroutines();
            BGM_In();
            StartCoroutine(Routine_Close());
        }
    }

    private IEnumerator Routine_Open()
    {
        var deltaSize1 = Vector2.Scale(_BackGround1.sizeDelta, new Vector2(_BackGround1.lossyScale.x, _BackGround1.lossyScale.y));
        var HidePosition1 = new Vector3(_ViewPosition1.x, deltaSize1.y * 0.6f, _ViewPosition1.z);

        var deltaSize2 = Vector2.Scale(_BackGround2.sizeDelta, new Vector2(_BackGround2.lossyScale.x, _BackGround2.lossyScale.y));
        var HidePosition2 = new Vector3(_ViewPosition2.x, -deltaSize2.y * 0.6f, _ViewPosition2.z);
        Vector3 b;

        _isMoving = true;

        for (float t = 0.0f; t < _ToOpenNeedSeconds; t += Time.deltaTime)
        {
            float e = t / _ToOpenNeedSeconds;
            b = Vector3.Lerp(_ViewPosition1, HidePosition1, e);
            _BackGround1.anchoredPosition = Vector3.Lerp(b, HidePosition1, e);

            b = Vector3.Lerp(_ViewPosition2, HidePosition2, e);
            _BackGround2.anchoredPosition = Vector3.Lerp(b, HidePosition2, e);

            yield return null;
        }
        _BackGround1.anchoredPosition = HidePosition1;
        _BackGround2.anchoredPosition = HidePosition2;
        _isMoving = false;
    }

    private IEnumerator Routine_Close()
    {
        var deltaSize1 = Vector2.Scale(_BackGround1.sizeDelta, new Vector2(_BackGround1.lossyScale.x, _BackGround1.lossyScale.y));
        var HidePosition1 = new Vector3(_ViewPosition1.x, deltaSize1.y * 0.6f, _ViewPosition1.z);

        var deltaSize2 = Vector2.Scale(_BackGround2.sizeDelta, new Vector2(_BackGround2.lossyScale.x, _BackGround2.lossyScale.y));
        var HidePosition2 = new Vector3(_ViewPosition2.x, -deltaSize2.y * 0.6f, _ViewPosition2.z);
        Vector3 b;

        _isMoving = true;

        for (float t = 0.0f; t < _ToOpenNeedSeconds; t += Time.deltaTime)
        {
            float e = t / _ToOpenNeedSeconds;
            b = Vector3.Lerp(HidePosition1, _ViewPosition1, e);
            _BackGround1.anchoredPosition = Vector3.Lerp(b, _ViewPosition1, e);

            b = Vector3.Lerp(HidePosition2, _ViewPosition2, e);
            _BackGround2.anchoredPosition = Vector3.Lerp(b, _ViewPosition2, e);

            yield return null;
        }
        _BackGround1.anchoredPosition = _ViewPosition1;
        _BackGround2.anchoredPosition = _ViewPosition2;
        _isMoving = false;

        _StartButton.SetActive(true);
    }

    IEnumerator BGMRoutine;

    public void BGM_In()
    {
        if (BGMRoutine != null) StopCoroutine(BGMRoutine);
        BGMRoutine = Routine_BGM_In();
        StartCoroutine(BGMRoutine);
    }

    public void BGM_Out()
    {
        if (BGMRoutine != null) StopCoroutine(BGMRoutine);
        BGMRoutine = Routine_BGM_Out();
        StartCoroutine(Routine_BGM_Out());
    }

    private IEnumerator Routine_BGM_In()
    {
        for (float t = 0.0f; t < 2.0f; t+= Time.deltaTime)
        {
            _Audio_BGM.volume = Mathf.Lerp(0.0f, 1.0f, t / 2.0f);
            yield return null;
        }
    }

    private IEnumerator Routine_BGM_Out()
    {
        for (float t = 0.0f; t < 2.0f; t += Time.deltaTime)
        {
            _Audio_BGM.volume = Mathf.Lerp(1.0f, 0.0f, t / 2.0f);
            yield return null;
        }
    }
}
