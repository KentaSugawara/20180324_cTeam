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
            StartCoroutine(Routine_Open());
        }
    }

    private IEnumerator Routine_Open()
    {
        isOpening = true;
        yield return new WaitForSeconds(_OpenDelaySeconds);

        foreach (var obj in _OtherUIObjects)
        {
            obj.SetActive(false);
        }

        _MainCamera.SetActive(false);
        _UICamera.SetActive(true);
        _PictureBookViewer.Init();
        _AlbumViewer.Init();
        UpdateView(_SelectTabIndex);
        isOpening = false;
    }

    public void Close()
    {
        foreach (var obj in _OtherUIObjects)
        {
            obj.SetActive(true);
        }

        _MainCamera.SetActive(true);
        _UICamera.SetActive(false);
        gameObject.SetActive(false);
    }

    public void SelectTab(int Index)
    {
        if (_SelectTabIndex == Index) return;
        _SelectTabIndex = Index;
        UpdateView(_SelectTabIndex);

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
