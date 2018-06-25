using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial_ToMainScene : TutorialMethod {
    [SerializeField]
    private Main_DataFileManager _DataFileManager;

    [SerializeField]
    private string _NextSceneName;

    private bool _isMoving = false;

    public override void Method(Action endcallback)
    {
        NextScene();
    }

    public void NextScene()
    {
        if (!_isMoving)
        {
            _isMoving = true;

            var savedata = new Json_SaveData();
            savedata.isAlreadyTutorial = true;

            _DataFileManager.Save_SaveData(savedata);

            SceneManager.LoadScene(_NextSceneName);
        }  
    }
}
