using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggSpawner : MonoBehaviour
{
    List<GameObject> _eggList = new List<GameObject>();
    [SerializeField]
    Camera _camera;

    [SerializeField]
    GameObject[] _eggObjs = null;
    [SerializeField]
    GameObject _eggsParentTransform = null;
    [SerializeField]
    int _maxNum = 1;

    float _spawnDist = 0;

    void Start()
    {

    }

    void Update()
    {
        foreach (var egg in _eggList)
        {
            var eggBehavour = egg.GetComponent<EggBehaviour>();
            if (eggBehavour._isTaken && !eggBehavour.isInCamera)
            {
                _eggList.Remove(egg);
                Destroy(egg);
            }
        }
    }


    // 画面外にスポーンさせたい
    public void Spawn()
    {
        if (_eggList.Count < _maxNum)
        {
            var t_ml = _eggsParentTransform.transform;
            if (_eggList.Count == 0) _spawnDist = t_ml.position.z;

            var rand_value = Random.Range(0.8f, 1.5f);
            if (Random.Range(0, 100) < 50) rand_value *= -1;

            var spawnVec = new Vector3(rand_value, 0, 1);
            var spawnPos = spawnVec * _spawnDist;

            var randomEggObj = _eggObjs[Random.Range(0, _eggObjs.Length)];
            var obj = Instantiate(randomEggObj, t_ml);
            obj.GetComponent<EggBehaviour>()._camera = _camera;
            //obj.transform.SetParent(t, false); // don't destroy on load の オブジェクトには設定できない？

            obj.transform.localPosition = new Vector3(spawnPos.x, 0, Random.Range(-100, 100));
            obj.transform.localRotation = randomEggObj.transform.localRotation;
            _eggList.Add(obj);
        }
    }

    public void DestroyAllObjects()
    {
        if (_eggList.Count > 0)
        {
            foreach (var obj in _eggList)
            {
                if (obj) Destroy(obj);
            }
            _eggList.Clear();
        }
    }

    public void CheckEggsInCamera()
    {
        foreach (var egg in _eggList)
        {
            if (!egg)
            {
                _eggList.Remove(egg);
                continue;
            }
            var eggBehaviour = egg.GetComponent<EggBehaviour>();
            if (eggBehaviour.isInCamera) eggBehaviour._isTaken = true;
            Debug.Log(eggBehaviour._isTaken);
        }
    }

    public List<GameObject> EggList { get { return _eggList; } }
    public int MaxNum { get { return _maxNum; } }
}
