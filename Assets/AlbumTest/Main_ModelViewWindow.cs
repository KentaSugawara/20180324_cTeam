using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Main_ModelViewWindow : MonoBehaviour {
	[SerializeField]
	private float _RotateScale;

	private float _RotateX;
	private float _RotateY;

	private List<int> _triggerHash = new List<int>(){
		Animator.StringToHash("Idle"),
		Animator.StringToHash("Walk"),
		Animator.StringToHash("Run"),
		Animator.StringToHash("Jump"),
		Animator.StringToHash("Unique"),
	};

	private Animator _childAnimator;

	[SerializeField]
	private float _changeAnimInterval = 3;

	public void Init()
    {
        transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        var angles = transform.localRotation.eulerAngles;
        _RotateX = angles.y;
        _RotateY = angles.x;

		_childAnimator = GetComponentInChildren<Animator>();
    }

	private void OnEnable() {
		StartCoroutine(ChangeMotion());
	}

	IEnumerator ChangeMotion() {
		while (true) {
			foreach(var trigger in _triggerHash) {
				yield return new WaitForSeconds(_changeAnimInterval);
				_childAnimator.SetTrigger(trigger);
			}
		}
	}

	private Vector2 _LastPoint;
    public void PointerDown(BaseEventData e)
    {
        PointerEventData point = (PointerEventData)e;

        _LastPoint = point.position;
    }

    public void Drag(BaseEventData e)
    {
        PointerEventData point = (PointerEventData)e;

        transform.Rotate(new Vector3(-_RotateScale * (_LastPoint.y - point.position.y), _RotateScale * (_LastPoint.x - point.position.x), 0.0f), Space.World);

		_LastPoint = point.position;
    }
}
