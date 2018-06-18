using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBaseBehaviour : MonoBehaviour {

    protected Dictionary<string, string> triggerName = new Dictionary<string, string>(){
        { "SeeSawA", "Play_SeeSaw_EggA"},
        { "SeeSawB", "Play_SeeSaw_EggB"},
    };

    [SerializeField]
    protected Collider[] _childrenColliders;

    protected List<GameObject> _eggObjects = new List<GameObject>();

    protected Animator _animator;

    protected int _playingEggNum = 0;

    protected virtual void Awake() {
        _animator = GetComponent<Animator>();
    }

    void Start() {

    }

    void Update() {

    }

    /// <summary>
    /// たまごのオブジェクトをリストに追加
    /// </summary>
    /// <param name="other"></param>
    protected virtual void OnTriggerEnter(Collider other) {
        if (other.tag == "Egg") {
            //Debug.LogError(other.tag);
            //other.GetComponent<Animator>().SetBool("Play", true);
            _eggObjects.Add(other.gameObject);
            other.GetComponent<Animator>().SetBool("Play", true);
            other.GetComponent<NavMeshCharacter>().Stop();

            other.transform.position = transform.position;
            other.transform.localRotation = transform.localRotation;
        }
    }

    public void ResetPlayingEggNum() { _playingEggNum = 0; }
    public void PlayAnimation() { _animator.SetTrigger("Play"); }

    public virtual void ResetColliders() { }
}
