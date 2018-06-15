using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main_ItemDragObj : MonoBehaviour {
    [SerializeField]
    private Image _Image;

    private int _ItemIndex;

    public void Init(Sprite sprite, int ItemIndex)
    {
        _Image.sprite = sprite;
        _ItemIndex = ItemIndex;
        transform.position = Input.mousePosition;
    }

    private void Update()
    {
        transform.position = Input.mousePosition;
    }
}
