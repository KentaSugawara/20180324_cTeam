using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main_AlbumViewerNode : MonoBehaviour {
    [SerializeField]
    private Image _Image;

    private Texture texture;

    //[SerializeField]
    //private LayoutElement _LayoutElement;

    public void ImageCallBack(Texture texture)
    {
        this.texture = texture;
        _Image.sprite = Sprite.Create((Texture2D)texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        //Debug.Log("width" + texture.width);
        //Debug.Log("height" + texture.height);
        //if (texture.width >= texture.height)
        //{
        //    _LayoutElement.preferredWidth = texture.width;
        //    _LayoutElement.preferredHeight = texture.height;
        //    if (texture.width > MaxSize)
        //    {
        //        _LayoutElement.preferredWidth = MaxSize;
        //        float q = _LayoutElement.preferredWidth / texture.width;
        //        _LayoutElement.preferredHeight = texture.height * q;
        //    }
        //}
        //else
        //{
        //    _LayoutElement.preferredWidth = texture.width;
        //    _LayoutElement.preferredHeight = texture.height;
        //    if (texture.height > MaxSize)
        //    {
        //        _LayoutElement.preferredHeight = MaxSize;
        //        float q = _LayoutElement.preferredHeight / texture.height;
        //        _LayoutElement.preferredWidth = texture.width * q;
        //    }
        //}
    }
}
