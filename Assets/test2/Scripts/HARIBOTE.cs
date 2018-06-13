using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HARIBOTE : MonoBehaviour {

	[SerializeField]
	GameObject[] _inactivateObjects;

	void Awake () {
		foreach(var obj in _inactivateObjects) {
			obj.SetActive(false);
		}
	}
}
