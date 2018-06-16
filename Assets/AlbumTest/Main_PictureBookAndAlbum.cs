using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main_PictureBookAndAlbum : MonoBehaviour {
    [SerializeField]
    private GameObject _MainCamera;

    [SerializeField]
    private GameObject _UICamera;

    [SerializeField]
    private List<GameObject> _OtherUIObjects;

    [SerializeField]
    private RectTransform _SelectImage;

    [SerializeField]
    private List<Vector3> _SelectImagePositions = new List<Vector3>();

    [SerializeField]
    private float _SelectImageMoveNeedSeconds;

    [SerializeField]
    private float _OpenDelaySeconds;

    [Space(5)]
    [SerializeField]
    private Main_PictureBookViewer _PictureBookViewer;

    [SerializeField]
    private Main_AlbumViewer _AlbumViewer;

    [SerializeField]
    private AudioSource _Audio_Open;

    [SerializeField]
    private AudioSource _Audio_Close;

    [SerializeField]
    private AudioSource _Audio_SelectTab;

    [SerializeField]
    private Main_BGM _Audio_BGM;

    private int _SelectTabIndex;

    private void Start()
    {
        _SelectTabIndex = 0;
        _SelectImage.localPosition = _SelectImagePositions[0];
    }

    bool isOpening;

    public void Open()
    {
        if (!isOpening)
        {
            gameObject.SetActive(true);
            _Audio_Open.Play();
            _Audio_BGM.BGM_In();
            StartCoroutine(Routine_Open());
        }
    }

    private IEnumerator Routine_Open()
    {
        isOpening = true;

        foreach (var obj in _OtherUIObjects)
        {
            obj.SetActive(false);
        }

        _MainCamera.GetComponent<Camera>().enabled = false;
        _MainCamera.GetComponent<AudioListener>().enabled = false;
        _UICamera.GetComponent<Camera>().enabled = true;
        _UICamera.GetComponent<AudioListener>().enabled = true;
        _PictureBookViewer.Init();
        _AlbumViewer.Init();
        UpdateView(_SelectTabIndex);
        isOpening = false;

        yield return null;
        //yield return new WaitForSeconds(_OpenDelaySeconds);
    }

    public void Close()
    {
        _Audio_BGM.BGM_Out();
        foreach (var obj in _OtherUIObjects)
        {
            obj.SetActive(true);
        }

        _PictureBookViewer.CloseWindowImmidiate();
        _Audio_Close.Play();
        _MainCamera.GetComponent<Camera>().enabled = true;
        _MainCamera.GetComponent<AudioListener>().enabled = true;
        _UICamera.GetComponent<Camera>().enabled = false;
        _UICamera.GetComponent<AudioListener>().enabled = false;
        gameObject.SetActive(false);
    }

    public void SelectTab(int Index)
    {
        if (_SelectTabIndex == Index) return;
        _SelectTabIndex = Index;
        UpdateView(_SelectTabIndex);
        _Audio_SelectTab.time = 0.2f;
        _Audio_SelectTab.Play();
        if (_SelectImageMoveRoutine != null) StopCoroutine(_SelectImageMoveRoutine);
        _SelectImageMoveRoutine = Routine_SelectImageMove(_SelectImagePositions[Index]);
        StartCoroutine(_SelectImageMoveRoutine);
    }

    private void UpdateView(int Index)
    {
        if (Index == 0)
        {
            _PictureBookViewer.SetActive(true);
            _AlbumViewer.SetActive(false);
        }
        else if (Index == 1)
        {
            _PictureBookViewer.SetActive(false);
            _AlbumViewer.SetActive(true);
        }
    }

    private IEnumerator _SelectImageMoveRoutine;
    private IEnumerator Routine_SelectImageMove(Vector3 EndPos)
    {
        Vector3 StartPos = _SelectImage.localPosition;

        Vector3 b;
        for (float t = 0.0f; t < _SelectImageMoveNeedSeconds; t += Time.deltaTime)
        {
            float e = t / _SelectImageMoveNeedSeconds;
            b = Vector3.Lerp(StartPos, EndPos, e);
            _SelectImage.localPosition = Vector3.Lerp(b, EndPos, e);
            yield return null;
        }
        _SelectImage.localPosition = EndPos;
    }
}
