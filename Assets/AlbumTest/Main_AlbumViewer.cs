using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main_AlbumViewer : MonoBehaviour {
    [SerializeField]
    private Main_DataFileManager _DataFileManager;

    [SerializeField]
    private GameObject _ScrollViewObject;

    [SerializeField]
    private Transform _ScrollViewContent;

    [SerializeField]
    private GameObject _Prefab_Node;

    [SerializeField, Range(1, 30)]
    private int _MaxSyncLoadNum = 1;

    private List<Main_AlbumViewerNode> _ScrollViewNodes = new List<Main_AlbumViewerNode>();

    public void Init()
    {
        _ZoomImage.gameObject.SetActive(false);
        _ZoomImage.material = new Material(_ZoomImage.material);
        ClearListInstance();
        ListUpTextures();
    }

    public void SetActive(bool value)
    {
        _ScrollViewObject.SetActive(value);
    }

    public void ListUpTextures()
    {
        StartCoroutine(Routine_ListUpTextures());
    }

    private IEnumerator Routine_ListUpTextures()
    {
        var list = _DataFileManager.GetAllTexturePath_png();

        int NumOfLoding = 0;
        for (int i = 0, size = list.Length; i < size; ++i)
        {
            while (NumOfLoding <= _MaxSyncLoadNum) yield return null;

            var obj = Instantiate(_Prefab_Node);
            obj.transform.SetParent(_ScrollViewContent, false);

            var component = obj.GetComponent<Main_AlbumViewerNode>();
            component.Init(this);
            _ScrollViewNodes.Add(component);
            ++NumOfLoding;
            _DataFileManager.InputTexture(list[i], component.ImageCallBack, () => --NumOfLoding);
        }
    }

    public void ClearListInstance()
    {
        foreach (var node in _ScrollViewNodes)
        {
            Destroy(node.gameObject);
        }
        _ScrollViewNodes.Clear();
    }

    [SerializeField]
    private Image _ZoomImage;

    public void OpenZoomPicture(Texture texture)
    {
        _ZoomImage.gameObject.SetActive(true);
        _ZoomImage.material.mainTexture = texture;
    }

    public void CloseZoomPicture()
    {
        _ZoomImage.gameObject.SetActive(false);
    }
}
