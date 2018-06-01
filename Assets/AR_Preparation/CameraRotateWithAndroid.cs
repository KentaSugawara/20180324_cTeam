using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraRotateWithAndroid : MonoBehaviour {

    [SerializeField]
    private Text _Text;

	// Use this for initialization
	void Start () {
        Input.gyro.enabled = true;
    }
	
	// Update is called once per frame
	void Update () {
        var g = Input.gyro.attitude;
        g.x *= -1.0f;
        g.y *= -1.0f;
        transform.localRotation = g;
        _Text.text = transform.rotation.ToString();
    }
}
