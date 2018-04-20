using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggMove : MonoBehaviour {

    bool m_isRendered = false;
    bool m_isTaken = false;

    Animator m_animator;
    Rigidbody m_rigidbody;

    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    void Update ()
    {
        var angle = 1f;
        //transform.Rotate(Vector3.up, angle, Space.Self);
        if (m_animator.GetBool("isWalking"))
        {
            Debug.Log("eggForward : " + transform.forward);
            
            transform.Translate(Vector3.forward, Space.Self);
        }
    }

    void SetTrueBoolean(string name) { m_animator.SetBool(name, true); }
    void CheckTrigger(string name) { m_animator.SetTrigger(name); }

    // カメラに入ったらフラグを立てる
    void OnBecameVisible()
    {
        m_isRendered = true;
    }

    // カメラから出たらフラグを折る
    void OnBecameInvisible()
    {
        // check
        //if (m_isRendered) Disappear();
        m_isRendered = false;

        // 写真を撮られていたら消す
        if (m_isTaken) Disappear();
    }

    private void OnDisable()
    {
        Disappear();
    }

    void Disappear()
    {
        Destroy(gameObject);
    }
}
