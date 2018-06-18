using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_PictureBookManager : MonoBehaviour {
    private static Main_DataFileManager DatafileManager;
    public static Assets_CharacterList CharacterList { get; private set; }
    private static Main_PictureBookViewer PictureBookViewer;
    public static Json_PictureBook_DataList CharacterSaveData { get; private set; }

    public static void Init(Main_DataFileManager DatafileManager, Main_PictureBookViewer Viewer, Assets_CharacterList Asset)
    {
        Main_PictureBookManager.DatafileManager = DatafileManager;
        Main_PictureBookManager.PictureBookViewer = Viewer;
        Main_PictureBookManager.CharacterList = Asset;
        UpdateFromJson();

        //セーブデータを補完する
        {
            foreach (var node in Asset.CharacterList)
            {
                bool isExist = false;
                for (int i = 0, size = CharacterSaveData.Data.Count; i < size; ++i)
                {
                    if (CharacterSaveData.Data[i].CloseID == node.CloseID)
                    {
                        isExist = true;
                        break;
                    }
                }

                //無かったら追加
                if (!isExist)
                {
                    var data = new Json_PictureBook_ListNode(node.CloseID);
                    CharacterSaveData.Data.Add(data);
                }
            }
        }
    }

    public static void UpdateFromJson()
    {
        CharacterSaveData = DatafileManager.Load_PictureBookData();
    }

    /// <summary>
    /// 新しいキャラクターを撮影
    /// </summary>
    public static List<KeyValuePair<GameObject, Sprite>> GetNewCharacters(List<KeyValuePair<GameObject, SnapShotInfo>> EggList)
    {
        List<KeyValuePair<GameObject, Sprite>> NewList = new List<KeyValuePair<GameObject, Sprite>>();

        foreach (var keyValue in EggList)
        {
            var savedata = CharacterSaveData.Data.Find(c => c.CloseID == keyValue.Value.CharaCloseIndex);
            if (savedata != null)
            {
                //一枚も撮っていないなら
                if (savedata.NumOfPhotos <= 0)
                {
                    var CharaData = CharacterList.CharacterList.Find(c => c.CloseID == keyValue.Value.CharaCloseIndex);
                    if (CharaData != null)
                    {
                        NewList.Add(new KeyValuePair<GameObject, Sprite>(keyValue.Key, CharaData.CharaNameSprite));
                    }
                }
            }
        }

        return NewList;
    }

    public static void UpdateAlbum(List<KeyValuePair<GameObject, SnapShotInfo>> EggList)
    {
        bool isChenge = false;
        foreach (var keyValue in EggList)
        {
            var savedata = CharacterSaveData.Data.Find(c => c.CloseID == keyValue.Value.CharaCloseIndex);
            if (savedata != null)
            {
                //一枚も撮っていないなら
                if (savedata.NumOfPhotos <= 0)
                {
                    savedata.NumOfPhotos = 1;
                    savedata.isNew = true;
                    isChenge = true;
                }
                else
                {
                    ++savedata.NumOfPhotos;
                }
            }
        }

        if (isChenge)
        {
            UpdateisNew();
        }
    }

    /// <summary>
    /// 既に見つけているキャラクター数を返す
    /// </summary>
    public static int countNumOfAlreadyFind()
    {
        int cnt = 0;
        foreach (var node in CharacterSaveData.Data)
        {
            if (node.NumOfPhotos > 0) ++cnt;
        }
        return cnt;
    }

    /// <summary>
    /// アイコンにNewを表示するかどうかを更新
    /// </summary>
    public static void UpdateisNew()
    {
        foreach (var node in CharacterSaveData.Data)
        {
            if (node.isNew)
            {
                PictureBookViewer.ViewNew();
                return;
            }
        }
        PictureBookViewer.HideNew();
    }
}
