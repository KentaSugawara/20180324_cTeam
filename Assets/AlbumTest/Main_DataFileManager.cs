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
        var file = new FileInfo(FilePath + "/PictureBookDataList.json");

        //ファイルが存在しなかったら
        if (!file.Exists)
        {
            //新規作成
            CreateFile_AlbumDataList(file);
            file = new FileInfo(FilePath + "/PictureBookDataList.json");
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

    /// <summary>
    /// ルート以下にある全てのpngファイルのFileInfoを受け取る
    /// </summary>
    /// <returns></returns>
    public FileInfo[] GetAllTexturePath_png()
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
    public delegate void ImageCallBack(ref byte[] bytes, int Width, int Height);
    public void InputTexture(FileInfo file, ImageCallBack callback, System.Action endcallback)
    {
        if (!file.Exists)
        {
            return;
        }

        StartCoroutine(Routine_LoadImage(file.FullName, callback, endcallback));
    }

    IEnumerator Routine_LoadImage(string FilePath, ImageCallBack callback, System.Action endcallback)
    {
        //using (WWW www = new WWW("file:///" + FilePath))
        //{
        //    Debug.Log("LoadStart");
        //    //読み込み完了まで待機
        //    yield return www;
        //    Debug.Log("LoadEnd");
        //    callback(www.texture);
        //}
        //endcallback();
        using (FileStream fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
        {
            BinaryReader bin = new BinaryReader(fileStream);
            byte[] bytes = bin.ReadBytes((int)bin.BaseStream.Length);
            bin.Close();

            yield return null;
            Debug.Log("byte");

            int pos = 16; // 16バイトから開始

            int width = 0;
            for (int i = 0; i < 4; i++)
            {
                width = width * 256 + bytes[pos++];
            }

            int height = 0;
            for (int i = 0; i < 4; i++)
            {
                height = height * 256 + bytes[pos++];
            }

            Texture2D texture = new Texture2D(width, height);

            yield return null;
            Debug.Log("texture");
            callback(ref bytes, width, height);

            //texture.LoadImage(bytes);

            //yield return null;
            //Debug.Log("loadlimage");

            //callback(texture);
        }
        yield return null;
        Debug.Log("LoadEnd");
        endcallback();
        yield break;
    }

    public void CreateFile_AlbumDataList(FileInfo file)
    {
        using (StreamWriter sw = file.CreateText())
        {
            sw.WriteLine(JsonUtility.ToJson(new Json_PictureBook_DataList(), true));
        }
    }

    public void CreateJsonFile<T>(FileInfo file, T Instance)
    {
        using (StreamWriter sw = file.CreateText())
        {
            sw.WriteLine(JsonUtility.ToJson(Instance, true));
        }
    }

    public T getJsonClassInstance<T> (FileInfo file)
    {
        using (StreamReader sr = new StreamReader(file.OpenRead()))
        {
            return JsonUtility.FromJson<T>(sr.ReadToEnd());
        }
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

    /// <summary>
    /// 保存先のルートパスを受け取る
    /// </summary>
    public string getRootPath()
    {
        string FullPath = "";
    #if UNITY_ANDROID
        FullPath = Application.persistentDataPath;
    #endif

    #if UNITY_EDITOR
        FullPath = Application.streamingAssetsPath;
    #endif
        return FullPath;
    }

    /// <summary>
    /// アルバムの保存先パスを受け取る
    /// </summary>
    public string getAlubmDataPath(int CloseID)
    {
        string FullPath = "";
    #if UNITY_ANDROID
        FullPath = getRootPath() + "/AlubmData/Chara_" + CloseID + ".json";
    #endif

    #if UNITY_EDITOR
        FullPath = getRootPath() + "/AlubmData/Chara_" + CloseID + ".json";
    #endif
        return FullPath;
    }

    public void SavePhoto(int CharacterCloseID, Json_Album_Data_Photo PhotoData)
    {
        string FullPath = "";
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
public class Json_Album_Data_Photo
{
    public string DirectoryPath; //保存フォルダ名
    public string FileName; //保存ファイル名

    //以下保存時間
    public int Year;
    public int Month;
    public int Day;
    public int Time;
    public int Minutes;
    public int Seconds;
}

//アルバムのノードのデータ構造
public class Json_Album_Data
{
    public List<Json_Album_Data_Photo> Photos = new List<Json_Album_Data_Photo>(); //順不同です
}

//図鑑のリストデータのノード(データ自体は別json)
public class Json_PictureBook_ListNode
{
    public int CharacterCloseID; //キャラの内部ID
    //public string DataPath; //このキャラクターの写真リストを保存しているJsonの絶対パス
    public int NumOfPhotos = 0; // 今までに撮った写真の数

    public Json_PictureBook_ListNode(int CharacterCloseID/*, string DataPath*/)
    {
        this.CharacterCloseID = CharacterCloseID;
        //this.DataPath = DataPath;
    }
}

//図鑑のリストデータ
public class Json_PictureBook_DataList
{
    public List<Json_PictureBook_ListNode> Data = new List<Json_PictureBook_ListNode>();
}