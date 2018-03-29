using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kudan.AR;

public class SerchingEgg : MonoBehaviour
{
    [SerializeField]
    GameObject m_eggObj = null;
    [SerializeField]
    GameObject m_driver = null;
    [SerializeField]
    GameObject m_markerlessObj = null;
    [SerializeField]
    KudanTracker m_kudanTracker = null;
    [Space]
    [SerializeField]
    MarkerlessTracking always;
    [SerializeField]
    MarkerlessTracking arbi;

    bool m_isAlwaysSearching = false;
    GameObject obj;

    IEnumerator Start()
    {
        m_kudanTracker.StopTracking();
        obj = Instantiate(m_eggObj, m_markerlessObj.transform.position + new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10)), m_eggObj.transform.rotation, m_markerlessObj.transform);
        yield break;
    }

    private void Update()
    {
        obj.transform.Translate(Vector3.right);
    }

    private void OnGUI()
    {

        GUILayout.Label("MarkerlessIsActive: " + m_markerlessObj.gameObject.activeSelf);
    }


    IEnumerator MainCoroutine()
    {
        obj.transform.position = m_eggObj.transform.position;
        ObjApper(true);
        m_kudanTracker.m_isSearching = true;

        if (!m_isAlwaysSearching)
        {
            m_kudanTracker.StopTracking();
            yield return new WaitForSeconds(1);
            m_kudanTracker.StartTracking();
            always.StartClicked();
            yield return new WaitForEndOfFrame();
            always.StartClicked();
        }
        else
        {
            arbi.StartClicked();
            yield return new WaitForEndOfFrame();
            arbi.StartClicked();
        }
        
        // キャラ表示
        
        
        m_kudanTracker.m_isSearching = false;
        yield return null;
    }


    IEnumerator AlwaysSearchCoroutine()
    {
        while (m_isAlwaysSearching)
        {
            yield return new WaitForSeconds(5);

            yield return StartCoroutine(MainCoroutine());
        }
    }

    public void SearchingStart()
    {
        StartCoroutine(MainCoroutine());
    }

    public void AlwaysIsActive(bool b)
    {
        m_isAlwaysSearching = b;
        if (m_isAlwaysSearching)
            StartCoroutine(AlwaysSearchCoroutine());
        else
        {
            StopCoroutine(AlwaysSearchCoroutine());
            StopCoroutine(MainCoroutine());
            m_kudanTracker.StartTracking();
        }
    }

    public void ObjApper(bool b)
    {
        if (b) obj.SetActive(true);
        else obj.SetActive(false);
    }
}
