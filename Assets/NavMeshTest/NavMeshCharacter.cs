using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshCharacter : MonoBehaviour {
    protected enum eAnimationState
    {
        isFalling,
        isIdle,
        isWalk,
        isJump,
        isAttack
    }

    protected enum eAttackType
    {
        OneShot,
    }

    [SerializeField]
    private Animation _Animation;

    [SerializeField]
    private UnityEngine.AI.NavMeshAgent _Agent;

    [SerializeField]
    private Rigidbody _Rigidbody;

    private eAnimationState _State = eAnimationState.isWalk;

    private void Awake()
    {
        _MoveSpeed = _Agent.speed;
    }

    private IEnumerator _AIRoutine;

    private float _MoveSpeed;
    private IEnumerator Routine_Main()
    {
        while (true)
        {
            _Agent.speed = _MoveSpeed;

            _Agent.SetDestination(transform.position + transform.forward);
            //Vector3 TargetPos = _Target.transform.position;
            //TargetPos.y = 0.0f;
            //if (Vector3.SqrMagnitude(TargetPos - new Vector3(transform.position.x, 0.0f, transform.position.z)) < _Attack_FieldOfVision * _Attack_FieldOfVision)
            //{
            //    StateChange(eAnimationState.isIdle, 0.1f);
            //}
            //if (_State == eAnimationState.isIdle)
            //{
            //    _Agent.speed = 0.0f;
            //    Vector3 TargetPos = _Target.transform.position;
            //    TargetPos.y = 0.0f;
            //    if (Vector3.SqrMagnitude(TargetPos - new Vector3(transform.position.x, 0.0f, transform.position.z)) >= _Attack_FieldOfVision * _Attack_FieldOfVision)
            //    {
            //        StateChange(eAnimationState.isWalk, 0.1f);
            //    }
            //    else if (_CurrentAttackDelay <= 0.0f)
            //    {
            //        StateChange(eAnimationState.isAttack, 0.1f);
            //    }
            //}
            //else if (_State == eAnimationState.isWalk)
            //{
            //    _Agent.speed = _MoveSpeed;

            //    //if (NavMesh.SamplePosition(_Agent.transform.localPosition, out navHit, 0.1f, NavMesh.AllAreas))
            //    //{
            //    _Agent.SetDestination(_Target.transform.position);
            //    Vector3 TargetPos = _Target.transform.position;
            //    TargetPos.y = 0.0f;
            //    if (Vector3.SqrMagnitude(TargetPos - new Vector3(transform.position.x, 0.0f, transform.position.z)) < _Attack_FieldOfVision * _Attack_FieldOfVision)
            //    {
            //        StateChange(eAnimationState.isIdle, 0.1f);
            //    }
            //    //}
            //    //else
            //    //{
            //    //    _Rigidbody.isKinematic = false;
            //    //    _Agent.updatePosition = false;
            //    //    StateChange(eAnimationState.isFalling, 0.1f);
            //    //    while (inStateChange) yield return null;
            //    //}
            //}
            //else if (_State == eAnimationState.isAttack)
            //{
            //    if (_CurrentAttackDelay <= 0.0f)
            //    {
            //        yield return StartCoroutine(_WeaponInstance.Routine_Weapon(_Target, _Damage, _Enemy));
            //        StartCoroutine(Routine_AttackSpan());

            //        StateChange(eAnimationState.isIdle, 0.1f);
            //        while (inStateChange) yield return null;
            //    }
            //}
            yield return new WaitForFixedUpdate();
        }
    }

    private bool inStateChange = false;
    protected virtual void StateChange(eAnimationState State, float FixedTime)
    {
        if (!inStateChange)
        {
            StartCoroutine(Routine_StateChange(State, FixedTime));
        }
    }

    protected IEnumerator Routine_StateChange(eAnimationState State, float FixedTime)
    {
        string StateName = "";
        if (State == eAnimationState.isIdle) StateName = "Idle";
        else if (State == eAnimationState.isAttack) StateName = "Idle";
        else if (State == eAnimationState.isWalk) StateName = "Walk";
        else if (State == eAnimationState.isJump) StateName = "Walk";

        if (StateName == "")
        {
            _Animation.Stop();
        }
        _State = State;
        inStateChange = true;
        _Animation.CrossFade(StateName, FixedTime, 0);
        yield return new WaitForSeconds(FixedTime);
        inStateChange = false;

    }
}
