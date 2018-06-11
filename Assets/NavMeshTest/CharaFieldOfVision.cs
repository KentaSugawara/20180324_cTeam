using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaFieldOfVision : MonoBehaviour {
    public static void Create(Transform parent, NavMeshCharacter component, Vector3 VisionScale)
    {
        var obj_first = new GameObject();
        obj_first.transform.SetParent(parent, false);

        var obj_second = new GameObject();
        obj_second.transform.SetParent(obj_first.transform, false);
        obj_second.transform.localPosition = new Vector3(0.0f, 0.0f, 0.5f);
        obj_first.transform.localScale = VisionScale;

        obj_second.layer = 19; //FieldOfView

        var col = obj_second.AddComponent<BoxCollider>();
        col.center = new Vector3(0.0f, 0.5f, 0.0f);
        col.isTrigger = true;
        var vision = obj_second.AddComponent<CharaFieldOfVision>();

        vision.Init(component);
    }

    [SerializeField]
    private NavMeshCharacter _myComponent;

    private void Init(NavMeshCharacter component)
    {
        _myComponent = component;
    }

    private void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponent<NavMeshTargetPoint>();
        if (target != null)
        {
            _myComponent.SetTargetPoint(target);
        }
    }
}
