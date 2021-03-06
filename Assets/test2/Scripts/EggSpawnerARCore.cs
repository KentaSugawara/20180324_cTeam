﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class EggSpawnerARCore : MonoBehaviour {

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private GameObject[] _EggPrefabs;

    [SerializeField]
    private int _MaxSpawnNum = 10;

    [SerializeField]
    private float _EggSpawnInterval = 1;

    [SerializeField]
    private float _GiveUpAribalSeconds = 5.0f;

    [SerializeField]
    private float _DistanceOfAlive = 10.0f;

    [SerializeField]
    private float _DestroyCheckDelaySeconds = 5.0f;

    [SerializeField]
    private float _SpawnDistance = 5.0f;

    [SerializeField]
    private bool _LookAtCameraSpawn = false;

    private static List<GameObject> _EggList = new List<GameObject>();
    private static List<int> _EggIDList = new List<int>();
    private List<DetectedPlane> _AllPlaneList = new List<DetectedPlane>();

    private void Awake()
    {
        Instance = this;
        DistanceOfAlive = _DistanceOfAlive;
        DestroyCheckDelaySeconds = _DestroyCheckDelaySeconds;
    }

    void Start()
    {
        _EggList.Clear();
        _EggIDList.Clear();
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

            if (Physics.Raycast(ray, out hit, 1000.0f, 1 << 12) && Vector3.SqrMagnitude(_camera.transform.position - hit.point) < _SpawnDistance * _SpawnDistance)
            {
                //Debug.DrawRay(
                //new Vector3(
                //    TargetPlane.CenterPose.position.x + Random.Range(-TargetPlane.ExtentX * 0.4f, TargetPlane.ExtentX * 0.4f),
                //    100.0f,
                //    TargetPlane.CenterPose.position.z + Random.Range(-TargetPlane.ExtentZ * 0.4f, TargetPlane.ExtentZ * 0.4f)
                //    ),
                //Vector3.down);

                //Meshに出現できるか
                var field = hit.collider.GetComponent<EggSpawnField>();
                if (field != null && field.checkSpawnable())
                {
                    //画面外かどうか
                    if (CheckScreenOut(hit.point))
                    {
                        var pose = new Pose(hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                        //すぽーん
                        int index = getRandomEggID();
                        if (index >= 0) {

                            var randEgg = _EggPrefabs[index];
                            var obj = Instantiate(randEgg, pose.position, pose.rotation);

                            obj.transform.localRotation = randEgg.transform.localRotation;
                            obj.transform.localScale *= 0.45f;

                            //カメラの方向ける
                            if (_LookAtCameraSpawn)
                            {
                                var localrotation = obj.transform.localRotation;
                                obj.transform.LookAt(_camera.transform, Vector3.up);

                                //var angle = localrotation.eulerAngles;
                                //var current = obj.transform.localEulerAngles;
                                //obj.transform.localEulerAngles = new Vector3(angle.x, current.y, angle.z);
                                //var degree = Mathf.Atan(Vector3.Dot(Vector3.forward, (Camera.main.transform.position - hit.point).normalized)) * Mathf.Rad2Deg;
                                //var angle = obj.transform.localRotation.eulerAngles;
                                //obj.transform.localRotation = Quaternion.Euler(angle.x, degree - 90.0f, angle.z);
                            }

                            obj.GetComponent<EggBehaviour>()._camera = _camera;

                            var anchor = TargetPlane.CreateAnchor(pose);

                            // Make Andy model a child of the anchor.
                            obj.transform.parent = anchor.transform;

                            _EggList.Add(obj);
                            _EggIDList.Add(index);

                            field.Spawn(obj);

                            obj.GetComponent<NavMeshCharacter>().Init(this, _GiveUpAribalSeconds);
                        }
                    }
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
                int index = getRandomEggID();
                if (index < 0) return;
                var prefab = _EggPrefabs[index];
                var obj = Instantiate(prefab, pose.position, prefab.transform.rotation);

                var anchor = TargetPlane.CreateAnchor(pose);

                obj.GetComponent<EggBehaviour>()._camera = _camera;

                // Make Andy model a child of the anchor.
                obj.transform.parent = anchor.transform;

                _EggList.Add(obj);
                _EggIDList.Add(index);
            }
        }
    }

    private int getRandomEggID()
    {
        var list = new List<int>();
        for (int i = 0; i < _EggPrefabs.Length; ++i) list.Add(i);
        foreach (var egg in _EggIDList)
        {
            for (int i = 0; i < list.Count; ++i)
            {
                if (list[i] == egg)
                {
                    list.RemoveAt(i);
                    break;
                }
            }
        }
        
        if (list.Count <= 0)
        {
            return -1;
        }

        return list[Random.Range(0, list.Count)];
    }

    public static bool CheckScreenOut(Vector3 _pos)
    {
        if (Camera.main == null) return false;
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
            _EggIDList.Clear();
        }
    }

    /// <summary>
    /// 指定のEggを消す
    /// </summary>
    /// <param name="target"></param>
    public void RemoveEgg(GameObject target)
    {
        if (_EggList.Contains(target))
        {
            for (int i = 0; i < _EggIDList.Count; ++i)
            {
                if (_EggList[i] == target)
                {
                    _EggList.RemoveAt(i);
                    _EggIDList.RemoveAt(i);
                }
            }
        }
    }

    public void CheckEggsInCamera()
    {
        foreach (var egg in _EggList)
        {
            if (!egg)
            {
                RemoveEgg(egg);
                continue;
            }
            var eggBehaviour = egg.GetComponent<EggBehaviour>();
            if (eggBehaviour.isInCamera) eggBehaviour._isTaken = true;
            Debug.Log(eggBehaviour._isTaken);
        }
    }

    public static EggSpawnerARCore Instance { get; private set; }
    public static List<GameObject> EggList { get { return _EggList; } }
    public int MaxNum { get { return _MaxSpawnNum; } }
    public static float DistanceOfAlive { get; private set; }
    public static float DestroyCheckDelaySeconds { get; private set; }
}
