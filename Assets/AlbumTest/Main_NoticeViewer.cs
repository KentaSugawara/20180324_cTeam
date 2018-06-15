using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main_NoticeViewer : MonoBehaviour {
    public enum eNoticeType
    {
        Challenge,
        Item
    }

    [SerializeField]
    private RectTransform _NoticeBackGround;

    [SerializeField]
    private Text _NoticeText;

    [SerializeField]
    private RectTransform _ItemNoticeBackGround;

    [SerializeField]
    private Text _ItemNoticeText;

    [SerializeField]
    private float _ToEnterSeconds;

    [SerializeField]
    private float _WaitSeconds;

    [SerializeField]
    private float _ToExitSeconds;

    private List<KeyValuePair<string, eNoticeType>> _Notices = new List<KeyValuePair<string, eNoticeType>>();

    private Vector3 _ViewPosition;

    private void Awake()
    {
        _ViewPosition = _NoticeBackGround.anchoredPosition;
        _NoticeBackGround.anchoredPosition = new Vector3(_ViewPosition.x, _NoticeBackGround.sizeDelta.x * 0.6f, _ViewPosition.z);
        _ItemNoticeBackGround.anchoredPosition = new Vector3(_ViewPosition.x, _NoticeBackGround.sizeDelta.x * 0.6f, _ViewPosition.z);
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

            if (_Notices[0].Value == eNoticeType.Challenge) _NoticeText.text = _Notices[0].Key;
            else if (_Notices[0].Value == eNoticeType.Item) _ItemNoticeText.text = _Notices[0].Key;

            yield return Routine_NoticeEnter(_Notices[0].Value);
            yield return new WaitForSeconds(_WaitSeconds);
            yield return Routine_NoticeExit(_Notices[0].Value);

            _Notices.RemoveAt(0);
        }
    }

    public void AddNotice(string str, eNoticeType NoticeType)
    {
        _Notices.Add(new KeyValuePair<string, eNoticeType>(str, NoticeType));
    }

    private IEnumerator Routine_NoticeEnter(eNoticeType NoticeType)
    {
        RectTransform background = null;
        if (NoticeType == eNoticeType.Challenge) background = _NoticeBackGround;
        else if (NoticeType == eNoticeType.Item) background = _ItemNoticeBackGround;

        var deltaSize = Vector2.Scale(background.sizeDelta, new Vector2(background.lossyScale.x, background.lossyScale.y));
        var HidePosition = new Vector3(_ViewPosition.x, background.sizeDelta.x * 0.6f, _ViewPosition.z);
        Vector3 b1;

        background.anchoredPosition = HidePosition;

        for (float t = 0.0f; t < _ToEnterSeconds; t += Time.deltaTime)
        {
            float e = t / _ToEnterSeconds;
            b1 = Vector3.Lerp(HidePosition, _ViewPosition, e);
            background.anchoredPosition = Vector3.Lerp(b1, _ViewPosition, e);

            yield return null;
        }
        background.anchoredPosition = _ViewPosition;
    }

    private IEnumerator Routine_NoticeExit(eNoticeType NoticeType)
    {
        RectTransform background = null;
        if (NoticeType == eNoticeType.Challenge) background = _NoticeBackGround;
        else if (NoticeType == eNoticeType.Item) background = _ItemNoticeBackGround;

        var deltaSize = Vector2.Scale(background.sizeDelta, new Vector2(background.lossyScale.x, background.lossyScale.y));
        var HidePosition = new Vector3(_ViewPosition.x, background.sizeDelta.x * 0.6f, _ViewPosition.z);
        Vector3 b1;

        for (float t = 0.0f; t < _ToExitSeconds; t += Time.deltaTime)
        {
            float e = t / _ToEnterSeconds;
            b1 = Vector3.Lerp(_ViewPosition, HidePosition, e);
            background.anchoredPosition = Vector3.Lerp(_ViewPosition, b1, e);

            yield return null;
        }
        background.anchoredPosition = HidePosition;
    }
}
