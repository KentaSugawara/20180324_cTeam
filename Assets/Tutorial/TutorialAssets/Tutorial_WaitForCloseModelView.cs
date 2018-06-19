using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_WaitForCloseModelView : TutorialMethod {
    [SerializeField]
    private GameObject _ModelView;

    [SerializeField]
    private GameObject _HideObject;

    [SerializeField]
    private float _WaitSeconds;

    [SerializeField]
    private float _DelaySeconds;

    public override void Method(System.Action endcallback)
    {
        StartCoroutine(Routine_Wait(endcallback));
    }

    private IEnumerator Routine_Wait(System.Action endcallback)
    {
        yield return new WaitForSeconds(_WaitSeconds);

        if (_HideObject) _HideObject.SetActive(false);

        while (_ModelView.activeInHierarchy)
        {
            yield return null;
        }

        if (_HideObject) _HideObject.SetActive(true);

        yield return new WaitForSeconds(_DelaySeconds);
        _ModelView.SetActive(false);
        endcallback();
    }
}
