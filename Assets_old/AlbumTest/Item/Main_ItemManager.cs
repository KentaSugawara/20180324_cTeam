using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_ItemManager : MonoBehaviour {
    private static Main_DataFileManager DatafileManager;
    public static Json_Item_DataList ItemData { get; private set; }

    public static void Init(Main_DataFileManager DatafileManager)
    {
        Main_ItemManager.DatafileManager = DatafileManager;
        UpdateFromJson();
    }

    public static void UpdateFromJson()
    {
        ItemData = DatafileManager.Load_ItemData();
    }
}
