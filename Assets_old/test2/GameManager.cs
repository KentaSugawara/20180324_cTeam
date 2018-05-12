using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] m_objs;


    IEnumerator Start()
    {
        //SetActiveAllObjects(false, m_objs);
        yield break;
    }

    IEnumerator MainCoroutine()
    {
        //if(Input.GetMouseButtonDown(0))
        //{
        //    SetActiveAllObjects(true, m_objs);
        //}
        return null;
    }

    void SetActiveAllObjects(bool b, GameObject[] objects)
    {
        foreach (var obj in objects)
        {
            obj.SetActive(b);
        }
    }
}
