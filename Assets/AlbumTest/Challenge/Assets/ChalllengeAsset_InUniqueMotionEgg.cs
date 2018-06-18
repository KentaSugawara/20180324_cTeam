using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChalllengeAsset_InUniqueMotionEgg", menuName = "ChalllengeAsset_InUniqueMotionEgg", order = 1)]
public class ChalllengeAsset_InUniqueMotionEgg : Main_Challenge {
    [Header("設定したキャラが固有モーションで写っている")]
    [SerializeField]
    private string _Comment;

    [SerializeField]
    private int EggCloseID;

    public override bool Check(List<KeyValuePair<GameObject, SnapShotInfo>> SnapShots)
    {
        if (SnapShots == null && SnapShots.Count <= 0) return false;
        if (EggCloseID < 0) return false;

        foreach (var s in SnapShots)
        {
            if (s.Value.CharaState == NavMeshCharacter.eCharaState.inUnique &&
                s.Value.CharaCloseIndex == EggCloseID)
            {
                return true;
            }
        }
        return false;
    }
}
