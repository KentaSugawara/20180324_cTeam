using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_ItemManager : MonoBehaviour {
    private static Main_DataFileManager DatafileManager;
    public static Assets_ItemList ItemList { get; private set; }
    public static Json_Item_DataList ItemSaveData { get; private set; }
    private static Main_ItemViewer ItemViewer;

    public static void Init(Main_DataFileManager DatafileManager, Main_ItemViewer Viewer, Assets_ItemList Asset)
    {
        Main_ItemManager.DatafileManager = DatafileManager;
        Main_ItemManager.ItemList = Asset;
        Main_ItemManager.ItemViewer = Viewer;
        UpdateFromJson();

        //セーブデータを補完する
        {
            foreach (var node in Asset.ItemList)
            {
                bool isExist = false;
                for (int i = 0, size = ItemSaveData.Data.Count; i < size; ++i)
                {
                    if (ItemSaveData.Data[i].CloseID == node.CloseID)
                    {
                        isExist = true;
                        break;
                    }
                }

                //無かったら追加
                if (!isExist)
                {
                    var data = new Json_Item_ListNode();
                    data.CloseID = node.CloseID;
                    ItemSaveData.Data.Add(data);
                }
            }
        }
    }

    //public void getItem(int CloseID)
    //{
    //    var savedata = ItemSaveData.Data.Find(c => c.CloseID == CloseID);
    //    savedata.isActive = 
    //}

    /// <summary>
    /// アイコンにNewを表示するかどうかを更新
    /// </summary>
    public static void UpdateisNew()
    {
        ItemViewer.SetNew(NumOfNew);
    }

    public static int NumOfNew
    {
        get
        {
            int num = 0;
            foreach (var c in ItemSaveData.Data)
            {
                if (c.isNewActive == true) ++num;
            }
            return num;
        }
    }

    public static void UpdateFromJson()
    {
        ItemSaveData = DatafileManager.Load_ItemData();
    }

    /// <summary>
    /// アイテムを所持しているかどうかを更新する
    /// </summary>
    public static void CheckUpdateItems()
    {
        int NumOfChallengeClear = Main_ChallengeManager.NumOfClear;
        foreach (var node in ItemList.ItemList)
        {
            var savedata = ItemSaveData.Data.Find(i => i.CloseID == node.CloseID);
            if (savedata != null)
            {
                //達成されているかどうか
                if (node.Need_NumOf_ChallengeClear <= Main_ChallengeManager.NumOfClear)
                {
                    //達成されていたら
                    if(!savedata.isActive)
                    {
                        savedata.isActive = true;
                        savedata.isNewActive = true;
                    }
                }
                else
                {
                    savedata.isActive = false;
                    savedata.isNewActive = false;
                }

            }
        }

        UpdateisNew();
    }
}
