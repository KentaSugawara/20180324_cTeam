using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class ItemCreatorARCore : MonoBehaviour
{

    [SerializeField]
    Mesh _Mesh;

    [SerializeField]
    GameObject[] _items;

    [SerializeField]
    EggSpawnerARCore _EggSpawner;

    GameObject _ActiveItem;

    void Start()
    {
    }

    public void CreateItem(GameObject item)
    {
        StartCoroutine(PutItem_Coroutine(item));
    }

    private IEnumerator PutItem_Coroutine(GameObject item)
    {
        var itemObj = Instantiate(item);

#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
#elif UNITY_ANDROID
        Touch touch = Input.GetTouch(0);
        while (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
#endif
        {
            //var pos = Camera.main.ScreenToWorldPoint(touch.position);
            //Ray ray = new Ray(pos, Camera.main.transform.forward);
            //RaycastHit hit;
            //if(Physics.Raycast(ray, out hit))
            //{
            //    obj.transform.position = hit.transform.position;
            //}

            TrackableHit hit;
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
                TrackableHitFlags.FeaturePointWithSurfaceNormal;

            Vector2 pos;
#if UNITY_EDITOR
            pos = Input.mousePosition;
#elif UNITY_ANDROID
            pos = touch.position;
#endif
            if (Frame.Raycast(pos.x, pos.y, raycastFilter, out hit))
            {
                if ((hit.Trackable is DetectedPlane) &&
                    Vector3.Dot(Camera.main.transform.position - hit.Pose.position,
                        hit.Pose.rotation * Vector3.up) < 0)
                {
                    Debug.Log("Hit at back of the current DetectedPlane");
                }
                else
                {
                    itemObj.transform.Rotate(0, 180, 0, Space.Self);

                    // サイズ変更
                    //obj.transform.localScale /= 2;

                    var anchor = hit.Trackable.CreateAnchor(hit.Pose);

                    itemObj.transform.parent = anchor.transform;
                }
            }

            yield return null;
        }

        foreach (var egg in _EggSpawner.EggList)
        {
            egg.GetComponent<EggBehaviour>().targetItem = itemObj;
        }

        _ActiveItem = itemObj;

        yield break;
    }

    public void SwitchItem(GameObject item)
    {
        DestroyActiveItem();
        CreateItem(item);
        _ActiveItem = item;
    }

    public void DestroyActiveItem()
    {
        Destroy(_ActiveItem);
    }
}
