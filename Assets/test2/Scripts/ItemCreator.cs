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

    GameObject _activeObject;

	void Start () {
	}

    public void Create()
    {
        var m_transform = _markerlessObj.transform;
        var obj = Instantiate(_items[0], m_transform);
        _activeObject = obj;
    }

    public void Destroy()
    {
        Destroy(_activeObject);
    }
}
