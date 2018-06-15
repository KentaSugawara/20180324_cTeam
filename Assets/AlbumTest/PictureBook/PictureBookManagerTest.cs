using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureBookManagerTest : MonoBehaviour {
    private int cnt = 0;

    public void AddTest()
    {
        Main_PictureBookManager.UpdateAlbum(new List<KeyValuePair<GameObject,int>>(){new KeyValuePair<GameObject, int>(null,cnt)});
        ++cnt;
    }
}
