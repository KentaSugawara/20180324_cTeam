using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main_ItemViewer : MonoBehaviour {

    [SerializeField]
    private RectTransform _BackGround;

    [SerializeField]
    private ScrollRect _ScrollView;

    [SerializeField]
    private ContentSizeFitter _ContentSizeFitter;

    [SerializeField]
    private float _ToOpenNeedSeconds;

    private Vector3 _ViewPosition;

    private bool _isMoving = false;

    private void Awake()
    {
        _ViewPosition = _BackGround.anchoredPosition;
    }

    public void Open()
    {
        if (!_isMoving)
        {
            gameObject.SetActive(true);

            StopAllCoroutines();
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
        _BackGround.anchoredPosition = new Vector3(_BackGround.sizeDelta.x * 0.6f, _ViewPosition.y, _ViewPosition.z);
        yield return null;
        var deltaSize = Vector2.Scale(_BackGround.sizeDelta, new Vector2(_BackGround.lossyScale.x, _BackGround.lossyScale.y));
        var HidePosition = new Vector3(deltaSize.x * 0.6f, _ViewPosition.y, _ViewPosition.z);
        Vector3 b1;

        _isMoving = true;
        _BackGround.anchoredPosition = HidePosition;


        yield return null;
        _ContentSizeFitter.SetLayoutVertical();
        _ScrollView.verticalNormalizedPosition = 1.0f;

        for (float t = 0.0f; t < _ToOpenNeedSeconds; t += Time.deltaTime)
        {
            float e = t / _ToOpenNeedSeconds;
            b1 = Vector3.Lerp(HidePosition, _ViewPosition, e);
            _BackGround.anchoredPosition = Vector3.Lerp(b1, _ViewPosition, e);

            yield return null;
        }
        _BackGround.anchoredPosition = _ViewPosition;
        _isMoving = false;
    }

    private IEnumerator Routine_Close()
    {
        var deltaSize = Vector2.Scale(_BackGround.sizeDelta, new Vector2(_BackGround.lossyScale.x, _BackGround.lossyScale.y));
        var HidePosition = new Vector3(deltaSize.x * 0.6f, _ViewPosition.y, _ViewPosition.z);
        Vector3 b1;

        _isMoving = true;
        for (float t = 0.0f; t < _ToOpenNeedSeconds; t += Time.deltaTime)
        {
            float e = t / _ToOpenNeedSeconds;
            b1 = Vector3.Lerp(_ViewPosition, HidePosition, e);
            _BackGround.anchoredPosition = Vector3.Lerp(_ViewPosition, b1, e);

            yield return null;
        }
        _BackGround.anchoredPosition = HidePosition;
        gameObject.SetActive(false);
        _isMoving = false;
    }
}
