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

    public enum eCharaState
    {
        isWaiting,
        isWalking,
        isRunning,
        inUnique,
        inHappy,
        inAngry,
        inSad,
        inFunny,
        isItemPlaying
    }

    [SerializeField]
    private Renderer _BodyRenderer;

    public Renderer BodyRenderer
    {
        get { return _BodyRenderer; }
    }

    private NavMeshAgent _Agent;

    [SerializeField]
    private float _CharaHight;

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

    [Header("移動から待機に移行する可能性(%)")]
    [SerializeField]
    private float _IntervalPossibility = 100.0f;

    [Space(5)]
    [Header("待機モーションをする割合")]
    [SerializeField, Range(0, 100)]
    private int _Rate_Idle;

    [Header("特殊モーションをする割合")]
    [SerializeField, Range(0, 100)]
    private int _Rate_Unique;

    [Header("喜モーションをする割合")]
    [SerializeField, Range(0, 100)]
    private int _Rate_Happy;

    [Header("怒モーションをする割合")]
    [SerializeField, Range(0, 100)]
    private int _Rate_Angry;

    [Header("哀モーションをする割合)")]
    [SerializeField, Range(0, 100)]
    private int _Rate_Sad;

    [Header("楽モーションをする割合")]
    [SerializeField, Range(0, 100)]
    private int _Rate_Funny;

    [SerializeField]
    private eMoveState _MoveState = eMoveState.inIntarval;

    [SerializeField]
    private eCharaState _CharaState = eCharaState.isWaiting;
    public eCharaState CharaState
    {
        get { return _CharaState; }
    }

    [Space(5)]
    [Header("鳴き声")]
    [SerializeField]
    private CharacterAudio _CharaAudio;

    private void Awake()
    {
        _Agent = GetComponent<NavMeshAgent>();
        _NavMeshBuilder = GetComponent<LocalNavMeshBuilder>();
        _Animator = GetComponent<Animator>();
    }

    private EggSpawnerARCore _EggSpawner;

    private void OnDisable()
    {
        if (gameObject != null) _EggSpawner.RemoveEgg(gameObject);
        if (transform.parent != null) Destroy(transform.parent.gameObject);
    }

    private IEnumerator _AIRoutine;

    private Vector3 _LastHitNormal;
    private Vector3 _MoveTargetPosition;

    private LocalNavMeshBuilder _NavMeshBuilder;
    private Animator _Animator;

    private NavMeshTargetPoint _MoveTargetPoint;
    private NavMeshTargetPoint _LastMoveTargetPoint;
    private CharaFieldOfVision _CharaFieldOfVision;

    private static int _ID_Idle = Animator.StringToHash("Idle");
    private static int _ID_Jump = Animator.StringToHash("Jump");
    private static int _ID_Walk = Animator.StringToHash("Walk");
    private static int _ID_Run = Animator.StringToHash("Run");
    private static int _ID_Unique = Animator.StringToHash("Unique");
    private static int _ID_Happy = Animator.StringToHash("Happy");
    private static int _ID_Angry = Animator.StringToHash("Angry");
    private static int _ID_Sad = Animator.StringToHash("Sad");
    private static int _ID_Funny = Animator.StringToHash("Funny");

    private int _ID_UniqueState = Animator.StringToHash("Base Layer.Unique");
    private int _ID_HappyState = Animator.StringToHash("Base Layer.Happy");
    private int _ID_AngryState = Animator.StringToHash("Base Layer.Angry");
    private int _ID_SadState = Animator.StringToHash("Base Layer.Sad");
    private int _ID_FunnyState = Animator.StringToHash("Base Layer.Funny");

    private void Start()
    {
		//今はここで初期化
		_CharaFieldOfVision = CharaFieldOfVision.Create(transform, this, _FieldOfVisionScale);
        {
            var w = _FieldOfVisionScale.x < _FieldOfVisionScale.z ? _FieldOfVisionScale.z : _FieldOfVisionScale.x;
            _NavMeshBuilder.m_Size = new Vector3(w, _NavMeshBuilder.m_Size.y, w);
        }
        if (_MinMoveInterval > _MaxMoveInterval) _MaxMoveInterval = _MinMoveInterval;

        StartCoroutine(Routine_Main());
        StartCoroutine(Routine_OnGround());
    }

    public void Init(EggSpawnerARCore Spawner)
    {
        _EggSpawner = Spawner;
        Debug.Log(_EggSpawner);
    }

    public void Play() {
        _PlayingItemIndex = null;
        _Agent.enabled = true;
        _CharaFieldOfVision.Play();
        StartCoroutine(Routine_Main());
        StartCoroutine(Routine_OnGround());
        GetComponent<Collider>().enabled = true;
    }

    public void Stop() {
        _PlayingItemIndex = null;
        _CharaFieldOfVision.Stop();
        StopAllCoroutines();
        _Agent.isStopped = true;
        _Agent.enabled = false;
        GetComponent<Collider>().enabled = false;
    }

    private int? _PlayingItemIndex = -1;
    public int? PlayingItemIndex
    {
        get { return _PlayingItemIndex; }
    }

    public void StartItemPlaying(int ItemIndex)
    {
        _PlayingItemIndex = ItemIndex;
        _CharaState = eCharaState.isItemPlaying;
        Stop();
    }

    public void EndItemPlaying()
    {
        _PlayingItemIndex = null;
        _CharaState = eCharaState.isWaiting;
        Play();
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

        //ランダムに次のモーションを設定
        {
            List<eCharaState> table = new List<eCharaState>();
            for (int i = 0; i < _Rate_Idle; ++i) table.Add(eCharaState.isWaiting);
            for (int i = 0; i < _Rate_Unique; ++i) table.Add(eCharaState.inUnique);
            for (int i = 0; i < _Rate_Happy; ++i) table.Add(eCharaState.inHappy);
            for (int i = 0; i < _Rate_Angry; ++i) table.Add(eCharaState.inAngry);
            for (int i = 0; i < _Rate_Sad; ++i) table.Add(eCharaState.inSad);
            for (int i = 0; i < _Rate_Funny; ++i) table.Add(eCharaState.inFunny);

            if (table.Count > 0) _CharaState = table[Random.Range(0, table.Count)];
            else _CharaState = eCharaState.isWaiting;
        }

        if (_CharaState == eCharaState.isWaiting)
        {
            //待機中
            _Animator.SetTrigger(_ID_Idle);

            float ElapsedSeconds = Random.Range(_MinMoveInterval, _MaxMoveInterval);
            while (ElapsedSeconds > 0.0f)
            {
                ElapsedSeconds -= Time.deltaTime;
                yield return null;
            }
        }
        else if (_CharaState == eCharaState.inUnique)
        {
            //特殊モーション
            _Animator.SetTrigger(_ID_Unique);

            //変わるまで待機
            while (_Animator.GetCurrentAnimatorStateInfo(0).fullPathHash != _ID_UniqueState) yield return null;

			if (_CharaAudio)
				_CharaAudio.Play(CharacterAudio.eAudioType.Unique);

            //終わるまで待機
            while (_Animator.GetCurrentAnimatorStateInfo(0).fullPathHash == _ID_UniqueState) yield return null;
        }
        else if (_CharaState == eCharaState.inHappy)
        {
            //喜モーション
            _Animator.SetTrigger(_ID_Happy);

            //変わるまで待機
            while (_Animator.GetCurrentAnimatorStateInfo(0).fullPathHash != _ID_HappyState) yield return null;

			if(_CharaAudio)
				_CharaAudio.Play(CharacterAudio.eAudioType.Happy);

            //終わるまで待機
            while (_Animator.GetCurrentAnimatorStateInfo(0).fullPathHash == _ID_HappyState) yield return null;
        }
        else if (_CharaState == eCharaState.inAngry)
        {
            //怒モーション
            _Animator.SetTrigger(_ID_Angry);

            //変わるまで待機
            while (_Animator.GetCurrentAnimatorStateInfo(0).fullPathHash != _ID_AngryState) yield return null;

			if (_CharaAudio)
				_CharaAudio.Play(CharacterAudio.eAudioType.Angry);

            //終わるまで待機
            while (_Animator.GetCurrentAnimatorStateInfo(0).fullPathHash == _ID_AngryState) yield return null;
        }
        else if (_CharaState == eCharaState.inSad)
        {
            //哀モーション
            _Animator.SetTrigger(_ID_Sad);

            //変わるまで待機
            while (_Animator.GetCurrentAnimatorStateInfo(0).fullPathHash != _ID_SadState) yield return null;

			if (_CharaAudio)
				_CharaAudio.Play(CharacterAudio.eAudioType.Sad);

            //終わるまで待機
            while (_Animator.GetCurrentAnimatorStateInfo(0).fullPathHash == _ID_SadState) yield return null;
        }
        else if (_CharaState == eCharaState.inHappy)
        {
            //楽モーション
            _Animator.SetTrigger(_ID_Funny);

            //変わるまで待機
            while (_Animator.GetCurrentAnimatorStateInfo(0).fullPathHash != _ID_FunnyState) yield return null;

			if (_CharaAudio)
				_CharaAudio.Play(CharacterAudio.eAudioType.Happy);

            //終わるまで待機
            while (_Animator.GetCurrentAnimatorStateInfo(0).fullPathHash == _ID_FunnyState) yield return null;
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
            _Animator.SetTrigger(_ID_Run);
            _Agent.speed = _RunSpeed;
        }
        else
        {
            //歩き
            _CharaState = eCharaState.isWalking;
            _Animator.SetTrigger(_ID_Walk);
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
            if (_MoveTargetPoint != null && _MoveTargetPoint.gameObject.activeInHierarchy)
            {
                if (Vector3.SqrMagnitude(Vector3.Scale(_MoveTargetPosition, new Vector3(1, 0, 1)) - Vector3.Scale(transform.position, new Vector3(1, 0, 1))) < 0.01f)
                {
                    _LastMoveTargetPoint = _MoveTargetPoint;
                    _MoveTargetPoint = null;
                    break;
                }
            }
            else
            {
                if (Vector3.SqrMagnitude(Vector3.Scale(_MoveTargetPosition, new Vector3(1, 0, 1)) - Vector3.Scale(transform.position, new Vector3(1, 0, 1))) < 0.1f)
                {
                    _LastMoveTargetPoint = _MoveTargetPoint;
                    _MoveTargetPoint = null;
                    break;
                }
            }


            yield return null;
        }

        if (Random.Range(0.0f, 100.0f) <= _IntervalPossibility)
            _MoveState = eMoveState.inIntarval;
        else
            _MoveState = eMoveState.isMoving;
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

        //NavMeshHit nvhit;
        //if (NavMesh.Raycast(ray.origin, ray.origin + Vector3.down * 100.0f, out nvhit, NavMesh.AllAreas) == false) return false;

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
