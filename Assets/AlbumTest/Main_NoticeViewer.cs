using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main_NoticeViewer : MonoBehaviour {
    [SerializeField]
    private RectTransform _NoticeBackGround;

    [SerializeField]
    private Text _NoticeText;

    [SerializeField]
    private float _ToEnterSeconds;

    [SerializeField]
    private float _WaitSeconds;

    [SerializeField]
    private float _ToExitSeconds;

    private List<string> _Notices = new List<string>();

    private Vector3 _ViewPosition;

    private void Awake()
    {
        _ViewPosition = _NoticeBackGround.anchoredPosition;
        _NoticeBackGround.anchoredPosition = new Vector3(_ViewPosition.x, _NoticeBackGround.sizeDelta.x * 0.6f, _ViewPosition.z);
    }

    private void Start()
    {
        StartCoroutine(Routine_Main());
    }

    private IEnumerator Routine_Main()
    {
        while (true)
        {
            while (_Notices.Count <= 0) yield return null;

            _NoticeText.text = _Notices[0];

            yield return Routine_NoticeEnter();
            yield return new WaitForSeconds(_WaitSeconds);
            yield return Routine_NoticeExit();

            _Notices.RemoveAt(0);
        }
    }

    public void AddNotice(string str)
    {
        _Notices.Add(str);
    }

    private IEnumerator Routine_NoticeEnter()
    {
        var deltaSize = Vector2.Scale(_NoticeBackGround.sizeDelta, new Vector2(_NoticeBackGround.lossyScale.x, _NoticeBackGround.lossyScale.y));
        var HidePosition = new Vector3(_ViewPosition.x, _NoticeBackGround.sizeDelta.x * 0.6f, _ViewPosition.z);
        Vector3 b1;

        _NoticeBackGround.anchoredPosition = HidePosition;

        for (float t = 0.0f; t < _ToEnterSeconds; t += Time.deltaTime)
        {
            float e = t / _ToEnterSeconds;
            b1 = Vector3.Lerp(HidePosition, _ViewPosition, e);
            _NoticeBackGround.anchoredPosition = Vector3.Lerp(b1, _ViewPosition, e);

            yield return null;
        }
        _NoticeBackGround.anchoredPosition = _ViewPosition;
    }

    private IEnumerator Routine_NoticeExit()
    {
        var deltaSize = Vector2.Scale(_NoticeBackGround.sizeDelta, new Vector2(_NoticeBackGround.lossyScale.x, _NoticeBackGround.lossyScale.y));
        var HidePosition = new Vector3(_ViewPosition.x, _NoticeBackGround.sizeDelta.x * 0.6f, _ViewPosition.z);
        Vector3 b1;

        for (float t = 0.0f; t < _ToExitSeconds; t += Time.deltaTime)
        {
            float e = t / _ToEnterSeconds;
            b1 = Vector3.Lerp(_ViewPosition, HidePosition, e);
            _NoticeBackGround.anchoredPosition = Vector3.Lerp(_ViewPosition, b1, e);

            yield return null;
        }
        _NoticeBackGround.anchoredPosition = HidePosition;
    }
}
