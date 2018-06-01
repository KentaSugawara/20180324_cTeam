using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCreator : MonoBehaviour {

    [SerializeField]
    GameObject[] _items;
    [SerializeField]
    GameObject _markerlessObj;
    [SerializeField]
    EggSpawner _eggSpawner;

	void Start () {
	}

    public void Create()
    {
        var m_transform = _markerlessObj.transform;
        var obj = Instantiate(_items[0], m_transform);

        foreach(var egg in _eggSpawner.EggList)
        {
            egg.GetComponent<EggBehaviour>().targetItem = this.gameObject;
        }
    }
}
