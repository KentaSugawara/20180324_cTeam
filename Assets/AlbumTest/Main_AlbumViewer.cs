﻿using System.Collections;
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

    private List<Main_AlbumViewerNode> _ScrollViewNodes = new List<Main_AlbumViewerNode>();

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
        var list = _DataFileManager.GetAllTexturePath_png();
        for (int i = 0, size = list.Length; i < size; ++i)
        {
            var obj = Instantiate(_Prefab_Node);
            obj.transform.SetParent(_ScrollViewContent, false);

            var component = obj.GetComponent<Main_AlbumViewerNode>();
            _ScrollViewNodes.Add(component);
            _DataFileManager.InputTexture(list[i], component.ImageCallBack);
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