﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_ChallengeUpdate : TutorialMethod {
    [SerializeField]
    private int _CharaCloseIndex;

    [SerializeField]
    private float _WaitSeconds;

    [SerializeField]
    private float _ViewSeconds;

    public void UpdateChallenge()
    {
        var s = new SnapShotInfo();
        s.CharaCloseIndex = _CharaCloseIndex;
        var list = new List<KeyValuePair<GameObject, SnapShotInfo>>() { new KeyValuePair<GameObject, SnapShotInfo>(null, s) };
        Main_ChallengeManager.CheckChallenges(list);
        //Main_ChallengeManager.CheckChallenges(list);
    }

    public override void Method(Action endcallback)
    {
        StartCoroutine(Routine_Method(endcallback));
    }

    private IEnumerator Routine_Method(Action endcallback)
    {
        yield return new WaitForSeconds(_WaitSeconds);
        UpdateChallenge();
        yield return new WaitForSeconds(_ViewSeconds);
        endcallback();
    }
}
