using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_MainRoutine : MonoBehaviour {
    [SerializeField]
    private Main_DataFileManager _DataFileManager;

    [SerializeField]
    private Assets_ChallengeList _ChallengeList;

    private void Start()
    {
        Main_ChallengeManager.Init(_DataFileManager, _ChallengeList);
    }
}
