using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChallengeAsset_PictureBookCount" , menuName = "ChallengeAsset_PictureBookCount", order = 1)]
public class ChallengeAsset_PictureBookCount : Main_Challenge {
    [SerializeField]
    private int _NumOfDecision;

    public override bool Check()
    {
        return Main_PictureBookManager.countNumOfAlreadyFind() >= _NumOfDecision;
    }
}
