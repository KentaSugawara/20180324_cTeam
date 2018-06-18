using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMethod : MonoBehaviour {
    [SerializeField]
    protected bool _isActive = true;

    public virtual void Method(System.Action endcallback)
    {
        endcallback();
    }
}
