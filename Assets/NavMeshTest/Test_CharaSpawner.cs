using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_CharaSpawner : MonoBehaviour {
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
    public List<GameObject> Instancs
    {
        get { return _Instances; }
    }

    [SerializeField]
    private NavMeshCharacter.AnimationStrings AnimStrings;

    private void Awake()
    {
        NavMeshCharacter.Init(AnimStrings);
    }

    // Use this for initialization
    void Start () {
        StartCoroutine(Routine_Spawn());
	}
    private IEnumerator Routine_Spawn()
    {
        while (true)
        {
            while (_Instances.Count >= _NumOfMaxSpawn) yield return null;

            Ray ray = new Ray(new Vector3(Random.Range(_Start.x, _End.x), Random.Range(_Start.y, _End.y), Random.Range(_Start.z, _End.z)), Vector3.down);
            RaycastHit hit;
            Debug.Log("Ray" + ray.origin + " " + ray.direction);
            Debug.DrawRay(ray.origin, ray.direction);

            if (Physics.Raycast(ray, out hit, 1000.0f, 1 << 12))
            {
                var obj = Instantiate(_Prefabs[Random.Range(0, _Prefabs.Count)], hit.point, Quaternion.FromToRotation(Vector3.forward, Vector3.up));
                _Instances.Add(obj);
            }
            yield return new WaitForSeconds(_Interval);
        }
    }
}
