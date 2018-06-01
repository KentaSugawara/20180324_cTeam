using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggBehaviour : MonoBehaviour
{

    [SerializeField]
    Vector3 _tuneParams;
    [SerializeField]
    float _speed = 1f;
    [SerializeField]
    bool _isMovable = false;
    [Space, SerializeField]
    GameObject _item;

    [System.NonSerialized]
    public Camera _camera;
    [System.NonSerialized]
    public bool _isTaken = false;

    Animator _animator;
    Rigidbody _rigidbody;

    enum Status
    {
        Idle, Walk, Run, Jump, Play,
    }
    //Status _status = Status.Play;
    string[] stateNames = {
        Status.Idle.ToString(),
        Status.Walk.ToString(),
        Status.Run.ToString(),
        Status.Jump.ToString(),
        Status.Play.ToString(),
    };

    [Space, SerializeField]
    float stateChangeTime = 3;
    float elapsedTime = 10;

    //
    // method
    //
    void Start()
    {
        _animator = GetComponent<Animator>();
        if (_isMovable)
        {
            //var angle = 1f;
            //transform.Rotate(Vector3.up, angle, Space.Self);

            StartCoroutine(Move());
        }
    }

    IEnumerator Move()
    {
        while (_isMovable)
        {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            elapsedTime += Time.deltaTime;

            Debug.Log(elapsedTime);

            if (stateInfo.IsName("Play"))
            {
                elapsedTime = 0;
            }
            else
            {
                if (stateInfo.IsName("Idle"))
                {
                    if (stateInfo.normalizedTime >= 2)
                        _animator.SetTrigger("Walk");
                }
                else if (stateInfo.IsName("Walk"))
                {
                    transform.Translate(Vector3.forward * _speed, Space.Self);

                    if (stateInfo.normalizedTime >= 5)
                        _animator.SetTrigger("Run");
                }
                else if (stateInfo.IsName("Run"))
                {
                    transform.Translate(Vector3.forward * _speed * 2f, Space.Self);

                    if (stateInfo.normalizedTime >= 3)
                        _animator.SetTrigger("Jump");
                }
                else if (stateInfo.IsName("Jump"))
                {
                    transform.Rotate(new Vector3(0, 1, 0));

                    if (stateInfo.normalizedTime >= 3)
                        _animator.SetTrigger("Idle");
                }

                //if(elapsedTime > stateChangeTime)
                //{
                //    _animator.SetTrigger(stateNames[Random.Range(0, 5)]);
                //    stateChangeTime += 3;
                //}

                if (_item && elapsedTime > 10)
                {
                    var t = transform;
                    var rot1 = t.rotation;
                    t.LookAt(_item.transform);
                    var rot2 = t.rotation;
                    t.rotation = Quaternion.Lerp(rot1, rot2, Time.deltaTime);
                }
                
            }

            yield return null;
        }
        yield break;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            transform.position = other.transform.position;
            transform.rotation = other.transform.rotation;
            _animator.SetTrigger("Play");
            elapsedTime = 0;
        }
    }

    //
    // property
    //
    public bool isInCamera
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

    public GameObject targetItem {
        get { return _item; }

        set {
            _item = value;
            elapsedTime += 10;
        }
    }

    void CheckTrigger(string name) { _animator.SetTrigger(name); }
}
