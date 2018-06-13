using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ターゲットの数（ColliderObject） : 3
/// </summary>
public class CampFireBehaviour : BaseItemBehaviour {

	public override void ResetColliderList() {
		//base.ResetColliders();
		_colliderList[0].SetActive(true);
		_colliderList[1].SetActive(false);
		_colliderList[2].SetActive(false);
	}
}
