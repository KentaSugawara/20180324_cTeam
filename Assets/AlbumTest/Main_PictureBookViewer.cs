using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_PictureBookViewer : MonoBehaviour {
    [SerializeField]
    private Main_DataFileManager _DataFileManager;

    [SerializeField]
    private GameObject _ScrollViewObject;

    [SerializeField]
    private Transform _ScrollViewContent;

    [SerializeField]
    private GameObject _Prefab_Node;

    private List<Main_PictureBookViewerNode> _ScrollViewNodes = new List<Main_PictureBookViewerNode>();

    public void Init()
    {
        ClearListInstance();
        ListUpTextures();
    }

    public void SetActive(bool value)
    {
        _ScrollViewObject.SetActive(value);
    }

    public void ListUpTextures()
    {
        var datalist = _DataFileManager.Load_PictureBookData();

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
                        break;
                    }
                }

                var component = obj.GetComponent<Main_PictureBookViewerNode>();
                //データを渡す(data = nullかもしれない)
                component.Init(chara, data);
                _ScrollViewNodes.Add(component);
            }
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
}
