using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Camera : MonoBehaviour {

    [SerializeField]
    private float _Interval;

    [SerializeField]
    private Test_CharaSpawner _CharaSpawner;

    private void Start()
    {
        StartCoroutine(Routine_Main());
    }

    private IEnumerator Routine_Main()
    {
        while (_CharaSpawner.Instancs.Count <= 0) yield return null;
        while (true)
        {
            transform.SetParent(_CharaSpawner.Instancs[Random.Range(0, _CharaSpawner.Instancs.Count)].transform, false);
            yield return new WaitForSeconds(_Interval);
        }
    }
}
