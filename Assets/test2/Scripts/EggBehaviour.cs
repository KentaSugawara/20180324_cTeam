using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GoogleARCore;

public class EggBehaviour : MonoBehaviour {
	[SerializeField]
	Vector3 _tuneParams;

	public Camera _camera { get; set; }

	public bool _isTaken { get; set; }

	[SerializeField]
	Transform _rigTransform;

	Vector3 _rigPos = Vector3.zero;

	GameObject _item;

	public Animator _animator { get; private set; }

	public enum EggState {
		// move state
		Idle, Walk, Run, Jump,

		// item animation state
		Playing, Waiting,
		CampFireA, CampFireB, CampFireC,
		SeeSawA, SeeSawB,
		Slide,
		Trampoline,
		Wheel,
		WoodBlockA,
	}

	static Dictionary<EggState, string> triggers = new Dictionary<EggState, string>(){
		{ EggState.Idle,        "Idle" },
		{ EggState.Walk,		"Walk" },
		{ EggState.Run,			"Run" },
		{ EggState.Jump,		"Jump" },
		{ EggState.CampFireA,   "Play_CampFire_A" },
		{ EggState.CampFireB,   "Play_CampFire_B" },
		{ EggState.CampFireC,   "Play_CampFire_C" },
		{ EggState.SeeSawA,     "Play_SeeSaw_A"	  },
		{ EggState.SeeSawB,     "Play_SeeSaw_B"	  },
		{ EggState.Slide,       "Play_Slide"	  },
		{ EggState.Trampoline,  "Play_Trampoline" },
		{ EggState.Wheel,       "Play_Wheel"	  },
		{ EggState.WoodBlockA,  "Play_WoodBlock_A"  },
	};

	//
	// methods
	//
	void Awake() {
		_isTaken = false;
		_animator = GetComponent<Animator>();
	}

	bool CheckForward() {
		List<DetectedPlane> planeList = new List<DetectedPlane>();
		Session.GetTrackables<DetectedPlane>(planeList);

		RaycastHit hit;
		Vector3 originPos = transform.position + Vector3.up * 0.2f;
		Vector3 vec = transform.TransformDirection(new Vector3(0, -1, 1));

		if (Physics.Raycast(originPos, vec, out hit)) {
			Debug.Log(hit);
			Debug.DrawRay(originPos, vec, Color.yellow);
			return true;
		}

		Debug.DrawRay(originPos, vec, Color.red);
		return false;
	}


	public void OnTriggerEnter(Collider other) {
		if (other.tag == "Item") {
			_item = other.gameObject;
		}
	}

	public void SetTrigger(EggState state) {
		_animator.SetTrigger(triggers[state]);
	}

	public void SetTransformFromItem() {
		transform.position = _item.transform.parent.transform.position;
		transform.localRotation = _item.transform.parent.transform.localRotation;
	}

	public void GetBodyPosition() {
		_rigPos = _rigTransform.position;
	}

	public void SetBodyPosition() {
		transform.position = _rigPos;
	}

	public void KillSelf() {
		foreach (var egg in EggSpawnerARCore.EggList) {
			if (egg == gameObject) {
				EggSpawnerARCore.EggList.Remove(egg);
				break;
			}
		}
		Destroy(gameObject);
	}

	public void PlayAgent() {
		GetComponent<NavMeshAgent>().enabled = true;
		GetComponent<NavMeshCharacter>().Play();
	}
	public void StopAgent() {
		GetComponent<NavMeshCharacter>().Stop();
		GetComponent<NavMeshAgent>().enabled = false;
	}

	//
	// property
	//
	public bool isInCamera {
		get {
			var M_V = _camera.worldToCameraMatrix;
			var M_P = _camera.projectionMatrix;
			var M_VP = M_P * M_V;

			var pos = transform.position;
			var p = M_VP * new Vector4(pos.x, pos.y, pos.z, 1.0f);

			if (p.w == 0) return true;

			var x = p.x / p.w;
			var y = p.y / p.w;
			var z = p.z / p.w;

			if (x < -_tuneParams.x) return false;
			if (x > _tuneParams.x) return false;
			if (y < -_tuneParams.y) return false;
			if (y > _tuneParams.y) return false;
			if (z < -_tuneParams.z) return false;
			if (z > _tuneParams.z) return false;

			return true;
		}
	}

	public GameObject targetItem {
		get { return _item; }
	}

	void CheckTrigger(string name) { _animator.SetTrigger(name); }
}
