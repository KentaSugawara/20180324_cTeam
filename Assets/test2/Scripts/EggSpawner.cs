using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggSpawner : MonoBehaviour
{
    List<GameObject> _eggList = new List<GameObject>();
    [SerializeField]
    Camera _camera;

    [SerializeField]
    GameObject _eggObj = null;
    [SerializeField]
    GameObject _markerlessObj = null;
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
            var ml_t = _markerlessObj.transform;
            if (_eggList.Count == 0) _spawnDist = ml_t.position.z;

            var spawnPos = new Vector3(1, 0, 1) * _spawnDist;

            var obj = Instantiate(_eggObj, _eggObj.transform.position, _eggObj.transform.rotation, ml_t);
            obj.GetComponent<EggBehaviour>()._camera = _camera;
            //obj.transform.SetParent(t, false); // don't destroy on load の オブジェクトには設定できない？

            obj.transform.localPosition = new Vector3(spawnPos.x, 0, spawnPos.z);
            obj.transform.localRotation = _eggObj.transform.rotation;
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
