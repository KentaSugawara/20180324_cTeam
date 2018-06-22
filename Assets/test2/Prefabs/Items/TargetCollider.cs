using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// たまごが衝突すると たまごのステートを変更し このオブジェクトを非アクティブ化する
/// </summary>
public class TargetCollider : MonoBehaviour {

	[Header("初期のコライダーのオンオフ")]
	[SerializeField]
	bool _initActive;

	[Header("たまごが移行するステート")]
	[SerializeField]
	EggBehaviour.EggState _playingState;

	[Header("全員揃ってアニメーションを開始するとき")]
	[Header("・最後に到着するコライダーか")]
	[Space(1)]
	[SerializeField]
	bool _lastCollider;

	[Header("アイテムのアニメーションを開始するか")]
	[SerializeField]
	bool _itemAnimStart;

	[Header("次にアクティブ化するコライダーのオブジェクト")]
	[SerializeField]
	GameObject _nextTargetObject;

	[Header("アクティブ化するまでの時間(秒)")]
	[SerializeField]
	float _delaySeconds;

	private void OnEnable() {
		SetActiveTarget(_initActive);
	}

	IEnumerator DelayMethod_Cor(float delaySeconds, Action action) {

		yield return new WaitForSeconds(delaySeconds);

		action();
	}

	void SetActiveTarget(GameObject obj, bool b) {
		obj.GetComponent<Collider>().enabled = b;
		obj.transform.GetChild(0).gameObject.SetActive(b);
	}
	void SetActiveTarget(bool b) {
		SetActiveTarget(gameObject, b);
	}

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Egg") {
			//Debug.LogError(1);
			var baseItemBehaviour = GetComponentInParent<BaseItemBehaviour>();
			baseItemBehaviour._eggList.Add(other.gameObject);
			
			other.GetComponent<Animator>().SetBool("Waiting", true);
			other.GetComponent<EggBehaviour>().SetTrigger(_playingState);

			other.GetComponent<EggBehaviour>().StopAgent(baseItemBehaviour._ID);

			other.transform.position = transform.parent.transform.position;
			other.transform.rotation = transform.parent.transform.rotation;

			if (_lastCollider) {
				var eggList = baseItemBehaviour._eggList;
				eggList.ForEach(egg => egg.GetComponent<Animator>().SetBool("Waiting", false));
				eggList.Clear();
			}
			if (_itemAnimStart) baseItemBehaviour.PlayAnimation();
			if (_nextTargetObject) StartCoroutine(DelayMethod_Cor(_delaySeconds, () => SetActiveTarget(_nextTargetObject, true)));
			
			SetActiveTarget(false);
		}
	}
}