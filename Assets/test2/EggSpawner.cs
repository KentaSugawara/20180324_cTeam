using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggSpawner : MonoBehaviour {

    List<GameObject> m_eggList = new List<GameObject>();

    [SerializeField]
    GameObject m_eggObj = null;
    [SerializeField]
    GameObject m_markerlessObj = null;
    [SerializeField]
    int m_maxNum = 1;

    void Start () {
		
	}
	
	void Update () {

    }

    public void Spawn()
    {
        if (m_eggList.Count < m_maxNum)
        {
            // 画面外にスポーンさせたい
            var mt = m_markerlessObj.transform;
            var spawnDist = mt.position.magnitude;
            //var mt_vec = mt.position.normalized;
            var random_pos = Quaternion.FromToRotation(Vector3.forward, new Vector3(0.5f, 0, 1)) * Camera.main.transform.forward * spawnDist;

            var obj = Instantiate(m_eggObj, m_eggObj.transform.position, m_eggObj.transform.rotation, mt);
            //obj.transform.SetParent(t, false); // don't destroy on load object 内には設定できない？
            obj.transform.localPosition = new Vector3(random_pos.x, mt.position.x, random_pos.z);
            obj.transform.localRotation = m_eggObj.transform.rotation;
            m_eggList.Add(obj);
            Debug.Log("Spawn : " + obj);
        }
    }

    public void DestroyAllObjects()
    {
        foreach (var obj in m_eggList)
        {
            if (obj) Destroy(obj);
        }
        m_eggList = new List<GameObject>();
    }

    public List<GameObject> EggList { get { return m_eggList; } }
    public int MaxNum { get { return m_maxNum; } }
}
