using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggSpawnField : MonoBehaviour {
    [SerializeField]
    private float _UnitOfArea;

    private MeshFilter _MeshFilter;
    private List<GameObject> _EggList= new List<GameObject>();

    private void Awake()
    {
        _MeshFilter = GetComponent<MeshFilter>();
    }

    [SerializeField]
    private float areaaaa;
    private void Update()
    {
        var extents = _MeshFilter.mesh.bounds.extents;
        areaaaa = extents.x * 2.0f * extents.z * 2.0f;
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

    public void Spawn(GameObject obj)
    {
        _EggList.Add(obj);
    }

    public bool checkSpawnable()
    {
        var NumOfEggs = getNumOfEggs();

        int MaxSpawn = 0;
        //最大数を計算
        {
            var extents = _MeshFilter.mesh.bounds.extents;
            var area = extents.x * 2.0f * extents.z * 2.0f;

            MaxSpawn = (int)(area / _UnitOfArea);
            if (MaxSpawn <= 0) MaxSpawn = 1;
        }

        return MaxSpawn > NumOfEggs;
    }
}
