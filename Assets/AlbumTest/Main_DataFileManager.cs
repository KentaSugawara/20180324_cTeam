using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Main_DataFileManager : MonoBehaviour {

    [SerializeField]
    private Assets_CharacterList _CharacterData;

    public Json_AlbumDataList AlbumDataLoad()
    {
        string FilePath = getRootPath();
        var file = new FileInfo(FilePath + "/AlbumDataList.json");

        //ファイルが存在しなかったら
        if (!file.Exists)
        {
            //新規作成
            CreateFile_AlbumDataList(file);
            file = new FileInfo(FilePath + "/AlbumDataList.json");
        }

        //アルバムリストのインスタンスを受け取る
        var album = getJsonClassInstance<Json_AlbumDataList>(file);
        {
            bool isExist = false;

            //キャラクター用データがあるか精査する
            foreach (var chara in _CharacterData.CharacterList)
            {
                isExist = false;
                foreach (var node in album.Data)
                {
                    if (node.CharacterCloseID == chara.CloseID)
                    {
                        isExist = true;
                        break;
                    }
                }

                if (!isExist)
                {
                    //無かったらalubumに追加する
                    var data = new Json_Album_ListNode(chara.CloseID, getAlubmDataPath(chara.CloseID));
                    album.Data.Add(data);
                }
            }
        }

        return album;
    }

    public void CreateFile_AlbumDataList(FileInfo file)
    {
        using (StreamWriter sw = file.CreateText())
        {
            sw.WriteLine(JsonUtility.ToJson(new Json_AlbumDataList(), true));
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

//アルバムのリストデータのノード(データ自体は別json)
public class Json_Album_ListNode
{
    public int CharacterCloseID; //キャラの内部ID
    public string DataPath; //このキャラクターの写真リストを保存しているJsonの絶対パス
    public int NumOfPhotos = 0; // 今までに撮った写真の数

    public Json_Album_ListNode(int CharacterCloseID, string DataPath)
    {
        this.CharacterCloseID = CharacterCloseID;
        this.DataPath = DataPath;
    }
}

//アルバムのリストデータ
public class Json_AlbumDataList
{
    public List<Json_Album_ListNode> Data = new List<Json_Album_ListNode>();
}