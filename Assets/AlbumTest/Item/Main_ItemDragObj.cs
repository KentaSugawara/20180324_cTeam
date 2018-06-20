﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main_ItemDragObj : MonoBehaviour {
    [SerializeField]
    private Image _Image;
    public Image ItemImage
    {
        get { return _Image; }
    }

    [SerializeField]
    private GameObject _Obj_CanNotSet;

    private int _ItemIndex;

    public void Init(Sprite sprite, int ItemIndex)
    {
        _Image.sprite = sprite;
        _ItemIndex = ItemIndex;
        transform.position = Input.mousePosition;
    }

    public void SetActive_CanNotSetImage(bool value)
    {
        var color = _Image.color;
        if (value)
        {
            _Image.color = Color.black;
        }
        else
        {
            _Image.color = Color.white;
        }
    }

    private void Update()
    {
        transform.position = Input.mousePosition;
    }
}
