using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HARIBOTE : MonoBehaviour {

	[SerializeField]
	private GameObject[] _activateObjects;

	[SerializeField]
	private GameObject[] _inactivateObjects;


	// Copy of Test_CharaSpawner by sugachan -------
	[SerializeField]
	private Vector3 _Start;

	[SerializeField]
	private Vector3 _End;

	[SerializeField]
	private float _Interval;

	[SerializeField]
	private int _NumOfMaxSpawn;

	[SerializeField]
	private Renderer _Renderer;

	[SerializeField]
	private List<GameObject> _Prefabs;

	[SerializeField]
	private List<GameObject> _Instances = new List<GameObject>();
	public List<GameObject> Instancs {
		get { return _Instances; }
	}
	// -----------------------------------------------


	private void Awake() {
		foreach (var obj in _inactivateObjects) {
			obj.SetActive(false);
		}
		foreach (var obj in _activateObjects) {
			obj.SetActive(true);
		}
	}

	private void Start() {
		foreach (var eggbehaviour in GetComponentsInChildren<EggBehaviour>()) {
			eggbehaviour._HARIBOTE = true;
		}
		foreach (var navmeshcharacter in GetComponentsInChildren<NavMeshCharacter>()) {
			navmeshcharacter._HARIBOTE = true;
			var nav_t = navmeshcharacter.gameObject.transform;

			var empty = new GameObject("empty");
			empty.transform.SetParent(nav_t.parent.transform);
			nav_t.SetParent(empty.transform);
		}

		// Copy of Test_CharaSpawner by sugachan
		StartCoroutine(Routine_Spawn());
	}

	private Ray _ray = new Ray();

	private void Update() {

		Debug.DrawRay(_ray.origin, _ray.direction, new Color(1, 0, 0));
	}

	// Copy of Test_CharaSpawner by sugachan
	private IEnumerator Routine_Spawn() {
		while (true) {
			while (_Instances.Count >= _NumOfMaxSpawn) yield return null;

			Ray ray = new Ray(new Vector3(Random.Range(_Start.x, _End.x), Random.Range(_Start.y, _End.y), Random.Range(_Start.z, _End.z)), Vector3.down);
			RaycastHit hit;
			Debug.Log("Ray" + ray.origin + " " + ray.direction);
			_ray = ray;

			if (Physics.Raycast(ray, out hit, 1000.0f, 1 << 12)) {
				var obj = Instantiate(_Prefabs[Random.Range(0, _Prefabs.Count)], hit.point, Quaternion.FromToRotation(Vector3.forward, Vector3.up));

				var empty = new GameObject("empty");
				obj.transform.parent = empty.transform;
				obj.GetComponent<EggBehaviour>()._HARIBOTE = true;
				obj.GetComponent<NavMeshCharacter>()._HARIBOTE = true;

				_Instances.Add(obj);
			}
			yield return new WaitForSeconds(_Interval);
		}
	}
}

