using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class EggBehaviour : MonoBehaviour
{
    [SerializeField]
    Vector3 _tuneParams;

    [SerializeField]
    float _speed = 1f;

    [SerializeField]
    bool _isMovable = false;

    GameObject _item;

    [System.NonSerialized]
    public Camera _camera;
    [System.NonSerialized]
    public bool _isTaken = false;

    Animator _animator;
    Rigidbody _rigidbody;

    enum EggState
    {
        Idle, Walk, Run, Jump, Play,
    }
    //Status _status = Status.Play;
    //string[] stateNames = {
    //    Status.Idle.ToString(),
    //    Status.Walk.ToString(),
    //    Status.Run.ToString(),
    //    Status.Jump.ToString(),
    //    Status.Play.ToString(),
    //};

    [Space, SerializeField]
    float stateChangeTime = 3;
    float elapsedTime = 10;

    //
    // methods
    //
    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        if (_isMovable)
        {
            // 行動開始
            StartCoroutine(Move());
        }
    }

    IEnumerator Move()
    {
        while (true)
        {
            elapsedTime += Time.deltaTime;

            MoveInAnimation();

            yield return null;
        }
    }

    // アニメーションに合わせた移動
    void MoveInAnimation()
    {
        var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        // Play
        if (stateInfo.IsName(EggState.Play.ToString()) && targetItem)
        {
            elapsedTime = 0;
        }
        else
        {
            // Idle
            if (stateInfo.IsName(EggState.Idle.ToString()))
            {
            }
            // Jump
            else if (stateInfo.IsName(EggState.Jump.ToString()))
            {
                transform.Rotate(Vector3.up);
            }
            else
            {
                // 前方に床があるか判定
                if (CheckForward())
                {
                    // Walk
                    if (stateInfo.IsName(EggState.Walk.ToString()))
                    {
                        transform.Translate(Vector3.forward * _speed, Space.Self);
                    }
                    // Run
                    else if (stateInfo.IsName(EggState.Run.ToString()))
                    {
                        transform.Translate(Vector3.forward * _speed * 2f, Space.Self);
                    }
                }
                else
                {
                    ChangeState(EggState.Jump, true);
                }
            }

            var randomState = (EggState)Random.Range(0, 4);
            ChangeState(randomState);

            // アイテムの方向を向く
            if (_item && elapsedTime > 10)
            {
                var rot1 = transform.rotation;

                transform.LookAt(_item.GetComponent<ItemInfo>()._targetTransform.position, transform.up);
                var rot2 = transform.rotation;

                transform.rotation = Quaternion.Lerp(rot1, rot2, Time.deltaTime);
            }
        }

    }

    bool CheckForward()
    {
        List<DetectedPlane> planeList = new List<DetectedPlane>();
        Session.GetTrackables<DetectedPlane>(planeList);

        RaycastHit hit;
        Vector3 originPos = transform.position + Vector3.up * 0.2f;
        Vector3 vec = transform.TransformDirection(new Vector3(0, -1, 1));

        if (Physics.Raycast(originPos, vec, out hit))
        {
            Debug.Log(hit);
            Debug.DrawRay(originPos, vec, Color.yellow);
            return true;
        }

        Debug.DrawRay(originPos, vec, Color.red);
        return false;
    }

    // 状態を変更
    void ChangeState(EggState state, bool constrain = false)
    {
        if (constrain)
        {
            elapsedTime = stateChangeTime;
            stateChangeTime += 1;
        }
        else
        {
            if (elapsedTime > stateChangeTime)
                stateChangeTime += Random.Range(3f, 4f);
            else
                return;
        }
        _animator.SetTrigger(state.ToString());
    }

    // 子の OnTrigerEnter で呼び出す
    public void OnTriggerEnterOnChild(Collider other)
    {
        if (other.tag == "Item")
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Play"))
            {
                transform.position = other.transform.position;
                transform.rotation = other.transform.rotation;
                _animator.SetTrigger("Play");
                elapsedTime = 0;
                stateChangeTime = 0;
            }
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

    public GameObject targetItem
    {
        get { return _item; }

        set
        {
            _item = value;
            elapsedTime += 10;
        }
    }

    void CheckTrigger(string name) { _animator.SetTrigger(name); }
}
