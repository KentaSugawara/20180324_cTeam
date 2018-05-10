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
    private ContentSizeFitter _ContentSizeFitter;

    [SerializeField]
    private ScrollRect _ScrollView;

    [SerializeField]
    private Transform _ScrollViewContent;

    [SerializeField]
    private Text _Text_NumOfPictures;

    [SerializeField]
    private GameObject _Prefab_Node;

    private List<Main_PictureBookViewerNode> _ScrollViewNodes = new List<Main_PictureBookViewerNode>();

    public void Init()
    {
        _ViewPosition = _BackGround.anchoredPosition;
        _ModelViewWindow.SetActive(false);
        _ViewWindowModel.SetActive(false);
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
            //データを渡す(data == nullかもしれない)
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

        //_ScrollViewVerticalBar.value = 1.0f;
        _ContentSizeFitter.SetLayoutVertical();
        _ScrollView.verticalNormalizedPosition = 1.0f;

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
    private RectTransform _BackGround;

    [SerializeField]
    private GameObject _ViewWindowModel;

    [SerializeField]
    private float _ToOpenNeedSeconds;

    private Vector3 _ViewPosition;

    private bool _isMoving = false;

    public void SetViewWindow()
    {
        if (!_isMoving)
        {
            _ModelViewWindow.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(Routine_OpenWindow());
        }
    }

    public void CloseViewWindow()
    {
        if (!_isMoving)
        {
            _ViewWindowModel.SetActive(false);
            StopAllCoroutines();
            StartCoroutine(Routine_CloseWindow());
        }
    }

    private IEnumerator Routine_OpenWindow()
    {
        _BackGround.anchoredPosition = new Vector3(_ViewPosition.x,-_BackGround.sizeDelta.y * 0.6f, _ViewPosition.z);
        yield return null;
        var deltaSize = Vector2.Scale(_BackGround.sizeDelta, new Vector2(_BackGround.lossyScale.x, _BackGround.lossyScale.y));
        var HidePosition = new Vector3(_ViewPosition.x, -_BackGround.sizeDelta.y * 0.6f, _ViewPosition.z);
        Vector3 b1;

        _isMoving = true;
        _BackGround.anchoredPosition = HidePosition;


        yield return null;
        _ContentSizeFitter.SetLayoutVertical();
        _ScrollView.verticalNormalizedPosition = 1.0f;

        for (float t = 0.0f; t < _ToOpenNeedSeconds; t += Time.deltaTime)
        {
            float e = t / _ToOpenNeedSeconds;
            b1 = Vector3.Lerp(HidePosition, _ViewPosition, e);
            _BackGround.anchoredPosition = Vector3.Lerp(b1, _ViewPosition, e);

            yield return null;
        }
        _BackGround.anchoredPosition = _ViewPosition;
        _isMoving = false;
        _ViewWindowModel.SetActive(true);
    }

    private IEnumerator Routine_CloseWindow()
    {
        var deltaSize = Vector2.Scale(_BackGround.sizeDelta, new Vector2(_BackGround.lossyScale.x, _BackGround.lossyScale.y));
        var HidePosition = new Vector3(_ViewPosition.x, -_BackGround.sizeDelta.y * 0.6f, _ViewPosition.z);
        Vector3 b1;

        _isMoving = true;
        for (float t = 0.0f; t < _ToOpenNeedSeconds; t += Time.deltaTime)
        {
            float e = t / _ToOpenNeedSeconds;
            b1 = Vector3.Lerp(_ViewPosition, HidePosition, e);
            _BackGround.anchoredPosition = Vector3.Lerp(_ViewPosition, b1, e);

            yield return null;
        }
        _BackGround.anchoredPosition = HidePosition;
        _ModelViewWindow.SetActive(false);
        
        _isMoving = false;
    }
}
