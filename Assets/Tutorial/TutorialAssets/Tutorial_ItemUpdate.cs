using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_ItemUpdate : TutorialMethod {
    [SerializeField]
    private int _CharaCloseIndex;

    [SerializeField]
    private float _WaitSeconds;

    [SerializeField]
    private float _ViewSeconds;

    public void UpdateItems()
    {
        Main_ItemManager.CheckUpdateItems();
    }

    public override void Method(Action endcallback)
    {
        StartCoroutine(Routine_Method(endcallback));
    }

    private IEnumerator Routine_Method(Action endcallback)
    {
        yield return new WaitForSeconds(_WaitSeconds);
        UpdateItems();
        yield return new WaitForSeconds(_ViewSeconds);
        endcallback();
    }
}
