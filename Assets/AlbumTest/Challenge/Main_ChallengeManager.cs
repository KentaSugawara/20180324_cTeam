﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Main_ChallengeManager {
    private static Main_DataFileManager DatafileManager;
    private static Assets_ChallengeList ChallengeList;
    private static Main_ChallengeViewer ChallengeViewer;
    private static Main_NoticeViewer NoiceViewer;
    public static Json_Challenge_DataList ChallengeSaveData { get; private set; }
    
    public static void Init(Main_DataFileManager DatafileManager, Main_ChallengeViewer Viewer, Assets_ChallengeList Asset, Main_NoticeViewer NoiceViewer)
    {
        Main_ChallengeManager.DatafileManager = DatafileManager;
        Main_ChallengeManager.ChallengeViewer = Viewer;
        Main_ChallengeManager.ChallengeList = Asset;
        Main_ChallengeManager.NoiceViewer = NoiceViewer;
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

    /// <summary>
    /// チャレンジが新しく達成されているかを調べる
    /// </summary>
    public static void CheckChallenges()
    {
        //全てのチャレンジを調べる
        foreach(var challenge in ChallengeList.ChallengeList)
        {
            //セーブデータから探す
            var savedata = ChallengeSaveData.Data.Find(c => c.CloseID == challenge.CloseID);
            if (savedata != null)
            {
                if (savedata.isCleard) continue;

                //まだされていないならば
                if (challenge.Challenge != null && challenge.Challenge.Check()) //判定
                {
                    //クリアされていたら
                    savedata.isCleard = true;
                    savedata.isNewCleard = true;

                    //ここで通知に流す
                    NoiceViewer.AddNotice("「" + challenge.Text +　"」くりあ！");
                }
            }
        }

        UpdateisNew();
    }

    /// <summary>
    /// アイコンにNewを表示するかどうかを更新
    /// </summary>
    public static void UpdateisNew()
    {
        foreach (var node in ChallengeSaveData.Data)
        {
            if (node.isNewCleard)
            {
                ChallengeViewer.SetNew(true);
                return;
            }
        }
        ChallengeViewer.SetNew(false);
    }
}