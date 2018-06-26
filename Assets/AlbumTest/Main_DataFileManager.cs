using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Main_DataFileManager : MonoBehaviour {

    [SerializeField]
    private Assets_CharacterList _CharacterData;
    public Assets_CharacterList CharacterData
    {
        get { return _CharacterData; }
    }

    public Json_PictureBook_DataList Load_PictureBookData()
    {
        string FilePath = getRootPath();
        var file = new FileInfo(getPictureBookDataListPath());

        //ファイルが存在しなかったら
        if (!file.Exists)
        {
            //新規作成
            CreateFile_PictureBookDataList();
            return new Json_PictureBook_DataList();
            //file = new FileInfo(FilePath + "/PictureBookDataList.json");
        }

        //アルバムリストのインスタンスを受け取る
        var album = getJsonClassInstance<Json_PictureBook_DataList>(file);
        //{
        //    bool isExist = false;

        //    //キャラクター用データがあるか精査する
        //    foreach (var chara in _CharacterData.CharacterList)
        //    {
        //        isExist = false;
        //        foreach (var node in album.Data)
        //        {
        //            if (node.CharacterCloseID == chara.CloseID)
        //            {
        //                isExist = true;
        //                break;
        //            }
        //        }

        //        if (!isExist)
        //        {
        //            //無かったらalubumに追加する
        //            var data = new Json_PictureBook_ListNode(chara.CloseID/*, getAlubmDataPath(chara.CloseID)*/);
        //            album.Data.Add(data);
        //        }
        //    }
        //}

        return album;
    }

    public Json_Album_DataList Load_AlbumData()
    {
        string FilePath = getRootPath();
        var file = new FileInfo(getAlbumDataListPath());

        //ファイルが存在しなかったら
        if (!file.Exists)
        {
            //新規作成
            CreateFile_AlbumDataList();
            return new Json_Album_DataList();
            //file = new FileInfo(FilePath + "/PictureBookDataList.json");
        }

        //アルバムリストのインスタンスを受け取る
        var album = getJsonClassInstance<Json_Album_DataList>(file);
        //{
        //    bool isExist = false;

        //    //キャラクター用データがあるか精査する
        //    foreach (var chara in _CharacterData.CharacterList)
        //    {
        //        isExist = false;
        //        foreach (var node in album.Data)
        //        {
        //            if (node.CharacterCloseID == chara.CloseID)
        //            {
        //                isExist = true;
        //                break;
        //            }
        //        }

        //        if (!isExist)
        //        {
        //            //無かったらalubumに追加する
        //            var data = new Json_PictureBook_ListNode(chara.CloseID/*, getAlubmDataPath(chara.CloseID)*/);
        //            album.Data.Add(data);
        //        }
        //    }
        //}

        return album;
    }

    public void Save_AlbumData(Json_Album_DataList Album)
    {
        var file = new FileInfo(getAlbumDataListPath());

        //ファイルが存在しなかったら
        if (!file.Exists)
        {
            file.Create();
        }

        CreateJsonFile<Json_Album_DataList>(file, Album);
    }

    public void Save_NewAlbumPicture(Json_Album_Data_Picture Picture)
    {
        //アルバムデータリストに追加
        var album = Load_AlbumData();
        album.Pictures.Add(Picture);
        Save_AlbumData(album);
    }

    public void Save_SaveData(Json_SaveData savedata)
    {
        string FilePath = getSaveDataPath();
        var file = new FileInfo(FilePath);

        //ファイルが存在しなかったら
        if (!file.Exists)
        {
            file.Create();
        }

        //アルバムリストのインスタンスを受け取る
        CreateJsonFile<Json_SaveData>(file, savedata);
    }

    public Json_SaveData Load_SaveData()
    {
        string FilePath = getSaveDataPath();
        var file = new FileInfo(FilePath);

        //ファイルが存在しなかったら
        if (!file.Exists)
        {
            //新規作成
            return new Json_SaveData();
            //file = new FileInfo(FilePath + "/PictureBookDataList.json");
        }

        //アルバムリストのインスタンスを受け取る
        var savedata = getJsonClassInstance<Json_SaveData>(file);
        return savedata;
    }

    public Json_Challenge_DataList Load_ChallengeData()
    {
        string FilePath = getRootPath();
        var file = new FileInfo(getChallengeDataListPath());

        //ファイルが存在しなかったら
        if (!file.Exists)
        {
            //新規作成
            CreateFile_ChallengeDataList();
            return new Json_Challenge_DataList();
            //file = new FileInfo(FilePath + "/PictureBookDataList.json");
        }

        //アルバムリストのインスタンスを受け取る
        var challenge = getJsonClassInstance<Json_Challenge_DataList>(file);

        return challenge;
    }

    public void Save_ChallengeData(Json_Challenge_DataList challenge)
    {
        var file = new FileInfo(getChallengeDataListPath());

        //ファイルが存在しなかったら
        if (!file.Exists)
        {
            file.Create();
        }

        CreateJsonFile<Json_Challenge_DataList>(file, challenge);
    }

    public Json_Item_DataList Load_ItemData()
    {
        string FilePath = getRootPath();
        var file = new FileInfo(getItemDataListPath());

        //ファイルが存在しなかったら
        if (!file.Exists)
        {
            //新規作成
            CreateFile_ChallengeDataList();
            return new Json_Item_DataList();
            //file = new FileInfo(FilePath + "/PictureBookDataList.json");
        }

        //アルバムリストのインスタンスを受け取る
        var itemlist = getJsonClassInstance<Json_Item_DataList>(file);

        return itemlist;
    }

    public void Save_ItemData(Json_Item_DataList itemlist)
    {
        var file = new FileInfo(getItemDataListPath());

        //ファイルが存在しなかったら
        if (!file.Exists)
        {
            file.Create();
        }

        CreateJsonFile<Json_Item_DataList>(file, itemlist);
    }

    public void Output_AlbumPicturePNG(Texture2D texture, Json_Album_Data_Picture Picture)
    {
        //本体PNG
        {
            var directory = new DirectoryInfo(getAlbumPicturesPath());
            if (!directory.Exists)
            {
                directory.Create();
            }

            // テクスチャを PNG に変換
            byte[] bytes = texture.EncodeToPNG();

            // PNGデータをファイルとして保存
            File.WriteAllBytes(directory.FullName + "/" + Picture.FileName, bytes);
        }

        //圧縮PNG
        {
            var directory = new DirectoryInfo(getAlbumSmallPicturesPath());
            if (!directory.Exists)
            {
                directory.Create();
            }

            //圧縮
            TextureScale.Bilinear(texture, (int)(texture.width * 0.1f), (int)(texture.height * 0.1f));

            // テクスチャを PNG に変換
            byte[] bytes = texture.EncodeToPNG();

            // PNGデータをファイルとして保存
            File.WriteAllBytes(directory.FullName + "/" + Picture.FileName_Small, bytes);
        }
    }


    /// <summary>
    /// ルート以下にある全てのpngファイルのFileInfoを受け取る
    /// </summary>
    /// <returns></returns>
    public FileInfo[] 
        TexturePath_png()
    {
        //Stagesフォルダ
        var directory = new DirectoryInfo(getRootPath());
        Debug.Log(directory.FullName);
        //無かったら作る
        if (!directory.Exists) directory.Create();

        //jsonをすべて受け取る
        return directory.GetFiles("*.png", SearchOption.AllDirectories);
    }

    //public delegate void ImageCallBack(Texture texture);
    public delegate void ImageCallBack(Texture texture);
    public void InputAlbumPicture(string LocalFileName, ImageCallBack callback, System.Action endcallback)
    {
        var file = new FileInfo(getAlbumPicturesPath() + "/" + LocalFileName);
        if (!file.Exists)
        {
            endcallback();
            return;
        }

        StartCoroutine(Routine_LoadImage(file.FullName, callback, endcallback));
    }

    public void InputAlbumSmallPicture(string LocalFileName, ImageCallBack callback, System.Action endcallback)
    {
        var file = new FileInfo(getAlbumSmallPicturesPath() + "/" + LocalFileName);
        if (!file.Exists)
        {
            endcallback();
            return;
        }

        StartCoroutine(Routine_LoadImage(file.FullName, callback, endcallback));
    }

    IEnumerator Routine_LoadImage(string FilePath, ImageCallBack callback, System.Action endcallback)
    {
        using (WWW www = new WWW("file:///" + FilePath))
        {
            //読み込み完了まで待機
            yield return www;
            callback(www.texture);
        }
        //using (FileStream fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
        //{
        //    BinaryReader bin = new BinaryReader(fileStream);
        //    byte[] bytes = bin.ReadBytes((int)bin.BaseStream.Length);
        //    bin.Close();

        //    yield return null;

        //    int pos = 16; // 16バイトから開始

        //    int width = 0;
        //    for (int i = 0; i < 4; i++)
        //    {
        //        width = width * 256 + bytes[pos++];
        //    }

        //    int height = 0;
        //    for (int i = 0; i < 4; i++)
        //    {
        //        height = height * 256 + bytes[pos++];
        //    }

        //    Texture2D texture = new Texture2D(width, height);

        //    yield return null;
        //    callback(ref bytes, width, height);

        //    //texture.LoadImage(bytes);

        //    //yield return null;
        //    //Debug.Log("loadlimage");

        //    //callback(texture);
        //}
        yield return null;
        endcallback();
        yield break;
    }

    public void Delete_AlbumPicture(Json_Album_Data_Picture picture)
    {
        {
            var file = new FileInfo(getAlbumPicturesPath() + "/" + picture.FileName);
            if (file.Exists)
            {
                file.Delete();
            }
        }

        {
            var file = new FileInfo(getAlbumSmallPicturesPath() + "/" + picture.FileName_Small);
            if (file.Exists)
            {
                file.Delete();
            }
        }
    }

    public void CreateFile_PictureBookDataList()
    {
        var file = new FileInfo(getPictureBookDataListPath());
        //FileStream fs = new FileStream(file.FullName, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.ReadWrite);
        File.CreateText(file.FullName).Dispose();
        using (StreamWriter sw = new StreamWriter(file.FullName, false))
        {
            sw.WriteLine(JsonUtility.ToJson(new Json_PictureBook_DataList(), true));
            sw.Close();
        }
    }

    public void CreateFile_AlbumDataList()
    {
        var file = new FileInfo(getAlbumDataListPath());
        //FileStream fs = new FileStream(file.FullName, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.ReadWrite);
        File.CreateText(file.FullName).Dispose();
        using (StreamWriter sw = new StreamWriter(file.FullName, false))
        {
            sw.WriteLine(JsonUtility.ToJson(new Json_Album_DataList(), true));
            sw.Close();
        }
    }

    public void CreateFile_ChallengeDataList()
    {
        var file = new FileInfo(getChallengeDataListPath());
        //FileStream fs = new FileStream(file.FullName, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.ReadWrite);
        File.CreateText(file.FullName).Dispose();
        using (StreamWriter sw = new StreamWriter(file.FullName, false))
        {
            sw.WriteLine(JsonUtility.ToJson(new Json_Challenge_DataList(), true));
            sw.Close();
        }
    }

    public void CreateFile_ItemDataList()
    {
        var file = new FileInfo(getItemDataListPath());
        //FileStream fs = new FileStream(file.FullName, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.ReadWrite);
        File.CreateText(file.FullName).Dispose();
        using (StreamWriter sw = new StreamWriter(file.FullName, false))
        {
            sw.WriteLine(JsonUtility.ToJson(new Json_Item_DataList(), true));
            sw.Close();
        }
    }

    public void CreateJsonFile<T>(FileInfo file, T Instance)
    {
        //FileStream fs = new FileStream(file.FullName, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.ReadWrite);
        File.CreateText(file.FullName).Dispose();
        using (StreamWriter sw = new StreamWriter(file.FullName, false))
        {
            sw.WriteLine(JsonUtility.ToJson(Instance, true));
            sw.Close();
        }
    }

    public T getJsonClassInstance<T> (FileInfo file)
    {
        //FileStream fs = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        T j;
        using (StreamReader sr = file.OpenText())
        {
            j = JsonUtility.FromJson<T>(sr.ReadToEnd());
            sr.Close(); 
        }

        return j;
    }

    /// <summary>
    /// AlubmData jsonファイルを新規作成
    /// </summary>
    /// <param name="CloseIndex"></param>
    public void CreateFile_CharaAlbumData(int CloseID)
    {
        var file = new FileInfo(getRootPath() + "/AlubmData/Chara_" + CloseID + ".json");
        //CreateJsonFile(file,)
    }

    public string getPictureBookDataListPath()
    {
        string FullPath = Application.persistentDataPath + "/PictureBookDataList.json";
    #if UNITY_ANDROID
        FullPath = Application.persistentDataPath + "/PictureBookDataList.json";
    #endif

    #if UNITY_EDITOR
        FullPath = Application.persistentDataPath + "/PictureBookDataList.json";
    #endif
        return FullPath;
    }

    public string getAlbumDataListPath()
    {
        string FullPath = Application.persistentDataPath + "/AlbumDataList.json";
    #if UNITY_ANDROID
        FullPath = Application.persistentDataPath + "/AlbumDataList.json";
    #endif

    #if UNITY_EDITOR
        FullPath = Application.persistentDataPath + "/AlbumDataList.json";
    #endif
        return FullPath;
    }

    public string getChallengeDataListPath()
    {
        string FullPath = Application.persistentDataPath + "/ChallengeDataList.json";
    #if UNITY_ANDROID
        FullPath = Application.persistentDataPath + "/ChallengeDataList.json";
    #endif

    #if UNITY_EDITOR
        FullPath = Application.persistentDataPath + "/ChallengeDataList.json";
    #endif
        return FullPath;
    }

    public string getItemDataListPath()
    {
        string FullPath = FullPath = Application.persistentDataPath + "/ItemDataList.json";
    #if UNITY_ANDROID
        FullPath = Application.persistentDataPath + "/ItemDataList.json";
    #endif

    #if UNITY_EDITOR
        FullPath = Application.persistentDataPath + "/ItemDataList.json";
    #endif
        return FullPath;
    }

    /// <summary>
    /// 保存先のルートパスを受け取る
    /// </summary>
    public string getRootPath()
    {
        string FullPath = Application.persistentDataPath;
    #if UNITY_ANDROID
        FullPath = Application.persistentDataPath;
    #endif

    #if UNITY_EDITOR
        FullPath = Application.persistentDataPath;
    #endif
        return FullPath;
    }

    /// <summary>
    /// アルバム写真の保存先パスを受け取る
    /// </summary>
    public string getAlbumPicturesPath()
    {
        string FullPath = getRootPath() + "/Pictures";
    #if UNITY_ANDROID
        FullPath = getRootPath() + "/Pictures";
    #endif

    #if UNITY_EDITOR
        FullPath = getRootPath() + "/Pictures";
    #endif
        return FullPath;
    }

        /// <summary>
    /// アルバム写真の保存先パスを受け取る
    /// </summary>
    public string getAlbumSmallPicturesPath()
    {
        string FullPath = getRootPath() + "/SmallPictures";
    #if UNITY_ANDROID
        FullPath = getRootPath() + "/SmallPictures";
    #endif

    #if UNITY_EDITOR
        FullPath = getRootPath() + "/SmallPictures";
    #endif
        return FullPath;
    }

    /// <summary>
    /// セーブデータの
    /// </summary>
    public string getSaveDataPath()
    {
        string FullPath = Application.persistentDataPath + "/SaveData.json";
    #if UNITY_ANDROID
        FullPath = Application.persistentDataPath + "/SaveData.json";
    #endif

    #if UNITY_EDITOR
        FullPath = Application.persistentDataPath + "/SaveData.json";
    #endif
        return FullPath;
    }

    public void SavePhoto(int CharacterCloseID, Json_Album_Data_Picture PhotoData)
    {
        string FullPath = Application.persistentDataPath + "/GameData.json";
    #if UNITY_ANDROID
        FullPath = Application.persistentDataPath + "/GameData.json";
    #endif

    #if UNITY_EDITOR
        FullPath = Application.streamingAssetsPath + "/GameData.json";
    #endif
        if (FullPath == "") return;
    }
}

