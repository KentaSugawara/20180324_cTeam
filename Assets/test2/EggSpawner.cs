using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggSpawner : MonoBehaviour
{

    List<GameObject> m_eggList = new List<GameObject>();

    [SerializeField]
    GameObject m_eggObj = null;
    [SerializeField]
    GameObject m_markerlessObj = null;
    [SerializeField]
    int m_maxNum = 1;

    float m_spawnDist = 0;

    void Start()
    {

    }

    void Update()
    {

    }

    public void Spawn()
    {
        if (m_eggList.Count < m_maxNum)
        {
            // 画面外にスポーンさせたい
            var ml_t = m_markerlessObj.transform;
            if (m_eggList.Count == 0) m_spawnDist = ml_t.position.magnitude;

            var spawnPos = Quaternion.FromToRotation(Vector3.forward, new Vector3(0.5f, 0, 1)) * Camera.main.transform.forward * m_spawnDist;

            var obj = Instantiate(m_eggObj, m_eggObj.transform.position, m_eggObj.transform.rotation, ml_t);
            //obj.transform.SetParent(t, false); // don't destroy on load の オブジェクト内には設定できない？

            obj.transform.localPosition = new Vector3(spawnPos.x, ml_t.position.y, spawnPos.z);
            obj.transform.localRotation = m_eggObj.transform.rotation;
            m_eggList.Add(obj);
        }
    }

    public void DestroyAllObjects()
    {
        if (m_eggList.Count > 0)
        {
            foreach (var obj in m_eggList)
            {
                if (obj) Destroy(obj);
            }
            m_eggList.Clear();
        }
    }

    public List<GameObject> EggList { get { return m_eggList; } }
    public int MaxNum { get { return m_maxNum; } }
}
