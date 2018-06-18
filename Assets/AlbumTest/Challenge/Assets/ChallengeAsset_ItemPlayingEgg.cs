using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChallengeAsset_PlayingItemEgg", menuName = "ChallengeAsset_PlayingItemEgg", order = 1)]
public class ChallengeAsset_ItemPlayingEgg : Main_Challenge {
    [Header("設定したアイテムでキャラが遊んでいるところが写っている")]
    [SerializeField]
    private string _Comment;

    [SerializeField]
    private int ItemCloseID;

    public override bool Check(List<KeyValuePair<GameObject, SnapShotInfo>> SnapShots)
    {
        if (SnapShots == null && SnapShots.Count <= 0) return false;
        if (ItemCloseID < 0) return false;

        foreach (var s in SnapShots)
        {
            if (s.Value.CharaState == NavMeshCharacter.eCharaState.isItemPlaying &&
                s.Value.ItemCloseIndex == ItemCloseID &&
                s.Value.CharaCloseIndex >= 0)
            {
                return true;
            }
        }
        return false;
    }
}