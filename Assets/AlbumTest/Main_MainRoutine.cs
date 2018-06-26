﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_MainRoutine : MonoBehaviour {
    [SerializeField]
    private Main_DataFileManager _DataFileManager;

    [SerializeField]
    private Assets_ChallengeList _ChallengeList;

    [SerializeField]
    private Main_ChallengeViewer _ChallengeViewer;

    [SerializeField]
    private Assets_ItemList _ItemList;

    [SerializeField]
    private Main_ItemViewer _ItemViewer;

    [SerializeField]
    private Main_PictureBookViewer _PictureBookViewer;

    [SerializeField]
    private Assets_CharacterList _CharacterList;

    [SerializeField]
    private Main_NoticeViewer _NoticeViewer;

    [SerializeField]
    private bool _isMainScene;

    private void Awake()
    {
        if (_isMainScene)
        {
            var savedata = _DataFileManager.Load_SaveData();
            if (savedata.isAlreadyTutorial == false)
            {
                SceneManager.LoadScene("Tutorial");
            }
        }

        Main_ChallengeManager.Init(_DataFileManager, _ChallengeViewer, _ChallengeList, _NoticeViewer);
        Main_ItemManager.Init(_DataFileManager, _ItemViewer, _ItemList);
        Main_PictureBookManager.Init(_DataFileManager, _PictureBookViewer, _CharacterList);
    }
}
