using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChallengeAsset_SnapEggs", menuName = "ChallengeAsset_SnapEggs", order = 1)]
public class ChallengeAsset_SnapEggs : Main_Challenge {
    [Header("設定したキャラが同時に写っている")]
    [SerializeField]
    private string _Comment;

    [SerializeField]
    private List<int> EggCloseIDs;

    public override bool Check(List<KeyValuePair<GameObject, SnapShotInfo>> SnapShots)
    {
        if (SnapShots == null && SnapShots.Count <= 0) return false;
        if (EggCloseIDs == null || EggCloseIDs.Count <= 0) return false;

        //ディープコピー
        var eggs = new List<int>(EggCloseIDs);

        for (int i = 0; i < eggs.Count; ++i)
        {
            var info = SnapShots.Find(c => c.Value.CharaCloseIndex == eggs[i]);
            if (info.Value != null)
            {
                eggs.RemoveAt(i);
            }
        }

        return eggs.Count <= 0;
    }
}
