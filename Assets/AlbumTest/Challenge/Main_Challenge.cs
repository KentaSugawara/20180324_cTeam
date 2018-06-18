using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Challenge : ScriptableObject {
    public virtual bool Check(List<KeyValuePair<GameObject, SnapShotInfo>> SnapShots)
    {
        return false;
    }
}

public class SnapShotInfo
{
    public int? CharaCloseIndex = null;
    public NavMeshCharacter.eCharaState CharaState = NavMeshCharacter.eCharaState.isWaiting;
    public int? ItemCloseIndex = null;
}
