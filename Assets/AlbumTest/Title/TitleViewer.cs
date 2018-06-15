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

    private Vector3 _ViewPosition1;
    private Vector3 _ViewPosition2;

    private void Awake()
    {
        _ViewPosition1 = _BackGround1.anchoredPosition;
        _ViewPosition2 = _BackGround2.anchoredPosition;
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
        _Help.SetActive(true);
    }

    public void HelpClose()
    {
        _Help.SetActive(false);
    }

    public void Open()
    {
        if (!_isMoving)
        {
            StopAllCoroutines();
            _StartButton.SetActive(false);
            StartCoroutine(Routine_Open());
        }
    }

    public void Close()
    {
        if (!_isMoving)
        {
            StopAllCoroutines();
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
}
