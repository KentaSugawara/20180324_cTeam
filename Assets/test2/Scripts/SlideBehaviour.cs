using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ターゲットの数（ColliderObject） : 1
/// </summary>
public class SlideBehaviour : BaseItemBehaviour {

	public override void ResetColliderList() {
		//base.ResetColliders();
		_colliderList[0].SetActive(true);
	}
}
