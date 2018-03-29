using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchUI : MonoBehaviour
{
    [SerializeField]
    GameObject m_titleUI = null;
    GameObject m_activeUI;

    void Start()
    {
        m_titleUI.SetActive(true);
    }

    public void SelectStart(GameObject obj)
    {
        m_titleUI.SetActive(false);
        obj.SetActive(true);
        m_activeUI = obj;
    }

    public void SelectBack()
    {
        m_activeUI.SetActive(false);
        m_titleUI.SetActive(true);
    }
}