//アルバムの写真のデータ構造
[System.Serializable]
public class Json_Album_Data_Picture
{

    //public string DirectoryPath; //保存フォルダ名
    //現在はRoot + Picturesフォルダ直下に格納
    public string FileName; //保存ファイル名
    //現在はRoot + SmallPicturesフォルダ直下に格納
    public string FileName_Small; // 簡易版画像のファイル名

    //以下保存時間
    public int Year;
    public int Month;
    public int Day;
    public int Hour;
    public int Minute;
    public int Second;

    //キャラクターデータ
    public List<int> CharacterCloseIDs = new List<int>();
}

//アルバムのノードのデータ構造
[System.Serializable]
public class Json_Album_DataList
{
    public List<Json_Album_Data_Picture> Pictures = new List<Json_Album_Data_Picture>(); //順不同です
}

//図鑑のリストデータのノード(データ自体は別json)
[System.Serializable]
public class Json_PictureBook_ListNode
{
    public int CloseID; //キャラの内部ID
    //public string DataPath; //このキャラクターの写真リストを保存しているJsonの絶対パス
    public int NumOfPhotos = 0; // 今までに撮った写真の数
    public bool isNew = false; 

    public Json_PictureBook_ListNode(int CharacterCloseID/*, string DataPath*/)
    {
        this.CloseID = CharacterCloseID;
        //this.DataPath = DataPath;
    }
}

//図鑑のリストデータ
[System.Serializable]
public class Json_PictureBook_DataList
{
    public List<Json_PictureBook_ListNode> Data = new List<Json_PictureBook_ListNode>();
}

//チャレンジの保存データ
[System.Serializable]
public class Json_Challenge_ListNode
{
    public int CloseID = -1;
    public bool isCleard = false;
    public bool isNewCleard = false;
}

[System.Serializable]
public class Json_Challenge_DataList
{
    public List<Json_Challenge_ListNode> Data = new List<Json_Challenge_ListNode>();
}

//アイテムの保存データ
[System.Serializable]
public class Json_Item_ListNode
{
    public int CloseID = -1;
    public bool isActive = false;
    public bool isNewActive = false;
}

[System.Serializable]
public class Json_Item_DataList
{
    public List<Json_Item_ListNode> Data = new List<Json_Item_ListNode>();
}

[System.Serializable]
public class Json_SaveData
{
    public bool isAlreadyTutorial = false;
}