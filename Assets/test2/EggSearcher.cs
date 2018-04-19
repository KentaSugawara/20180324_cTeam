using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggSearcher : MonoBehaviour
{
    [SerializeField]
    Kudan.AR.KudanTracker m_kudanTracker = null;
    [SerializeField]
    EggSpawner m_eggSpawner = null;
    [SerializeField]
    float m_spawnTime = 3;

    static bool isSerching = false;
    public static bool IsSearching { get { return isSerching; } }

    void Start()
    {
        BackHome();
    }

    IEnumerator SearchCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(m_spawnTime);
            
            m_eggSpawner.Spawn();

            yield return null;
        }
    }

    //
    public void SearchingStart()
    {
        m_kudanTracker.StartTracking();
        StartCoroutine(SearchCoroutine());
        isSerching = true;
    }

    public void BackHome()
    {
        m_kudanTracker.StopTracking();
        StopAllCoroutines();
        isSerching = false;
    }
}
