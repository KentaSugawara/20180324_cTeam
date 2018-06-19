using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_WaitForSeconds : TutorialMethod {
    [SerializeField]
    private float _WaitSeconds;

    public override void Method(System.Action endcallback)
    {
        StartCoroutine(Routine_Wait(endcallback));
    }

    private IEnumerator Routine_Wait(System.Action endcallback)
    {
        yield return new WaitForSeconds(_WaitSeconds);
        endcallback();
    }
}
