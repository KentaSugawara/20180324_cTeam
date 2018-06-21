using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kudan.AR;

public class IsInCamera : MonoBehaviour
{

    [SerializeField]
    KudanTracker _kudanTracker;

    EggBehaviour _parentEggMove;

    Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
        _parentEggMove = transform.parent.GetComponent<EggBehaviour>();
    }
    
}
