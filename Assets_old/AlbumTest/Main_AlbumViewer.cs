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

    public void Init()
    {
        _ZoomImage.gameObject.SetActive(false);
        _ZoomImage.material = new Material(_ZoomImage.material);

        //ゴミ箱閉じる
        GarbageBoxClose();
        _GarbageBoxInfo.SetActive(false);

        ClearListInstance();
        ListUpTextures();
    }

    public void SetActive(bool value)
    {
        GarbageBoxClose();
        _GarbageBoxInfo.SetActive(false);
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
        if (_GarbageBoxInfo.activeInHierarchy) return;

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

    [SerializeField]
    private GameObject _GarbageBoxInfo;

    public void GarbageBoxButton()
    {
        if (_GarbageBoxInfo.activeInHierarchy) return;

        if (!GarbageBoxActive)
        {
            GarbageBoxOpen();
        }
        else
        {
            //一つでも選択されていたら
            if (GarbageBoxSelectList.Count > 0)
            {
                _GarbageBoxInfo.SetActive(true);
            }
            else
            {
                //ゴミ箱閉じる
                GarbageBoxClose();
            }
        }
    }

    public void GarbageBoxOpen()
    {
        //ゴミ箱開く
        GarbageBoxActive = true;
        _Image_GarbageBox.sprite = _Sprite_GarbageBox_Open;
        EventSystem.current.SetSelectedGameObject(gameObject);
        
    }

    public void GarbageBoxClose()
    {
        //ゴミ箱開く
        GarbageBoxActive = false;
        _GarbageBoxInfo.SetActive(false);
        _Image_GarbageBox.sprite = _Sprite_GarbageBox_Close;
        foreach(var node in GarbageBoxSelectList)
        {
            node.GarbageBoxNonSelect();
        }

        GarbageBoxSelectList.Clear();
        EventSystem.current.SetSelectedGameObject(null);
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
}
