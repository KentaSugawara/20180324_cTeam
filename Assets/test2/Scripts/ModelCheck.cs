using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelCheck : MonoBehaviour {

    [SerializeField]
    GameObject _KudanCameraObj;

    private void OnEnable()
    {
        _KudanCameraObj.SetActive(false);
    }
}
