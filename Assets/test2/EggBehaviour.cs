using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggBehaviour : MonoBehaviour {
    
    [System.NonSerialized]
    public Camera _camera;
    [System.NonSerialized]
    public bool _isTaken = false;

    Animator _animator;
    Rigidbody _rigidbody;

    [SerializeField]
    Vector3 _tuneParams;

    [SerializeField]
    bool _isMovable = true;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update ()
    {
        if (_isMovable)
        {
            //var angle = 1f;
            //transform.Rotate(Vector3.up, angle, Space.Self);
            Move();
        }
    }
    
    void Move()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            Debug.Log("eggForward : " + transform.forward);

            transform.Translate(Vector3.forward, Space.Self);
        }
    }

    public bool IsInCamera
    {
        get
        {
            var M_V = _camera.worldToCameraMatrix;
            var M_P = _camera.projectionMatrix;
            var M_VP = M_P * M_V;

            var pos = transform.position;
            var p = M_VP * new Vector4(pos.x, pos.y, pos.z, 1.0f);

            if (p.w == 0) return true;

            var x = p.x / p.w;
            var y = p.y / p.w;
            var z = p.z / p.w;

            if (x < -_tuneParams.x) return false;
            if (x > _tuneParams.x) return false;
            if (y < -_tuneParams.y) return false;
            if (y > _tuneParams.y) return false;
            if (z < -_tuneParams.z) return false;
            if (z > _tuneParams.z) return false;

            return true;
        }
    }
    
    void CheckTrigger(string name) { _animator.SetTrigger(name); }
}
