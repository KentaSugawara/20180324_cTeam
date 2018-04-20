using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main_PictureBookViewer : MonoBehaviour {
    [SerializeField]
    private Main_DataFileManager _DataFileManager;

    [SerializeField]
    private GameObject _ScrollViewObject;

    [SerializeField]
    private Scrollbar _ScrollViewVerticalBar;

    [SerializeField]
    private Transform _ScrollViewContent;

    [SerializeField]
    private Text _Text_NumOfPictures;

    [SerializeField]
    private GameObject _Prefab_Node;

    private List<Main_PictureBookViewerNode> _ScrollViewNodes = new List<Main_PictureBookViewerNode>();

    public void Init()
    {
        CloseViewWindow();
        ClearListInstance();
        ListUpTextures();
    }

    public void SetActive(bool value)
    {
        _ScrollViewObject.SetActive(value);
    }

    public void ListUpTextures()
    {
        int NumOfCharacters = 0;
        var datalist = _DataFileManager.Load_PictureBookData();

        //デバッグ用
        {
            var obj = Instantiate(_Prefab_Node);
            obj.transform.SetParent(_ScrollViewContent, false);

            var component = obj.GetComponent<Main_PictureBookViewerNode>();
            //データを渡す(data = nullかもしれない)
            component.Init(this, _DataFileManager.CharacterData.CharacterList[0], new Json_PictureBook_ListNode(0));
            _ScrollViewNodes.Add(component);
            ++NumOfCharacters;
        }

        {
            Json_PictureBook_ListNode data = null;
            foreach (var chara in _DataFileManager.CharacterData.CharacterList)
            {
                var obj = Instantiate(_Prefab_Node);
                obj.transform.SetParent(_ScrollViewContent, false);

                //キャラのデータが存在するか検索
                foreach(var d in datalist.Data)
                {
                    if (d.CharacterCloseID == chara.CloseID)
                    {
                        data = d;
                        ++NumOfCharacters;
                        break;
                    }
                }

                var component = obj.GetComponent<Main_PictureBookViewerNode>();
                //データを渡す(data = nullかもしれない)
                component.Init(this, chara, data);
                _ScrollViewNodes.Add(component);
            }
        }

        _ScrollViewVerticalBar.value = 1.0f;

        _Text_NumOfPictures.text = NumOfCharacters + "/" + _ScrollViewNodes.Count;
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
    private GameObject _ModelViewWindow;

    [SerializeField]
    private GameObject _ViewWindowModel;

    public void SetViewWindow()
    {
        _ModelViewWindow.SetActive(true);
        _ViewWindowModel.SetActive(true);
    }

    public void CloseViewWindow()
    {
        _ModelViewWindow.SetActive(false);
        _ViewWindowModel.SetActive(false);
    }
}
