using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Main_ChallengeManager {
    private static Main_DataFileManager DatafileManager;
    private static Assets_ChallengeList ChallengeList;
    public static Json_Challenge_DataList ChallengeSaveData { get; private set; }
    
    public static void Init(Main_DataFileManager DatafileManager, Assets_ChallengeList Asset)
    {
        Main_ChallengeManager.DatafileManager = DatafileManager;
        Main_ChallengeManager.ChallengeList = Asset;
        UpdateFromJson();

        //セーブデータを補完する
        {
            foreach(var node in Asset.ChallengeList)
            {
                bool isExist = false;
                for(int i = 0, size = ChallengeSaveData.Data.Count; i < size; ++i)
                {
                    if (ChallengeSaveData.Data[i].CloseID == node.CloseID)
                    {
                        isExist = true;
                        break;
                    }
                }

                //無かったら追加
                if (!isExist)
                {
                    var data = new Json_Challenge_ListNode();
                    data.CloseID = node.CloseID;
                    ChallengeSaveData.Data.Add(data);
                }
            }
        }
    }

    public static void UpdateFromJson()
    {
        ChallengeSaveData = DatafileManager.Load_ChallengeData();
    }
}
