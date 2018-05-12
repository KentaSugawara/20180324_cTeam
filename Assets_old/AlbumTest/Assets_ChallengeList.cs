using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChallengeList", menuName = "ChallengeList")]
public class Assets_ChallengeList : ScriptableObject {
    public List<ChallengeData> ChallengeList = new List<ChallengeData>();
}

[System.Serializable]
public class ChallengeData
{
    public int CloseID; // 内部ID
    public int OpenID;  // 表示ID
    public string ViewName; //表示名
    public GameObject Prefab;
    public Sprite sprite;
    public Color Color = Color.white;
    public string Text;
}
