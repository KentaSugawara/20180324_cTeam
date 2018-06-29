using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChallengeAsset_PlayingItemEgg", menuName = "ChallengeAsset_PlayingItemEgg", order = 1)]
public class ChallengeAsset_ItemPlayingEgg : Main_Challenge {
    [Header("設定したアイテムでキャラが遊んでいるところが写っている")]
    [SerializeField]
    private string _Comment;

    [SerializeField]
    private int _NumOfChara;

    [SerializeField]
    private int ItemCloseID;

    [SerializeField]
    private int _CharaCloseID = -1;

    [SerializeField]
    private bool _CheckAnimatorState;

    [SerializeField]
    private string _AnimatorStateName = "";

    public override bool Check(List<KeyValuePair<GameObject, SnapShotInfo>> SnapShots)
    {
        if (SnapShots == null || SnapShots.Count <= 0) return false;
        if (ItemCloseID < 0) return false;

        int cnt = 0;
        foreach (var s in SnapShots)
        {
            if (s.Value.CharaState == NavMeshCharacter.eCharaState.isItemPlaying &&
                s.Value.ItemCloseIndex == ItemCloseID &&
                s.Value.CharaCloseIndex >= 0)
            {
                if (_CharaCloseID < 0 || s.Value.CharaCloseIndex == _CharaCloseID)
                {
                    if (!_CheckAnimatorState || _AnimatorStateName == "") {
                        ++cnt;
                    }
                    else
                    {
                        if (s.Value._Animator == null) continue;
                        var info = s.Value._Animator.GetCurrentAnimatorStateInfo(0);
                        if (info.IsName(_AnimatorStateName))
                        {
                            ++cnt;
                        }
                    }
                }
            }
        }
        return _NumOfChara <= cnt;
    }
}