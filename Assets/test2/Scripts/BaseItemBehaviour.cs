using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItemBehaviour : MonoBehaviour {

	public int _ID = 0;

	[System.NonSerialized]
	public List<GameObject> _eggList = new List<GameObject>();

	protected Animator _animator;


	protected virtual void Awake() {
		_animator = GetComponent<Animator>();
	}

	public void PlayAnimation() { _animator.SetTrigger("Play"); }
}
