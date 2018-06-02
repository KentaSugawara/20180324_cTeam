using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggSearcher : MonoBehaviour
{
    [SerializeField]
    EggSpawner m_eggSpawner = null;
    [SerializeField]
    float m_spawnTime = 3;

    bool isSerching = false;
    public bool IsSearching { get { return isSerching; } }

    void Start()
    {
        StartCoroutine(SearchCoroutine());
    }

    IEnumerator SearchCoroutine()
    {
        while (true)
        {
            if (isSerching)
            {
                m_eggSpawner.Spawn();

                yield return new WaitForSeconds(m_spawnTime);
            }
            yield return null;
        }
    }

    //
    public void StartSearching()
    {
        isSerching = true;
    }

    public void StopSearching()
    {
        isSerching = false;
        m_eggSpawner.DestroyAllObjects();
    }
}
