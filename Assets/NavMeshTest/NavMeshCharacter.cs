using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshCharacter : MonoBehaviour {
    protected enum eMoveState
    {
        inIntarval,
        isMoving,
    }

    protected enum eCharaState
    {
        isWaiting,
        isWalking,
        isRunning,
        inSpecialMotion,
    }

    [SerializeField]
    private NavMeshAgent _Agent;

    [SerializeField]
    private Rigidbody _Rigidbody;

    [SerializeField]
    private float _CharaHight;

    [System.Serializable]
    public class AnimationStrings
    {
        public string Idle;
        public string Jump;
        public string Walk;
        public string Run;
    }

    public class AnimationIDs
    {
        public int Idle;
        public int Jump;
        public int Walk;
        public int Run;

        public void Init(AnimationStrings AnimStrings)
        {
            Idle = Animator.StringToHash(AnimStrings.Idle);
            Jump = Animator.StringToHash(AnimStrings.Jump);
            Walk = Animator.StringToHash(AnimStrings.Walk);
            Run = Animator.StringToHash(AnimStrings.Run);
        }
    }

    public static AnimationIDs AnimIDs { get; private set; }

    public static void Init(AnimationStrings AnimStrings)
    {
        AnimIDs = new AnimationIDs();
        AnimIDs.Init(AnimStrings);
    }

    [Space(10)]

    [Header("AI移動を許可するか")]
    [Space(5)]
    [Header("キャラクターの性質")]
    [SerializeField]
    private bool _AutoMove = true;

    [Header("走行を許可するか")]
    [SerializeField]
    private bool _EnableRun = true;

    [Header("キャラ正面の視界範囲(次の移動先範囲に影響)")]
    [SerializeField]
    private Vector3 _FieldOfVisionScale;

    [Header("歩行・走行 速度")]
    [SerializeField]
    private float _WalkSpeed;

    [SerializeField]
    private float _RunSpeed;

    [Header("走行をする可能性(%)")]
    [SerializeField, Range(0.0f, 100.0f)]
    private float _RunPossibility;

    [Header("最小・最大 移動間_待機時間(秒)")]
    [SerializeField]
    private float _MinMoveInterval;

    [SerializeField]
    private float _MaxMoveInterval;

    [Header("待機時に特殊モーションをする可能性(%)")]
    [SerializeField, Range(0.0f, 100.0f)]
    private float _SpecialMotionPossibility;

    [Header("特殊モーションが次に可能になるまでの待機回数")]
    [SerializeField]
    private int _SpecialMotionInterval;

    [SerializeField]
    private eMoveState _MoveState = eMoveState.inIntarval;

    [SerializeField]
    private eCharaState _CharaState = eCharaState.isWaiting;

    private void Awake()
    {
        _Agent = GetComponent<NavMeshAgent>();
        _NavMeshBuilder = GetComponent<LocalNavMeshBuilder>();
        _Animator = GetComponent<Animator>();
    }

    private IEnumerator _AIRoutine;

    private Vector3 _LastHitNormal;
    private Vector3 _MoveTargetPosition;

    private LocalNavMeshBuilder _NavMeshBuilder;
    private Animator _Animator;

    private NavMeshTargetPoint _MoveTargetPoint;
    private NavMeshTargetPoint _LastMoveTargetPoint;

    private void Start()
    {
        //今はここで初期化
        CharaFieldOfVision.Create(transform, this, _FieldOfVisionScale);
        {
            var w = _FieldOfVisionScale.x < _FieldOfVisionScale.z ? _FieldOfVisionScale.z : _FieldOfVisionScale.x;
            _NavMeshBuilder.m_Size = new Vector3(w, _NavMeshBuilder.m_Size.y, w);
        }
        if (_MinMoveInterval > _MaxMoveInterval) _MaxMoveInterval = _MinMoveInterval;

        StartCoroutine(Routine_Main());
        StartCoroutine(Routine_OnGround());
    }

    public void Play() {
        StartCoroutine(Routine_Main());
        StartCoroutine(Routine_OnGround());
        GetComponent<Collider>().enabled = true;
    }

    public void Stop() {
        StopAllCoroutines();
        _Agent.isStopped = true;
        GetComponent<Collider>().enabled = false;
    }

    private IEnumerator Routine_Main()
    {
        while (!CalcNextPoint())
        {
            yield return new WaitForSeconds(0.5f);
        }

        while (true)
        {
            if (!_AutoMove)
            {
                _MoveState = eMoveState.inIntarval;
            }

            if (_MoveState == eMoveState.inIntarval)
            {
                yield return StartCoroutine(Routine_Intarval());
            }
            else if (_MoveState == eMoveState.isMoving)
            {
                yield return StartCoroutine(Routine_Move());
            }
            else
            {
                yield return null;
            }
        }
    }

    private IEnumerator Routine_Intarval()
    {
        _Agent.isStopped = true;

        if (Random.Range(0.0f, 100.0f) <= _SpecialMotionPossibility)
        {
            //特殊モーション
            _CharaState = eCharaState.inSpecialMotion;
        }
        else
        {
            //待機中
            _CharaState = eCharaState.isWaiting;
            _Animator.SetTrigger(AnimIDs.Idle);

            float ElapsedSeconds = Random.Range(_MinMoveInterval, _MaxMoveInterval);
            while (ElapsedSeconds > 0.0f)
            {
                ElapsedSeconds -= Time.deltaTime;
                yield return null;
            }
        }

        _MoveState = eMoveState.isMoving;
    }

    private IEnumerator Routine_Move()
    {
        _Agent.isStopped = false;

        if (Random.Range(0.0f, 100.0f) <= _RunPossibility)
        {
            //走り
            _CharaState = eCharaState.isRunning;
            _Animator.SetTrigger(AnimIDs.Run);
            _Agent.speed = _RunSpeed;
        }
        else
        {
            //歩き
            _CharaState = eCharaState.isWalking;
            _Animator.SetTrigger(AnimIDs.Walk);
            _Agent.speed = _WalkSpeed;
        }

        //次の地点が見つかるまで待機
        while (_MoveTargetPoint == null && !CalcNextPoint())
        {
            yield return new WaitForSeconds(0.5f);
            Debug.Log("Serching");
        }

        while (true)
        {
            _Agent.Move(transform.forward * _Agent.speed * 0.75f * Time.deltaTime);

            //到着まで移動
            if (Vector3.SqrMagnitude(Vector3.Scale(_MoveTargetPosition, new Vector3(1, 0, 1)) - Vector3.Scale(transform.position, new Vector3(1, 0, 1))) < 0.1f)
            {
                _LastMoveTargetPoint = _MoveTargetPoint;
                _MoveTargetPoint = null;
                break;
            }
            yield return null;
        }

        _MoveState = eMoveState.inIntarval;
    }

    private IEnumerator Routine_OnGround()
    {
        while (true)
        {
            //床に対してray
            Ray ray = new Ray(transform.position + Vector3.up * _CharaHight, Vector3.down);
            RaycastHit hit;
            NavMeshHit navhit;
            if (Physics.Raycast(ray, out hit, _CharaHight * 1.5f, 1 << 12))
            {
                if (_LastHitNormal != hit.normal)
                {
                    if (NavMesh.SamplePosition(_Agent.transform.localPosition, out navhit, _CharaHight * 1.5f, NavMesh.AllAreas))
                    {
                        _Agent.baseOffset = hit.point.y - navhit.position.y;
                        _LastHitNormal = hit.normal;
                        CalcNextPoint();
                    }
                }
            }
            yield return null;
        }
    }

    private bool CalcNextPoint()
    {
        Ray ray = new Ray(
            transform.position + 
            new Vector3(
                (Random.Range(0, 2) == 0 ? Random.Range(0.2f, 0.4f) : Random.Range(-0.4f, -0.2f)) * _NavMeshBuilder.m_Size.x,
                1.0f,
                (Random.Range(0, 2) == 0 ? Random.Range(0.2f, 0.4f) : Random.Range(-0.4f, -0.2f)) * _NavMeshBuilder.m_Size.z
                ),
            Vector3.down
            );
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, 1 << 12))
        {
            _MoveTargetPosition = hit.point;
            _Agent.isStopped = false;
            _Agent.SetDestination(_MoveTargetPosition);

            //デバッグ用
            //var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //obj.transform.position = _MoveTargetPosition;
            //obj.transform.localScale = new Vector3(0.1f, 1.0f, 0.1f);
            return true;
        }
        return false;
    }

    public void SetTargetPoint(NavMeshTargetPoint MoveTargetPoint)
    {
        if (_LastMoveTargetPoint == null || (_LastMoveTargetPoint != MoveTargetPoint && _MoveTargetPoint == null))
        {
            _MoveTargetPoint = MoveTargetPoint;
            _MoveTargetPosition = MoveTargetPoint.transform.position;
            _Agent.SetDestination(MoveTargetPoint.transform.position);
        }
    }
}
