using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourWithChildCollider : MonoBehaviour {
    
    EggBehaviour _parentScript;

    private void OnEnable()
    {
        _parentScript = GetComponentInParent<EggBehaviour>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _parentScript.OnTriggerEnterOnChild(other);
    }
}
