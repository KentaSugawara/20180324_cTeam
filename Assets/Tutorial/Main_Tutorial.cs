using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Main_Tutorial : MonoBehaviour {
    public enum eTamago
    {
        m1,m2,m3,m4,m5,m6
    }

    public enum eTutorialType
    {
        Tamago, Button, Method
    }

    [System.Serializable]
    public class TutorialChild
    {
        public eTutorialType Type;
        public eTamago Tamago;
        [Multiline(3)]
        public string text;
        public bool ChangeFontSize;
        public int FontSize;
        public bool ActiveYubi;
        public Vector3 YubiScreenPos;
        public bool ActiveHoleView;
        public Vector2 HoleViewScreenPos;
        public Vector2 HoleViewSize;
        public UnityEvent _OnTap;
        public Button NextButton;
        public TutorialMethod _TutorialMethod;
    }

    [SerializeField]
    private List<TutorialChild> _TutorialList = new List<TutorialChild>();

    [SerializeField, Space(5)]
    private Animator _Animator_Tamago;

    [SerializeField]
    private RectTransform _TamagoTransform;

    [SerializeField]
    private Image _BackGround;

    [SerializeField]
    private Transform _Yubi;

    [SerializeField]
    private Transform _CanvasTransform;

    [SerializeField]
    private Image _HoleView;

    [SerializeField]
    private RectTransform _Fukidashi;

    [SerializeField]
    private Text _Text_Fukidashi;

    [SerializeField, Space(5)]
    private float _Seconds_Fukidashi;

    [SerializeField]
    private float _Seconds_Fade;

    private Vector2 _Scale_Fukidashi;
    private Vector3 _TamagoPosition;
    private Transform _OldButtonParent;
    private bool canTap;

    void Start () {
        _Scale_Fukidashi = _Fukidashi.localScale;
        _HoleView.material.SetFloat("_ScreenW", Screen.width);
        _HoleView.material.SetFloat("_ScreenH", Screen.height);

        _Fukidashi.localScale = Vector3.zero;

        _TamagoPosition = _Animator_Tamago.GetComponent<RectTransform>().anchoredPosition;

        _TamagoTransform.gameObject.SetActive(false);
        _BackGround.gameObject.SetActive(false);

        Next();
    }

    private IEnumerator Routine_Next()
    {
        var YubiActive = _TutorialList[_TutorialIndex].ActiveYubi;
        _Yubi.gameObject.SetActive(YubiActive);
        if (YubiActive)
        {
            _Yubi.position = _TutorialList[_TutorialIndex].YubiScreenPos;
        }

        var HoleViewActive = _TutorialList[_TutorialIndex].ActiveHoleView;
        _HoleView.gameObject.SetActive(HoleViewActive);
        if (HoleViewActive)
        {
            _HoleView.material.SetFloat("_HoleX", _TutorialList[_TutorialIndex].HoleViewScreenPos.x);
            _HoleView.material.SetFloat("_HoleY", _TutorialList[_TutorialIndex].HoleViewScreenPos.y);

            _HoleView.material.SetFloat("_Width", _TutorialList[_TutorialIndex].HoleViewSize.x);
            _HoleView.material.SetFloat("_Height", _TutorialList[_TutorialIndex].HoleViewSize.y);
        }

        if (_TutorialList[_TutorialIndex].Type == eTutorialType.Button)
        {
            yield return StartCoroutine(Routine_Fade(false, false));

            _Fukidashi.gameObject.SetActive(false);
            _BackGround.gameObject.SetActive(false);
            _Animator_Tamago.gameObject.SetActive(false);

            Vector3 pos = _TutorialList[_TutorialIndex].NextButton.transform.position;
            RectTransform r = (RectTransform)_TutorialList[_TutorialIndex].NextButton.transform;

            Vector2 holesize = Vector2.Scale(r.sizeDelta, r.lossyScale);
            holesize *= 1.2f;

            _HoleView.material.SetFloat("_HoleX", pos.x - holesize.x * 0.5f);
            _HoleView.material.SetFloat("_HoleY", pos.y - holesize.y * 0.5f);

            _HoleView.material.SetFloat("_Width", holesize.x);
            _HoleView.material.SetFloat("_Height", holesize.y);

            _Yubi.position = pos;

            _OldButtonParent = _TutorialList[_TutorialIndex].NextButton.transform.parent;
            _TutorialList[_TutorialIndex].NextButton.transform.parent = _CanvasTransform;

            _TutorialList[_TutorialIndex].NextButton.onClick.AddListener(OnClickButton);
        }
        else if (_TutorialList[_TutorialIndex].Type == eTutorialType.Tamago)
        {
            canTap = false;
            _Fukidashi.gameObject.SetActive(true);

            _Fukidashi.localScale = new Vector2(0, 0);

            yield return StartCoroutine(Routine_Fade(true, true));

            //ふきだし
            {
                for (float t = 0.0f; t < _Seconds_Fukidashi; t += Time.deltaTime)
                {
                    var b = Vector2.Lerp(Vector2.zero, _Scale_Fukidashi, t / _Seconds_Fukidashi);
                    _Fukidashi.localScale = Vector2.Lerp(b, _Scale_Fukidashi, t / _Seconds_Fukidashi);
                    yield return null;
                }
                _Fukidashi.localScale = _Scale_Fukidashi;
            }
            canTap = true;
        }
        else if (_TutorialList[_TutorialIndex].Type == eTutorialType.Method)
        {
            _Fukidashi.gameObject.SetActive(false);

            yield return StartCoroutine(Routine_Fade(false, false));

            bool isMoving = true;
            _TutorialList[_TutorialIndex]._TutorialMethod.Method(() => isMoving = false);
            while (isMoving) yield return null;

            NextMoving = false;
            Next();
        }
        else 
        {
            _Fukidashi.gameObject.SetActive(true);

            yield return StartCoroutine(Routine_Fade(false, false));

            //ふきだし
            {
                _Fukidashi.localScale = new Vector2(0, 0);

                for (float t = 0.0f; t < _Seconds_Fukidashi; t += Time.deltaTime)
                {
                    var b = Vector2.Lerp(Vector2.zero, _Scale_Fukidashi, t / _Seconds_Fukidashi);
                    _Fukidashi.localScale = Vector2.Lerp(b, _Scale_Fukidashi, t / _Seconds_Fukidashi);
                    yield return null;
                }
                _Fukidashi.localScale = _Scale_Fukidashi;
            }
        }
        NextMoving = false;
    }

    private int _TutorialIndex = -1;
    private bool NextMoving = false;
    private void Next()
    {
        if (NextMoving) return;

        if (_OldButtonParent != null)
        {
            _TutorialList[_TutorialIndex].NextButton.transform.parent = _OldButtonParent;
            _OldButtonParent = null;
        }

        if (_TutorialList.Count > _TutorialIndex + 1)
        {
            ++_TutorialIndex;
            _TutorialList[_TutorialIndex]._OnTap.Invoke();
            if (_TutorialList[_TutorialIndex].ChangeFontSize) _Text_Fukidashi.fontSize = _TutorialList[_TutorialIndex].FontSize;
            _Text_Fukidashi.text = _TutorialList[_TutorialIndex].text;
            _Animator_Tamago.SetTrigger(_TutorialList[_TutorialIndex].Tamago.ToString());
        }

        StopAllCoroutines();
        NextMoving = true;
        StartCoroutine(Routine_Next());
    }

    private bool _Active_Tamago = false;
    private bool _Active_Backgournd = false;

    private IEnumerator Routine_Fade(bool tamago, bool background)
    {
        int cnt = 0;
        if (_Active_Tamago != tamago)
        {
            ++cnt;
            _Active_Tamago = tamago;
            StartCoroutine(Routine_Fade_Tamago(tamago, () => --cnt));
        }
        if (_Active_Backgournd != background)
        {
            ++cnt;
            _Active_Backgournd = background;
            StartCoroutine(Routine_Fade_Background(background, () => --cnt));
        }
        while(cnt > 0) yield return null;
    }

    private IEnumerator Routine_Fade_Tamago(bool tamago, System.Action endcallback)
    {
        var deltaSize = Vector2.Scale(_TamagoTransform.sizeDelta, new Vector2(_TamagoTransform.lossyScale.x, _TamagoTransform.lossyScale.y));
        var HidePosition = new Vector3(_TamagoPosition.x, -deltaSize.y * 0.6f, _TamagoPosition.z);
        Vector3 StartPos, EndPos;
        if (tamago)
        {
            _TamagoTransform.gameObject.SetActive(true);
            StartPos = HidePosition;
            EndPos = _TamagoPosition;
        }
        else
        {
            StartPos = _TamagoPosition;
            EndPos = HidePosition;
        }

        Vector3 b;
        for (float t = 0.0f; t < _Seconds_Fade; t += Time.deltaTime)
        {
            float e = t / _Seconds_Fade;
            b = Vector3.Lerp(StartPos, EndPos, e);
            _TamagoTransform.anchoredPosition = Vector3.Lerp(b, EndPos, e);

            yield return null;
        }
        _TamagoTransform.anchoredPosition = EndPos;

        if (!tamago) _TamagoTransform.gameObject.SetActive(false);

        endcallback();
    }

    private IEnumerator Routine_Fade_Background(bool background, System.Action endcallback)
    {
        Color StartColor = _BackGround.color;
        Color EndColor = _BackGround.color;
        if (background)
        {
            _BackGround.gameObject.SetActive(true);
            StartColor.a = 0.0f;
            EndColor.a = 0.588f;
        }
        else
        {
            StartColor.a = 0.588f;
            EndColor.a = 0.0f;
        }

        Color b;
        for (float t = 0.0f; t < _Seconds_Fade; t += Time.deltaTime)
        {
            float e = t / _Seconds_Fade;
            b = Color.Lerp(StartColor, EndColor, e);
            _BackGround.color = Color.Lerp(b, EndColor, e);

            yield return null;
        }
        _BackGround.color = EndColor;

        if (!background) _BackGround.gameObject.SetActive(false);

        endcallback();
    }

    public void OnTap()
    {
        if (_TutorialList[_TutorialIndex].Type == eTutorialType.Tamago && canTap)
        {
            Next();
        }
    }

    public void OnClickButton()
    {
        if (_TutorialList[_TutorialIndex].Type == eTutorialType.Button)
        {
            Next();
        }
    }

    public void OnTap(Button button)
    {
        if (_TutorialList[_TutorialIndex].NextButton == button)
        {
            Next();
        }
    }

    public void DeleteIndex(int index)
    {
        _TutorialList.RemoveAt(index);
    }

    public void InsertIndex(int index)
    {
        _TutorialList.Insert(index, new TutorialChild());
    }
}
