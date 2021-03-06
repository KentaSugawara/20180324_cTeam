﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Main_PictureBookViewer : MonoBehaviour {
    [SerializeField]
    private Main_DataFileManager _DataFileManager;

    [SerializeField]
    private GameObject _ScrollViewObject;

    [SerializeField]
    private ContentSizeFitter _ContentSizeFitter;

    [SerializeField]
    private Main_PictureBookNew _Obj_New;

    [SerializeField]
    private ScrollRect _ScrollView;

    [SerializeField]
    private Transform _ScrollViewContent;

    [SerializeField]
    private Text _Text_NumOfPictures;

    [SerializeField]
    private GameObject _Prefab_Node;

    [SerializeField]
    private float _NewViewSeconds;

    private List<Main_PictureBookViewerNode> _ScrollViewNodes = new List<Main_PictureBookViewerNode>();
    public List<Main_PictureBookViewerNode> ScrollViewNodes { get { return _ScrollViewNodes; } }

    public void Awake()
    {
        _ViewPosition = _BackGround.anchoredPosition;
    }

    public void Init()
    {
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
        //var datalist = _DataFileManager.Load_PictureBookData();

        ////デバッグ用
        //{
        //    var obj = Instantiate(_Prefab_Node);
        //    obj.transform.SetParent(_ScrollViewContent, false);

        //    var component = obj.GetComponent<Main_PictureBookViewerNode>();
        //    //データを渡す(data == nullかもしれない)
        //    component.Init(this, _DataFileManager.CharacterData.CharacterList[0], new Json_PictureBook_ListNode(0));
        //    _ScrollViewNodes.Add(component);
        //    ++NumOfCharacters;
        //}



        {
            var datalist = Main_PictureBookManager.CharacterList.CharacterList;
            var savedatalist = Main_PictureBookManager.CharacterSaveData.Data;
            foreach (var chara in datalist)
            {
                var obj = Instantiate(_Prefab_Node);
                obj.transform.SetParent(_ScrollViewContent, false);
                //キャラのデータが存在するか検索
                var data = savedatalist.Find(c => c.CloseID == chara.CloseID);
                data.isNew = false;
                if (data.NumOfPhotos > 0)
                {
                    ++NumOfCharacters;
                }

                var component = obj.GetComponent<Main_PictureBookViewerNode>();
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

    [SerializeField]
    private Text _Text_ViewName;

    [SerializeField]
    private Text _Text_Info;

    private Vector3 _ViewPosition;

    private bool _isMoving = false;

    private GameObject _CurrentModel;

    public void SetViewWindow(GameObject Prefab, CharacterData chara)
    {
        if (!_isMoving)
        {
            _ModelViewWindow.SetActive(true);
            _ViewWindowModel.GetComponent<Main_ModelViewWindow>().Init();
            if (_CurrentModel != null) Destroy(_CurrentModel);
            if (Prefab != null) _CurrentModel = Instantiate(Prefab);
            Debug.Log(_CurrentModel);
            _ViewWindowModel.GetComponent<Main_ModelViewWindow>().ChangeObj(_CurrentModel);
            _CurrentModel.transform.SetParent(_ViewWindowModel.transform, false);
            _Text_ViewName.text = chara.ViewName;
            _Text_Info.text = chara.Text;

            StopAllCoroutines();
            _Audio_OpenViewWindow.Play();
            StartCoroutine(Routine_OpenWindow());
        }
    }

    [SerializeField]
    private AudioSource _Audio_OpenViewWindow;

    [SerializeField]
    private AudioSource _Audio_CloseViewWindow;

    public void CloseViewWindow()
    {
        if (!_isMoving)
        {
            _ViewWindowModel.SetActive(false);
            StopAllCoroutines();
            _Audio_CloseViewWindow.Play();
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

    public void CloseWindowImmidiate()
    {
        var deltaSize = Vector2.Scale(_BackGround.sizeDelta, new Vector2(_BackGround.lossyScale.x, _BackGround.lossyScale.y));
        var HidePosition = new Vector3(_ViewPosition.x, -_BackGround.sizeDelta.y * 0.6f, _ViewPosition.z);

        _BackGround.anchoredPosition = HidePosition;
        _ModelViewWindow.SetActive(false);

        _isMoving = false;
    }

    public void HideNew()
    {
        _Obj_New.HideNew();
    }

    public void ViewNew()
    {
        _Obj_New.ViewNew();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _ScrollView.OnInitializePotentialDrag(eventData);
        _ScrollView.OnBeginDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _ScrollView.OnEndDrag(eventData);
    }

    public void OnDrag(BaseEventData eventData)
    {
        _ScrollView.OnDrag((PointerEventData)eventData);
    }
}
