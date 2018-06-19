using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HARIBOTE : MonoBehaviour {

	[SerializeField]
	GameObject[] _activateObjects;

	[SerializeField]
	GameObject[] _inactivateObjects;

	[SerializeField]
	EggSpawnerARCore _eggspawner;

	void Awake() {
		foreach (var obj in _inactivateObjects) {
			obj.SetActive(false);
		}
		foreach (var obj in _activateObjects) {
			obj.SetActive(true);
		}
	}

	private void Start() {
		for (int i = 0; i < 10; i++) {
			_eggspawner.Spawn();
		}
	}
}
