using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGUI : MonoBehaviour
{
    [SerializeField]
    Kudan.AR.KudanTracker _kudanTracker = null;
    [SerializeField]
    Camera _kudanCamera = null;
    [SerializeField]
    GameObject _markerlessObj = null;
    [SerializeField]
    EggSpawner _eggSpawner = null;
    [SerializeField]
    EggSearcher _eggSearcher = null;


    // onGUI()で呼び出す
    public void DisplayGUIKudan(float debugGUIScale)
    {
        var eggList = _eggSpawner.EggList;
        var maxNum = _eggSpawner.MaxNum;

        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(debugGUIScale, debugGUIScale, 1f));
        GUILayout.BeginVertical("box");

        GUILayout.Label("Status: " + (_eggSearcher.IsSearching ? "Searching" : "Not Searching"));

        GUILayout.Label("cross: " + Vector3.Cross(_kudanCamera.transform.forward, _markerlessObj.transform.position));
        GUILayout.Label("CameraDirection: " + _kudanCamera.transform.forward);
        GUILayout.Label("MarkerlessVec: " + _markerlessObj.transform.position.normalized);

        GUILayout.Label("MarkerlessObjPosition: " + _markerlessObj.transform.position);
        GUILayout.Label("MarkerlessObjRotation: " + _markerlessObj.transform.rotation.eulerAngles);
        GUILayout.Label("EggNum: " + eggList.Count + "/" + maxNum);

        if (eggList.Count > 0)
            foreach (var obj in eggList)
            {
                GUILayout.Label("EggLocalPos: " + obj.transform.localPosition);

                if (obj.GetComponent<EggBehaviour>().targetItem)
                {
                    //GUILayout.Label("targetItemWorldPos: " + obj.GetComponent<EggBehaviour>().targetItem.transform.position);
                    //GUILayout.Label("targetItemLocalPos: " + obj.GetComponent<EggBehaviour>().targetItem.transform.localPosition);
                }
            }
    }

    public void DisplayGUIARCore()
    {

    }
}
