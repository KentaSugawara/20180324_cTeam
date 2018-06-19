using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkisincamera : MonoBehaviour {

	public void Take() {

		var eggBehaviour = GetComponent<EggBehaviour>();
		//Debug.Log("isInCamera : " + eggBehaviour.isInCamera);
		//Debug.Log("isInCameraForSnap :" + eggBehaviour.isInCameraForSnap);
		Debug.Log("isFaceToCamera : " + eggBehaviour.isFaceToCamera);
	}
}
