using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Main_Tutorial : MonoBehaviour {
    public enum eTamago
    {
        m1, m2, m3, m4, m5, m6
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
        public bool DisableBackGround;
        public bool DisableTapScreen;
        public UnityEvent _OnTap;
        public Button NextButton;
        public bool isCameraCanvas;
        public Camera TargetCamera;
        public TutorialMethod _TutorialMethod;
    }

    [SerializeField]
    private List<TutorialChild> _TutorialList = new List<TutorialChild>();
    public List<TutorialChild> TutorialList { get { return _TutorialList; } }

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
    private GameObject _TapScreen;

    [SerializeField]
    private RectTransform _Fukidashi;

    [SerializeField]
    private Tutorial_ButtonDummy _ButtonDummy;

    [SerializeField]
    private Text _Text_Fukidashi;

    [SerializeField, Space(5)]
    private float _Seconds_Fukidashi;

    [SerializeField]
    private float _Seconds_Fade;

    [SerializeField]
    private float _Seconds_HoleFade;

    [SerializeField]
    private float _Seconds_YubiView;

    [SerializeField]
    private float _Seconds_YubiHide;

    private Vector2 _Scale_Fukidashi;
    private Vector3 _TamagoPosition;
    private Vector3 _YubiScale;
    private Transform _OldButtonParent;
    private bool canTap;

    void Start () {
        _Scale_Fukidashi = _Fukidashi.localScale;
        _HoleView.material.SetFloat("_ScreenW", Screen.width);
        _HoleView.material.SetFloat("_ScreenH", Screen.height);

        _Fukidashi.localScale = Vector3.zero;

        _TamagoPosition = _Animator_Tamago.GetComponent<RectTransform>().anchoredPosition;
        _YubiScale = _Yubi.transform.localScale;

        _TamagoTransform.gameObject.SetActive(false);
        _BackGround.gameObject.SetActive(false);
        _ButtonDummy.SetActive(false);

        Next();
    }

    private IEnumerator Routine_Next()
    {
        _ButtonDummy.SetActive(false);
        _TapScreen.SetActive(!_TutorialList[_TutorialIndex].DisableTapScreen);

        var YubiActive = _TutorialList[_TutorialIndex].ActiveYubi || _TutorialList[_TutorialIndex].Type == eTutorialType.Button;
        StartCoroutine(Routine_Yubi(YubiActive));
        if (YubiActive)
        { 
            _Yubi.localPosition = _TutorialList[_TutorialIndex].YubiScreenPos;
        }

        var HoleViewActive = _TutorialList[_TutorialIndex].ActiveHoleView && _TutorialList[_TutorialIndex].Type != eTutorialType.Button;
        StartCoroutine(Routine_HoleView(HoleViewActive, _TutorialList[_TutorialIndex].HoleViewScreenPos, _TutorialList[_TutorialIndex].HoleViewSize));

        if (_TutorialList[_TutorialIndex].Type == eTutorialType.Button)
        {
            _Fukidashi.gameObject.SetActive(false);

            Vector2 pos;
            if (_TutorialList[_TutorialIndex].isCameraCanvas) pos = _TutorialList[_TutorialIndex].TargetCamera.WorldToScreenPoint(_TutorialList[_TutorialIndex].NextButton.transform.position);
            else pos = ((RectTransform)_TutorialList[_TutorialIndex].NextButton.transform).position;
            RectTransform r = (RectTransform)_TutorialList[_TutorialIndex].NextButton.transform;

            if (!_TutorialList[_TutorialIndex].ActiveYubi) _Yubi.position = pos;

            Vector2 holesize;
            if (_TutorialList[_TutorialIndex].isCameraCanvas) holesize = Vector2.Scale(r.sizeDelta, r.localScale);
            else holesize = Vector2.Scale(r.sizeDelta, r.lossyScale);
            holesize *= 1.2f;

            StartCoroutine(Routine_HoleView(_TutorialList[_TutorialIndex].ActiveHoleView, pos - holesize * 0.5f, holesize));

            yield return StartCoroutine(Routine_Fade(false, false));

            _ButtonDummy.SetActive(true);
            _BackGround.gameObject.SetActive(false);

            _ButtonDummy.Init(_TutorialList[_TutorialIndex].NextButton, _TutorialList[_TutorialIndex].isCameraCanvas, _TutorialList[_TutorialIndex].TargetCamera);
            //_OldButtonParent = _TutorialList[_TutorialIndex].NextButton.transform.parent;
            //_TutorialList[_TutorialIndex].NextButton.transform.SetParent(_CanvasTransform, false);

            _TutorialList[_TutorialIndex].NextButton.onClick.AddListener(OnClickButton);
        }
        else if (_TutorialList[_TutorialIndex].Type == eTutorialType.Tamago)
        {
            canTap = false;
            _Fukidashi.gameObject.SetActive(true);

            _Fukidashi.localScale = new Vector2(0, 0);

            yield return StartCoroutine(Routine_Fade(true, !_TutorialList[_TutorialIndex].DisableBackGround));

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

    [SerializeField]
    private int _TutorialIndex = -1;
    private bool NextMoving = false;
    private void Next()
    {
        if (NextMoving) return;

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

    private bool isHoleViewActive = false;
    private Vector2 _Old_HoleViewScreenPos;
    private Vector2 _Old_HoleViewSize;
    private IEnumerator Routine_HoleView(bool value, Vector2 HoleViewScreenPos, Vector2 HoleViewSize)
    {
        Vector2 Zero_plus = Vector2.zero - new Vector2(1.0f, 1.0f);
        Vector2 ScreenSize_plus = new Vector2(Screen.width, Screen.height) + new Vector2(1.0f, 1.0f);
        if (isHoleViewActive != value)
        {
            isHoleViewActive = value;
        }
        else
        {
            if (value)
            {
                //Zero_plus = _Old_HoleViewScreenPos;
                //ScreenSize_plus = _Old_HoleViewSize;

                yield return StartCoroutine(Routine_HoleView(false, HoleViewScreenPos, HoleViewSize));
                yield return StartCoroutine(Routine_HoleView(true, HoleViewScreenPos, HoleViewSize));
            }
            yield break;
        }

        if (value)
        {
            Vector2 HoleViewEnd = HoleViewScreenPos + HoleViewSize;

            _Old_HoleViewScreenPos = HoleViewScreenPos;
            _Old_HoleViewSize = HoleViewSize;

            _HoleView.gameObject.SetActive(true);

            Vector3 b;
            Vector2 holepos;
            Vector2 holeendpos;
            for (float t = 0.0f; t < _Seconds_HoleFade; t += Time.deltaTime)
            {
                float e = t / _Seconds_HoleFade;
                b = Vector2.Lerp(Zero_plus, HoleViewScreenPos, e);
                holepos = Vector2.Lerp(b, HoleViewScreenPos, e);

                b = Vector2.Lerp(ScreenSize_plus, HoleViewEnd, e);
                holeendpos = Vector2.Lerp(b, HoleViewEnd, e);

                SetHoleViewValue(holepos, holeendpos - holepos);

                yield return null;
            }

            SetHoleViewValue(HoleViewScreenPos, HoleViewSize);
        }
        else
        {
            HoleViewScreenPos = _Old_HoleViewScreenPos;
            HoleViewSize = _Old_HoleViewSize;
            Vector2 HoleViewEnd = HoleViewScreenPos + HoleViewSize;

            Vector3 b;
            Vector2 holepos;
            Vector2 holeendpos;
            for (float t = 0.0f; t < _Seconds_HoleFade; t += Time.deltaTime)
            {
                float e = t / _Seconds_HoleFade;
                b = Vector2.Lerp(HoleViewScreenPos, Zero_plus, e);
                holepos = Vector2.Lerp(b, Zero_plus, e);

                b = Vector2.Lerp(HoleViewEnd, ScreenSize_plus, e);
                holeendpos = Vector2.Lerp(b, ScreenSize_plus, e);

                SetHoleViewValue(holepos, holeendpos - holepos);

                yield return null;
            }

            SetHoleViewValue(HoleViewScreenPos, HoleViewSize);

            _HoleView.gameObject.SetActive(false);
        }
    }

    private void SetHoleViewValue(Vector2 holepos, Vector2 holesize)
    {
        _HoleView.material.SetFloat("_HoleX", holepos.x);
        _HoleView.material.SetFloat("_HoleY", holepos.y);

        _HoleView.material.SetFloat("_Width", holesize.x);
        _HoleView.material.SetFloat("_Height", holesize.y);
    }

    private bool isYubiActive = false;
    private IEnumerator Routine_Yubi(bool value)
    {
        if (isYubiActive != value)
        {
            isYubiActive = value;
        }
        else
        {
            if (value)
            {
                yield return StartCoroutine(Routine_Yubi(false));
                yield return StartCoroutine(Routine_Yubi(true));
            }
            yield break;
        }

        if (value)
        {
            _Yubi.gameObject.SetActive(true);

            Vector3 b;
            for (float t = 0.0f; t < _Seconds_YubiView; t += Time.deltaTime)
            {
                float e = t / _Seconds_YubiView;
                b = Vector3.Lerp(Vector3.zero, _YubiScale, e);
                _Yubi.localScale = Vector2.Lerp(b, _YubiScale, e);

                yield return null;
            }
            _Yubi.localScale = _YubiScale;
        }
        else
        {
            Vector3 b;
            for (float t = 0.0f; t < _Seconds_YubiHide; t += Time.deltaTime)
            {
                float e = t / _Seconds_YubiHide;
                b = Vector3.Lerp(_YubiScale, Vector3.zero, e);
                _Yubi.localScale = Vector2.Lerp(b, Vector3.zero, e);

                yield return null;
            }
            _Yubi.localScale = Vector3.zero;

            _Yubi.gameObject.SetActive(false);
        }
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
