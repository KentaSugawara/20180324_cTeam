using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureBookManagerTest : MonoBehaviour {
    private int cnt = 0;

    public void AddTest()
    {
        var s = new SnapShotInfo();
        s.CharaCloseIndex = cnt;
        var list = new List<KeyValuePair<GameObject, SnapShotInfo>>() { new KeyValuePair<GameObject, SnapShotInfo>(null, s) };
        Main_PictureBookManager.UpdateAlbum(list);
        Main_ChallengeManager.CheckChallenges(list);
        ++cnt;
    }
}
