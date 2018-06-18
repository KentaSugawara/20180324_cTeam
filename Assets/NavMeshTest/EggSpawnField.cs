using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggSpawnField : MonoBehaviour {
    [SerializeField]
    private int _UnitOfArea;

    private MeshFilter _MeshFilter;
    private List<GameObject> _EggList;

    private void Awake()
    {
        _MeshFilter = GetComponent<MeshFilter>();
    }

    public int getNumOfEggs()
    {
        for (int i = 0; i < _EggList.Count; ++i)
        {
            if (_EggList[i] == null)
            {
                _EggList.RemoveAt(i);
                --i;
            }
        }

        return _EggList.Count;
    }
}
