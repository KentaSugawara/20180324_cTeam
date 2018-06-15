using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Main_AlbumViewer : MonoBehaviour {
    [SerializeField]
    private Main_DataFileManager _DataFileManager;

    [SerializeField]
    private GameObject _ScrollViewObject;

    [SerializeField]
    private Transform _ScrollViewContent;

    [SerializeField]
    private ContentSizeFitter _ContentSizeFitter;

    [SerializeField]
    private ScrollRect _ScrollView;

    [SerializeField]
    private Text _Text_NumOfPictures;

    [SerializeField]
    private GameObject _Prefab_Node;

    [SerializeField, Range(1, 30)]
    private int _MaxSyncLoadNum = 1;

    [Space(5), SerializeField]
    private Image _Image_GarbageBox;

    [SerializeField]
    private Button _Button_GarbageBox;

    [SerializeField]
    private Sprite _Sprite_GarbageBox_Open;

    [SerializeField]
    private Sprite _Sprite_GarbageBox_Close;

    private List<Main_AlbumViewerNode> _ScrollViewNodes = new List<Main_AlbumViewerNode>();

    private Vector3 _GarbageBoxInfo_ViewPosition;

    [SerializeField]
    private RectTransform _GarbageBoxInfo;

    private void Awake()
    {
        _GarbageBoxInfo_ViewPosition = _GarbageBoxInfo.anchoredPosition;
    }

    public void Init()
    {
        _ZoomImage.gameObject.SetActive(false);
        _ZoomImage.material = new Material(_ZoomImage.material);

        //ゴミ箱閉じる
        GarbageBoxClose();
        _GarbageBoxInfo.gameObject.SetActive(false);

        ClearListInstance();
        ListUpTextures();
    }

    public void SetActive(bool value)
    {
        GarbageBoxClose();
        _GarbageBoxInfo.gameObject.SetActive(false);
        _ScrollViewObject.SetActive(value);
    }

    public void ListUpTextures()
    {
        StartCoroutine(Routine_ListUpTextures());
    }

    [SerializeField]
    private Json_Album_DataList _AlbumDataList;

    private IEnumerator Routine_ListUpTextures()
    {
        var album = _DataFileManager.Load_AlbumData();
        _AlbumDataList = album;
        _Text_NumOfPictures.text = "0枚";

        int NumOfLoding = 0;
        int LoadingCompleted = 0;
        for (int i = 0, size = album.Pictures.Count; i < size; ++i)
        {
            while (NumOfLoding >= _MaxSyncLoadNum) yield return null;

            var obj = Instantiate(_Prefab_Node);
            obj.transform.SetParent(_ScrollViewContent, false);

            var component = obj.GetComponent<Main_AlbumViewerNode>();
            component.Init(this, album.Pictures[i]);
            _ScrollViewNodes.Add(component);
            ++NumOfLoding;
            _DataFileManager.InputAlbumSmallPicture(
                album.Pictures[i].FileName_Small,
                component.ImageCallBack,
                () => {--NumOfLoding; ++LoadingCompleted; _Text_NumOfPictures.text = LoadingCompleted + "枚"; });
        }

        _ContentSizeFitter.SetLayoutVertical();
        _ScrollView.verticalNormalizedPosition = 1.0f;
    }

    public void ClearListInstance()
    {
        foreach (var node in _ScrollViewNodes)
        {
            Destroy(node.gameObject);
        }
        _ScrollViewNodes.Clear();
    }

    private bool GarbageBoxActive = false;
    private List<Main_AlbumViewerNode> GarbageBoxSelectList = new List<Main_AlbumViewerNode>();

    [SerializeField]
    private Image _ZoomImage;

    private IEnumerator OpenZoomPictureRoutine = null;
    public void PictureButton(Main_AlbumViewerNode node)
    {
        if (GarbageBoxActive)
        {
            if (node.isGarbageBoxSelected)
            {
                node.GarbageBoxNonSelect();
                GarbageBoxSelectList.Remove(node);
            }
            else
            {
                node.GarbageBoxSelect();
                GarbageBoxSelectList.Add(node);
            }
        }
        else
        {
            if (OpenZoomPictureRoutine == null)
            {
                OpenZoomPictureRoutine = Routine_OpenZoomPicture(node.Data);
                StartCoroutine(OpenZoomPictureRoutine);
            }
        }
    }

    private IEnumerator Routine_OpenZoomPicture(Json_Album_Data_Picture Picture)
    {
        bool isLoading = true;
        _DataFileManager.InputAlbumPicture(Picture.FileName, SetZoomTexture, () => isLoading = false);
        while (isLoading) yield return null;

        _ZoomImage.gameObject.SetActive(true);
        OpenZoomPictureRoutine = null;
    }

    public void SetZoomTexture(Texture texture)
    {
        _ZoomImage.sprite = Sprite.Create((Texture2D)texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    public void CloseZoomPicture()
    {
        _ZoomImage.gameObject.SetActive(false);
    }

    public void GarbageBoxButton()
    {
        if (_GarbageBoxInfo_isMoving) return;

        if (!GarbageBoxActive)
        {
            GarbageBoxOpen();
        }
        else
        {
            //ゴミ箱閉じる
            GarbageBoxClose();
        }
    }

    public void GarbageBoxOpen()
    {
        //ゴミ箱開く
        GarbageBoxActive = true;
        _Image_GarbageBox.sprite = _Sprite_GarbageBox_Open;
        EventSystem.current.SetSelectedGameObject(gameObject);

        GarbageBoxInfoOpen();
    }

    public void GarbageBoxClose()
    {
        //ゴミ箱開く
        GarbageBoxActive = false;
        _Image_GarbageBox.sprite = _Sprite_GarbageBox_Close;
        foreach(var node in GarbageBoxSelectList)
        {
            node.GarbageBoxNonSelect();
        }

        GarbageBoxSelectList.Clear();
        EventSystem.current.SetSelectedGameObject(null);

        GarbageBoxInfoClose();
    }

    public void GarbageBoxDelete()
    {
        foreach (var node in GarbageBoxSelectList)
        {
            _ScrollViewNodes.Remove(node);
            _DataFileManager.Delete_AlbumPicture(node.Data);
            _AlbumDataList.Pictures.Remove(node.Data);
            Destroy(node.gameObject);
        }

        _DataFileManager.Save_AlbumData(_AlbumDataList);

        GarbageBoxClose();

        _Text_NumOfPictures.text = _ScrollViewNodes.Count + "枚";
    }

    /// <summary>
    /// 写真を追加する
    /// </summary>
    public void SnapShot(Texture2D texture)
    {
        var picture = new Json_Album_Data_Picture();
        picture.Year = System.DateTime.Now.Year;
        picture.Month = System.DateTime.Now.Month;
        picture.Day = System.DateTime.Now.Day;
        picture.Hour = System.DateTime.Now.Hour;
        picture.Minute = System.DateTime.Now.Minute;
        picture.Second = System.DateTime.Now.Second;

        string fileName
            = "Image"
            + picture.Year
            + picture.Month
            + picture.Day
            + picture.Hour
            + picture.Minute
            + picture.Second;

        picture.FileName = fileName + ".png";
        picture.FileName_Small = fileName + "_s.png";

        _DataFileManager.Output_AlbumPicturePNG(texture, picture);

        _DataFileManager.Save_NewAlbumPicture(picture);

        //picture.CharacterCloseIDs = 
    }

    [SerializeField]
    private float _GarbageBoxInfo_ToOpenNeedSeconds;

    [SerializeField]
    private float _GarbageBoxInfo_ToCloseNeedSeconds;

    private bool _GarbageBoxInfo_isMoving;

    private void GarbageBoxInfoOpen()
    {
        StartCoroutine(Routine_GarbageBoxInfoOpen());
    }

    private IEnumerator Routine_GarbageBoxInfoOpen()
    {
        var deltaSize = Vector2.Scale(_GarbageBoxInfo.sizeDelta, new Vector2(_GarbageBoxInfo.localScale.x, _GarbageBoxInfo.localScale.y));
        var HidePosition = new Vector3(_GarbageBoxInfo_ViewPosition.x, -deltaSize.y * 1.1f, _GarbageBoxInfo_ViewPosition.z);
        _GarbageBoxInfo.gameObject.SetActive(true);

        Vector3 b1;
        _GarbageBoxInfo.anchoredPosition = HidePosition;

        _GarbageBoxInfo_isMoving = true;
        yield return null;

        for (float t = 0.0f; t < _GarbageBoxInfo_ToOpenNeedSeconds; t += Time.deltaTime)
        {
            float e = t / _GarbageBoxInfo_ToOpenNeedSeconds;
            b1 = Vector3.Lerp(HidePosition, _GarbageBoxInfo_ViewPosition, e);
            _GarbageBoxInfo.anchoredPosition = Vector3.Lerp(b1, _GarbageBoxInfo_ViewPosition, e);

            yield return null;
        }
        _GarbageBoxInfo.anchoredPosition = _GarbageBoxInfo_ViewPosition;
        _GarbageBoxInfo_isMoving = false;
    }


    private void GarbageBoxInfoClose()
    {
        StartCoroutine(Routine_GarbageBoxInfoClose());
    }

    private IEnumerator Routine_GarbageBoxInfoClose()
    {
        var deltaSize = Vector2.Scale(_GarbageBoxInfo.sizeDelta, new Vector2(_GarbageBoxInfo.localScale.x, _GarbageBoxInfo.localScale.y));
        var HidePosition = new Vector3(_GarbageBoxInfo_ViewPosition.x, -deltaSize.y * 1.1f, _GarbageBoxInfo_ViewPosition.z);

        Vector3 b1;
        _GarbageBoxInfo.anchoredPosition = _GarbageBoxInfo_ViewPosition;

        _GarbageBoxInfo_isMoving = true;
        yield return null;

        for (float t = 0.0f; t < _GarbageBoxInfo_ToCloseNeedSeconds; t += Time.deltaTime)
        {
            float e = t / _GarbageBoxInfo_ToCloseNeedSeconds;
            b1 = Vector3.Lerp(_GarbageBoxInfo_ViewPosition, HidePosition, e);
            _GarbageBoxInfo.anchoredPosition = Vector3.Lerp(b1, HidePosition, e);

            yield return null;
        }
        _GarbageBoxInfo.anchoredPosition = HidePosition;
        _GarbageBoxInfo_isMoving = false;

        _GarbageBoxInfo.gameObject.SetActive(false);
    }
}
