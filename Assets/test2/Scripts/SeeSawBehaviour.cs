using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ターゲットの数（ColliderObject） : 2
/// </summary>
public class SeeSawBehaviour : BaseItemBehaviour {

	public override void ResetColliderList() {
		//base.ResetColliders();
		_colliderList[0].SetActive(true);
		_colliderList[1].SetActive(false);
	}

	public override void SetItemToWaiting() {
		_colliderList[1].SetActive(true);
	}
}
