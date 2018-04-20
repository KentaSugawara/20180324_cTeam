using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main_PictureBookViewerNode : MonoBehaviour {
    [SerializeField]
    private Image _Image;

    private CharacterData _myCharacterData = null;
    private Json_PictureBook_ListNode _myData = null;

    private Main_PictureBookViewer _ParentComponent;

    public void Init(Main_PictureBookViewer parent, CharacterData chara, Json_PictureBook_ListNode data)
    {
        _ParentComponent = parent;
        _myCharacterData = chara;
        _myData = data;

        //画像を差し替え
        if (data != null)
            _Image.sprite = chara.sprite;//Sprite.Create((Texture2D)texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    public void SetViewWindow()
    {
        if (_myData != null)
            _ParentComponent.SetViewWindow();
    }
}
