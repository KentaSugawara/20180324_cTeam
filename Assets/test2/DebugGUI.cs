using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGUI : MonoBehaviour
{
    [SerializeField]
    Kudan.AR.KudanTracker m_kudanTracker = null;
    [SerializeField]
    GameObject m_markerlessObj = null;
    [SerializeField]
    EggSpawner m_eggSpawner = null;
    [SerializeField]
    EggSearcher m_eggSearcher = null;

    // onGUI()で呼び出す
    public void DisplayGUI(float debugGUIScale)
    {
        var eggList = m_eggSpawner.EggList;
        var maxNum = m_eggSpawner.MaxNum;

        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(debugGUIScale, debugGUIScale, 1f));
        GUILayout.BeginVertical("box");

        GUILayout.Label("Status: " + (m_eggSearcher.IsSearching ? "Searching" : "Not Searching"));

        GUILayout.Label("cross: " + Vector3.Cross(Camera.main.transform.forward, m_markerlessObj.transform.position));
        GUILayout.Label("CameraDirection: " + Camera.main.transform.forward);
        GUILayout.Label("MarkerlessVec: " + m_markerlessObj.transform.position.normalized);
        
        GUILayout.Label("MarkerlessObjPosition: " + m_markerlessObj.transform.position);
        GUILayout.Label("MarkerlessObjRotation: " + m_markerlessObj.transform.rotation.eulerAngles);
        GUILayout.Label("EggNum: " + eggList.Count + "/" + maxNum);

        if (eggList.Count > 0)
            foreach (var obj in eggList)
            {
                GUILayout.Label("EggLocalPos: " + obj.transform.localPosition);
            }
    }
}
