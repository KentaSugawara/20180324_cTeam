using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Number of Collisions : 2
/// 
/// </summary>
public class SeeSawBehaviour : ItemBaseBehaviour {

    protected override void Awake() {
        base.Awake();
        _childrenColliders[0].enabled = true;
        _childrenColliders[1].enabled = false;
    }

    void Start() {

    }

    protected override void OnTriggerEnter(Collider other) {
        base.OnTriggerEnter(other);

        if (other.tag == "Egg") {
            if (_childrenColliders[0].enabled) {
                _childrenColliders[0].enabled = false;
                _childrenColliders[1].enabled = true;
                
                _eggObjects[0].GetComponent<EggBehaviour>().PlayItem(triggerName["SeeSawA"]);

                return;
            }
            if (_childrenColliders[1].enabled) {
                _childrenColliders[1].enabled = false;
                other.gameObject.GetComponent<EggBehaviour>().PlayItem(triggerName["SeeSawB"]);

                _eggObjects[0].GetComponent<Animator>().SetTrigger("EndWait");

                PlayAnimation();

                return;
            }
        }
    }

    public override void ResetColliders() {
        //base.ResetColliders();
        _childrenColliders[0].enabled = true;
        _childrenColliders[1].enabled = false;
    }
}
