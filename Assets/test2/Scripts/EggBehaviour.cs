using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GoogleARCore;

public class EggBehaviour : MonoBehaviour {
	[SerializeField]
	public bool _HARIBOTE;

	[SerializeField]
	Vector3 _tuneParams;

	[SerializeField]
	Vector3 _tuneParamsForSnap;

	[SerializeField]
	float _dotParam;

	public Camera _camera { get; set; }

	public bool _isTaken { get; set; }

	[SerializeField]
	Transform _rigTransform;

	Vector3 _rigPos = Vector3.zero;
	Quaternion _rigRot = Quaternion.identity;
	NavMeshCharacter _NavMeshCharacter;
	int _ID_Playing = Animator.StringToHash("Playing");

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
		WoodBlockA, WoodBlockB, WoodBlockC,
	}

	static Dictionary<EggState, string> triggers = new Dictionary<EggState, string>(){
		{ EggState.Idle,        "Idle" },
		{ EggState.Walk,        "Walk" },
		{ EggState.Run,         "Run" },
		{ EggState.Jump,        "Jump" },
		{ EggState.CampFireA,   "Play_CampFire_A" },
		{ EggState.CampFireB,   "Play_CampFire_B" },
		{ EggState.CampFireC,   "Play_CampFire_C" },
		{ EggState.SeeSawA,     "Play_SeeSaw_A"   },
		{ EggState.SeeSawB,     "Play_SeeSaw_B"   },
		{ EggState.Slide,       "Play_Slide"      },
		{ EggState.Trampoline,  "Play_Trampoline" },
		{ EggState.Wheel,       "Play_Wheel"      },
		{ EggState.WoodBlockA,  "Play_WoodBlock_A"  },
		{ EggState.WoodBlockB,  "Play_WoodBlock_B"  },
		{ EggState.WoodBlockC,  "Play_WoodBlock_C"  },
	};

	//
	// methods
	//
	void Awake() {
		_isTaken = false;
		_animator = GetComponent<Animator>();
		_NavMeshCharacter = GetComponent<NavMeshCharacter>();
	}

	private void Start() {
			StartCoroutine(Routine_CheckDestroy());
	}

	IEnumerator Routine_CheckDestroy() {
		while (true) {
			yield return new WaitForSeconds(EggSpawnerARCore.DestroyCheckDelaySeconds);
			if (_HARIBOTE) yield break;
			//写真に写された後に画面外なら削除
			if (_isTaken && !_animator.GetBool(_ID_Playing) && !isInCamera) KillSelf();
			//プレイヤーから一定距離離れたら削除
			else if (Vector3.SqrMagnitude((Camera.main.transform.position - transform.position)) > EggSpawnerARCore.DistanceOfAlive * EggSpawnerARCore.DistanceOfAlive) KillSelf();
		}
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
	public void GetBodyRotation() {
		_rigRot = _rigTransform.rotation;
	}
	public void SetBodyRotation() {
		transform.rotation = _rigRot;
	}

	public void KillSelf() {
		//foreach (var egg in EggSpawnerARCore.EggList) {
		//	if (egg == gameObject) {
		//		EggSpawnerARCore.EggList.Remove(egg);
		//		break;
		//	}
		//}
		if (!_HARIBOTE) EggSpawnerARCore.Instance.RemoveEgg(gameObject);
		Destroy(gameObject);
	}

	public void PlayAgent() {
		GetComponent<NavMeshCharacter>().EndItemPlaying();
		_animator.SetBool("Playing", false);
		_animator.SetBool("Waiting", false);

	}
	public void StopAgent(int ItemCloseIndex) {
		GetComponent<NavMeshCharacter>().StartItemPlaying(ItemCloseIndex);
		_animator.SetBool("Playing", true);
		_animator.SetBool("Waiting", true);
	}

	public bool IsInCamera(Vector3 tuneParams) {

		var M_V = Camera.main.worldToCameraMatrix;
		var M_P = Camera.main.projectionMatrix;
		var M_VP = M_P * M_V;

		var pos = transform.position;
		var p = M_VP * new Vector4(pos.x, pos.y, pos.z, 1.0f);

		if (p.w == 0) return true;

		var x = p.x / p.w;
		var y = p.y / p.w;
		var z = p.z / p.w;

		if (x <= -1 - tuneParams.x) return false;
		if (x >= 1 + tuneParams.x) return false;
		if (y <= -1 - tuneParams.y) return false;
		if (y >= 1 + tuneParams.y) return false;
		if (z <= -1 - tuneParams.z) return false;
		if (z >= 1 + tuneParams.z) return false;

		return true;
	}

	public bool isInCamera { get { return IsInCamera(_tuneParams); } }

	public bool isInCameraForSnap {
		get {
			_isTaken = true;
			return IsInCamera(_tuneParamsForSnap);
		}
	}


	public bool isFaceToCamera {
		get {
			if (Vector3.Dot((transform.forward + transform.up * 0.2f).normalized, -Camera.main.transform.forward.normalized) > DotParam)
				return true;
			else
				return false;
		}
	}

	public GameObject targetItem {
		get { return _item; }
	}

	public float DotParam {
		get {
			return _dotParam;
		}

		set {
			_dotParam = value;
		}
	}

	void CheckTrigger(string name) { _animator.SetTrigger(name); }
}
