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
        Tamago, Button, Find, Take1, Take2
    }

    [System.Serializable]
    public class TutorialChild
    {
        public eTutorialType Type;
        public eTamago Tamago;
        [Multiline(3)]
        public string text;
        public UnityEvent _OnTap;
        public Button NextButton;
    }

    [SerializeField]
    private List<TutorialChild> _TutorialList = new List<TutorialChild>();

    [SerializeField, Space(5)]
    private Animator _Animator_Tamago;

    [SerializeField]
    private GameObject _BackGround;

    [SerializeField]
    private Transform _Yubi;

    [SerializeField]
    private Image _HoleView;

    [SerializeField]
    private RectTransform _Fukidashi;

    [SerializeField]
    private Text _Text_Fukidashi;

    [SerializeField, Space(5)]
    private float _Seconds_Fukidashi;

    private Vector2 _Scale_Fukidashi;

    void Start () {
        _Scale_Fukidashi = _Fukidashi.localScale;
        _HoleView.material.SetFloat("_ScreenW", Screen.width);
        _HoleView.material.SetFloat("_ScreenH", Screen.height);

        Next();
    }

    private IEnumerator Routine_Next()
    {
        if (_TutorialList[_TutorialIndex].Type == eTutorialType.Button)
        {
            _Fukidashi.gameObject.SetActive(false);
            _BackGround.SetActive(false);
            _HoleView.gameObject.SetActive(true);
            _Yubi.gameObject.SetActive(true);

            Vector3 pos = _TutorialList[_TutorialIndex].NextButton.transform.position;
            RectTransform r = (RectTransform)_TutorialList[_TutorialIndex].NextButton.transform;

            Vector2 holesize = Vector2.Scale(r.sizeDelta, r.lossyScale);
            holesize *= 1.2f;

            _HoleView.material.SetFloat("_HoleX", pos.x - holesize.x * 0.5f);
            _HoleView.material.SetFloat("_HoleY", pos.y - holesize.y * 0.5f);

            _HoleView.material.SetFloat("_Width", holesize.x);
            _HoleView.material.SetFloat("_Height", holesize.y);

            _Yubi.position = pos;
        }
        else if (_TutorialList[_TutorialIndex].Type == eTutorialType.Tamago)
        {
            _Fukidashi.gameObject.SetActive(true);
            _BackGround.SetActive(true);
            _HoleView.gameObject.SetActive(false);
            _Yubi.gameObject.SetActive(false);

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
        else
        {
            _Fukidashi.gameObject.SetActive(true);
            _BackGround.SetActive(true);
            _HoleView.gameObject.SetActive(false);
            _Yubi.gameObject.SetActive(false);

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
    }

    private int _TutorialIndex = -1;
    private void Next()
    {
        if (_TutorialList.Count > _TutorialIndex + 1)
        {
            ++_TutorialIndex;
            _TutorialList[_TutorialIndex]._OnTap.Invoke();
            _Text_Fukidashi.text = _TutorialList[_TutorialIndex].text;
            _Animator_Tamago.SetTrigger(_TutorialList[_TutorialIndex].Tamago.ToString());
        }

        StopAllCoroutines();
        StartCoroutine(Routine_Next());
    }

    public void OnTap()
    {
        if (_TutorialList[_TutorialIndex].Type == eTutorialType.Tamago)
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
}
