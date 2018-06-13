using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItemBehaviour : MonoBehaviour {

	public enum ItemState {
		Play,
		Wait,
	}

	protected Dictionary<ItemState, string> triggers = new Dictionary<ItemState, string>(){
		{ ItemState.Play,   "Play" },
		{ ItemState.Wait,   "Wait" },
	};

	[SerializeField]
	protected List<GameObject> _colliderList = new List<GameObject>();

	[System.NonSerialized]
	public List<GameObject> _eggList = new List<GameObject>();

	protected Animator _animator;


	protected virtual void Awake() {
		_animator = GetComponent<Animator>();
		ResetColliderList();
	}

	public void StartEggsAnimation() {
		_eggList.ForEach(egg => egg.GetComponent<Animator>().SetBool("Waiting", false));
	}

	public void SetTrigger(ItemState state) { _animator.SetTrigger(triggers[state]); }

	public virtual void ResetEggList() {
		_eggList.ForEach(egg => egg.GetComponent<Animator>().SetBool("Playing", false));
		_eggList.Clear();
	}

	public virtual void ResetColliderList() { }

	public virtual void SetItemToWaiting() { }
}
