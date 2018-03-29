using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main_PictureBookViewerNode : MonoBehaviour {
    [SerializeField]
    private Image _Image;

    private CharacterData _myCharacterData = null;
    private Json_PictureBook_ListNode _myData = null;

    public void Init(CharacterData chara, Json_PictureBook_ListNode data)
    {
        _myCharacterData = chara;
        _myData = data;

        //画像を差し替え
        _Image.sprite = chara.sprite;//Sprite.Create((Texture2D)texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}
