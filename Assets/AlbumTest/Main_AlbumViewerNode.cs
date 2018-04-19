using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main_AlbumViewerNode : MonoBehaviour {
    [SerializeField]
    private Image _Image;

    [SerializeField]
    private RectTransform _RectTransform;

    [SerializeField]
    private float _png_Width;

    [SerializeField]
    private float _png_Height;

    [SerializeField]
    private float _png_BitDepth;

    [SerializeField]
    private float _png_ColorType;

    [SerializeField]
    private float _png_CompressionMethod;

    [SerializeField]
    private float _png_FilterMethod;

    [SerializeField]
    private float _png_InterlaceMethod;

    [SerializeField]
    private float _png_CRC;

    private byte[] bytes;
    private int Width;
    private int Height;

    private Texture texture;

    //[SerializeField]
    //private LayoutElement _LayoutElement;

    private Main_AlbumViewer _ParentComponent;

    public void Init(Main_AlbumViewer parent)
    {
        _ParentComponent = parent;
    }

    //public void ImageCallBack(Texture texture)
    //{
    //    //this.texture = texture;
    //    //_Image.sprite = Sprite.Create((Texture2D)texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    //    //_Image.material = new Material(_Image.material);
    //    //_Image.material.mainTexture = texture;
    //    //Debug.Log("width" + texture.width);
    //    //Debug.Log("height" + texture.height);
    //    //if (texture.width >= texture.height)
    //    //{
    //    //    _LayoutElement.preferredWidth = texture.width;
    //    //    _LayoutElement.preferredHeight = texture.height;
    //    //    if (texture.width > MaxSize)
    //    //    {
    //    //        _LayoutElement.preferredWidth = MaxSize;
    //    //        float q = _LayoutElement.preferredWidth / texture.width;
    //    //        _LayoutElement.preferredHeight = texture.height * q;
    //    //    }
    //    //}
    //    //else
    //    //{
    //    //    _LayoutElement.preferredWidth = texture.width;
    //    //    _LayoutElement.preferredHeight = texture.height;
    //    //    if (texture.height > MaxSize)
    //    //    {
    //    //        _LayoutElement.preferredHeight = MaxSize;
    //    //        float q = _LayoutElement.preferredHeight / texture.height;
    //    //        _LayoutElement.preferredWidth = texture.width * q;
    //    //    }
    //    //}
    //}

    public void ImageCallBack(ref byte[] bytes, int Width, int Height)
    {
        this.bytes = bytes;
        this.Width = Width;
        this.Height = Height;

        Debug.Log("ChunkType : " + (char)bytes[12] + (char)bytes[13] + (char)bytes[14] + (char)bytes[15]);
        _png_Width = bytes[19]
            + bytes[18] * Mathf.Pow(2, 8)
            + bytes[17] * Mathf.Pow(2, 16)
            + bytes[16] * Mathf.Pow(2, 24);
        _png_Height = bytes[23]
            + bytes[22] * Mathf.Pow(2, 8)
            + bytes[21] * Mathf.Pow(2, 16)
            + bytes[20] * Mathf.Pow(2, 24);

        _png_BitDepth = bytes[24];
        _png_ColorType = bytes[25];
        _png_CompressionMethod = bytes[26];
        _png_FilterMethod = bytes[27];
        _png_InterlaceMethod = bytes[28];
        //Debug.Log("CRC : " + (char)bytes[29] + (char)bytes[30] + (char)bytes[31] + (char)bytes[32]);
        //StartCoroutine(Routine_bytes());

        Texture2D texture = new Texture2D(Width, Height);
        texture.LoadImage(bytes);
        //_Image.material = texture;
        //Debug.Log("Length : " + bytes[36]);
        //Debug.Log("ChunkType : " + (char)bytes[37] + (char)bytes[38] + (char)bytes[39] + (char)bytes[40]);

        //var sizeDelta = _RectTransform.sizeDelta;
        //int ViewWidth = (int)(transform.lossyScale.x * sizeDelta.x);
        //int ViewHeight = (int)(transform.lossyScale.y * sizeDelta.y);

        ////ここで間引いて表示
        //int bufferLength = ViewWidth * ViewHeight;
        //Color[] buffer = new Color[bufferLength];

        //float scale = bytes.Length / bufferLength;

        //for (float i = 0.0f; i < bufferLength; i += scale)
        //{

        //}
    }

    private IEnumerator Routine_bytes()
    {
        for (int i = 33; i < bytes.Length;)
        {
            int length = bytes[i + 3]
            + bytes[i + 2] * (int)Mathf.Pow(2, 8)
            + bytes[i + 1] * (int)Mathf.Pow(2, 16)
            + bytes[i] * (int)Mathf.Pow(2, 24);
            Debug.Log("Length : " + length); 
            
            i += 4;
            Debug.Log("ChunkType : " + (char)bytes[i] + (char)bytes[i + 1] + (char)bytes[i + 2] + (char)bytes[i + 3]);
            i += 4;
            i += length;
            i += 4; // CRC
            yield return null;
        }
    }

    public void OpenZoomPicture()
    {
        _ParentComponent.OpenZoomPicture(_Image.material.mainTexture);
    }
}
