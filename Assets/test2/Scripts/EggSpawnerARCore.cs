using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class EggSpawnerARCore : MonoBehaviour {

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private Mesh _Mesh;

    [SerializeField]
    private GameObject[] _EggPrefabs;

    [SerializeField]
    private int _MaxSpawnNum = 10;

    [SerializeField]
    private float _EggSpawnInterval = 1;

    private List<GameObject> _EggList = new List<GameObject>();
    private List<DetectedPlane> _AllPlaneList = new List<DetectedPlane>();

    void Start()
    {
        StartCoroutine(Routine_Spawn());
    }

    void Update()
    {
        //foreach (var egg in _EggList)
        //{
        //    var eggBehavour = egg.GetComponent<EggBehaviour>();
        //    if (eggBehavour._isTaken && !eggBehavour.isInCamera)
        //    {
        //        _EggList.Remove(egg);
        //        Destroy(egg);
        //    }
        //}
    }

    private IEnumerator Routine_Spawn()
    {
        while (true)
        {
            while (_EggList.Count >= _MaxSpawnNum) yield return null;

            Session.GetTrackables<DetectedPlane>(_AllPlaneList);
            Trackable a;
            
            var TrackingPlanes = new List<DetectedPlane>();
            //Trackingのものだけ集める
            foreach (var plane in _AllPlaneList)
            {
                if (plane != null && plane.TrackingState == TrackingState.Tracking)
                {
                    TrackingPlanes.Add(plane);
                }
                //Debug.Log(p.TrackingState.ToString());
            }

            if (TrackingPlanes.Count <= 0)
            {
                yield return null;
                continue;
            }
            else
            {
                //デバッグ表示
                {
                    string str = "";
                    foreach (var p in TrackingPlanes)
                    {
                        str += p.ToString() + "\n";
                    }
                    //Debug.Log(str);
                }
            }

            //ランダムに一つ決める
            var TargetPlane = TrackingPlanes[Random.Range(0, TrackingPlanes.Count)];
            Debug.Log(TargetPlane.ToString());
            Debug.Log(TargetPlane.CenterPose.position);
            Debug.Log(TargetPlane.ExtentX);
            Debug.Log(TargetPlane.ExtentZ);

            RaycastHit hit;
            Ray ray = new Ray(
                new Vector3(
                    TargetPlane.CenterPose.position.x + Random.Range(-TargetPlane.ExtentX * 0.4f, TargetPlane.ExtentX * 0.4f),
                    100.0f,
                    TargetPlane.CenterPose.position.z + Random.Range(-TargetPlane.ExtentZ * 0.4f, TargetPlane.ExtentZ * 0.4f)
                    ),
                Vector3.down);

            if (Physics.Raycast(ray, out hit, 1000.0f, 1 << 12))
            {
                //画面外かどうか
                if (/*CheckScreenOut(hit.point)*/true)
                {
                    var pose = new Pose(hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                    //すぽーん
                    var obj = Instantiate(_EggPrefabs[Random.Range(0, _EggPrefabs.Length)], pose.position, pose.rotation);

                    var anchor = TargetPlane.CreateAnchor(pose);

                    // Make Andy model a child of the anchor.
                    obj.transform.parent = anchor.transform;

                    _EggList.Add(obj);
                }
            }

            yield return new WaitForSeconds(_EggSpawnInterval);
        }
    }

    public void Spawn()
    {
        Session.GetTrackables<DetectedPlane>(_AllPlaneList);

        var TrackingPlanes = new List<DetectedPlane>();

        //Trackingのものだけ集める
        foreach (var plane in _AllPlaneList)
        {
            if (plane != null && plane.TrackingState == TrackingState.Tracking)
            {
                TrackingPlanes.Add(plane);
            }
            //Debug.Log(p.TrackingState.ToString());
        }

        if (TrackingPlanes.Count <= 0)
        {
            return;
        }
        else
        {
            //デバッグ表示
            {
                string str = "";
                foreach (var p in TrackingPlanes)
                {
                    str += p.ToString() + "\n";
                }
                //Debug.Log(str);
            }
        }

        //ランダムに一つ決める
        var TargetPlane = TrackingPlanes[Random.Range(0, TrackingPlanes.Count)];
        Debug.Log(TargetPlane.ToString());
        Debug.Log(TargetPlane.CenterPose.position);
        Debug.Log(TargetPlane.ExtentX);
        Debug.Log(TargetPlane.ExtentZ);

        RaycastHit hit;
        Ray ray = new Ray(
            new Vector3(
                TargetPlane.CenterPose.position.x + Random.Range(-TargetPlane.ExtentX * 0.4f, TargetPlane.ExtentX * 0.4f),
                100.0f,
                TargetPlane.CenterPose.position.z + Random.Range(-TargetPlane.ExtentZ * 0.4f, TargetPlane.ExtentZ * 0.4f)
                ),
            Vector3.down);

        if (Physics.Raycast(ray, out hit, 1000.0f, 1<<12))
        {
            //画面外かどうか
            if (CheckScreenOut(hit.point))
            {
                var pose = new Pose(hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                //すぽーん
                var obj = Instantiate(_EggPrefabs[Random.Range(0, _EggPrefabs.Length)], pose.position, pose.rotation);

                var anchor = TargetPlane.CreateAnchor(pose);

                // Make Andy model a child of the anchor.
                obj.transform.parent = anchor.transform;

                _EggList.Add(obj);
            }
        }
    }

    public static bool CheckScreenOut(Vector3 _pos)
    {
        Vector3 view_pos = Camera.main.WorldToViewportPoint(_pos);
        if (view_pos.x < -0.2f ||
           view_pos.x > 1.2f ||
           view_pos.y < -0.2f ||
           view_pos.y > 1.2f)
        {
            // 範囲外 
            return true;
        }
        // 範囲内 
        return false;
    }

    public void DestroyAllObjects()
    {
        if (_EggList.Count > 0)
        {
            foreach (var obj in _EggList)
            {
                if (obj) Destroy(obj);
            }
            _EggList.Clear();
        }
    }

    public void CheckEggsInCamera()
    {
        foreach (var egg in _EggList)
        {
            if (!egg)
            {
                _EggList.Remove(egg);
                continue;
            }
            var eggBehaviour = egg.GetComponent<EggBehaviour>();
            if (eggBehaviour.isInCamera) eggBehaviour._isTaken = true;
            Debug.Log(eggBehaviour._isTaken);
        }
    }

    public List<GameObject> EggList { get { return _EggList; } }
    public int MaxNum { get { return _MaxSpawnNum; } }
}
