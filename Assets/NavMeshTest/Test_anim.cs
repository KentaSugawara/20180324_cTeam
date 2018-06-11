using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_anim : MonoBehaviour {
    [SerializeField]
    private Transform _rig;

    [SerializeField]
    private Transform _target;

    [SerializeField]
    private Animator _Animtor;

    public void Go()
    {
        var pos = _rig.transform.position;
        _target.transform.position = pos;
        _Animtor.CrossFade("Idle", 0.0f, 0);
    }
}
