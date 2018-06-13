using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// たまごが衝突すると たまごのステートを変更し このオブジェクトを非アクティブ化する
/// </summary>
public class TargetCollider : MonoBehaviour {

	[Header("たまごが移行するステート")]
	[SerializeField]
	EggBehaviour.EggState _playingState;

	[Header("全員揃ってアニメーションを開始するとき")]
	[Header("最後に到着するコライダーか")]
	[Space(1)]
	[SerializeField]
	bool _lastCollider;
	
	[Header("アイテムを待機状態にするか")]
	[Space(1)]
	[SerializeField]
	bool _setItemToWaiting;

	[Header("アイテムのアニメーションを開始するか")]
	[SerializeField]
	bool _itemAnimStart;

	[Header("次にアクティブ化するオブジェクト")]
	[SerializeField]
	GameObject _nextTargetObject;

	private void OnTriggerEnter(Collider other) {
		if(other.tag == "Egg") {
			GetComponentInParent<BaseItemBehaviour>()._eggList.Add(other.gameObject);

			other.GetComponent<Animator>().SetBool("Playing", true);
			other.GetComponent<Animator>().SetBool("Waiting", true);
			other.GetComponent<EggBehaviour>().SetTrigger(_playingState);

			other.GetComponent<EggBehaviour>().StopAgent();

			other.transform.position = transform.parent.transform.position;
			other.transform.localRotation = transform.parent.transform.localRotation;

			if (_setItemToWaiting) GetComponentInParent<BaseItemBehaviour>().SetTrigger(BaseItemBehaviour.ItemState.Wait);
			if (_lastCollider) GetComponentInParent<BaseItemBehaviour>().StartEggsAnimation();
			if (_itemAnimStart) GetComponentInParent<BaseItemBehaviour>().SetTrigger(BaseItemBehaviour.ItemState.Play);
			if (_nextTargetObject) _nextTargetObject.SetActive(true);

			gameObject.SetActive(false);
		}
	}
}