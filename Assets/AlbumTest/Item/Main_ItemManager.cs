using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_ItemManager : MonoBehaviour {
    private static Main_DataFileManager DatafileManager;
    private static Assets_ItemList ItemList;
    public static Json_Item_DataList ItemSaveData { get; private set; }

    public static void Init(Main_DataFileManager DatafileManager, Assets_ItemList Asset)
    {
        Main_ItemManager.DatafileManager = DatafileManager;
        Main_ItemManager.ItemList = Asset;
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

    public static void UpdateFromJson()
    {
        ItemSaveData = DatafileManager.Load_ItemData();
    }
}
