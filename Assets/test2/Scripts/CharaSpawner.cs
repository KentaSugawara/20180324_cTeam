using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class CharaSpawner : MonoBehaviour {
    [SerializeField]
    private Mesh _Mesh;

    [SerializeField]
    private int _MaxSpawnCount;

    [SerializeField]
    private GameObject[] _CharaPrefabs;

    private List<GameObject> _Characters = new List<GameObject>();
    private List<DetectedPlane> _AllPlanes = new List<DetectedPlane>();

    private void Start()
    {
        StartCoroutine(Routine_Spawn());
    }

    public void DestroyAll()
    {
        StopAllCoroutines();
        foreach (var c in _Characters)
        {
            Destroy(c.gameObject);
        }
        _Characters.Clear();
        StartCoroutine(Routine_Spawn());
    }

    private IEnumerator Routine_Spawn()
    {
        while (true)
        {
            while (_MaxSpawnCount <= _Characters.Count) yield return null;

            Spawn();

            yield return new WaitForSeconds(0.5f);
        }
    }

    public void Spawn()
    {
        Session.GetTrackables<DetectedPlane>(_AllPlanes);


        var TrackingPlanes = new List<DetectedPlane>();

        //Trackingのものだけ集める
        foreach (var p in _AllPlanes)
        {
            if (p != null && p.TrackingState == TrackingState.Tracking)
            {

                TrackingPlanes.Add(p);
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
                TargetPlane.CenterPose.position.x + Random.Range(-TargetPlane.ExtentX * 0.5f, TargetPlane.ExtentX * 0.5f),
                100.0f,
                TargetPlane.CenterPose.position.z + Random.Range(-TargetPlane.ExtentZ * 0.5f, TargetPlane.ExtentZ * 0.5f)
                ),
            Vector3.down);

        if (Physics.Raycast(ray, out hit))
        {
            //画面外かどうか
            if (CheckScreenOut(hit.point))
            {

                var pose = new Pose(hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                //すぽーん
                var obj = Instantiate(_CharaPrefabs[Random.Range(0, _CharaPrefabs.Length)], pose.position, pose.rotation);

                var anchor = TargetPlane.CreateAnchor(pose);

                // Make Andy model a child of the anchor.
                obj.transform.parent = anchor.transform;

                _Characters.Add(obj);
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
}
