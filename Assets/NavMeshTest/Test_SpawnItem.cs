using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_SpawnItem : MonoBehaviour {

    [SerializeField]
    private List<GameObject> _Prefab_Items;

    [SerializeField]
    private int _SelectedIndex = 0;

    private List<GameObject> _Instances = new List<GameObject>();

    public void SelectIndex(int index)
    {
        _SelectedIndex = index;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnItem();
        }
    }

    public void SpawnItem()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000.0f, 1 << 12))
        {
            var pose = new Pose(hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));

            var obj = Instantiate(_Prefab_Items[_SelectedIndex], pose.position, pose.rotation);

            _Instances.Add(obj);
        }
    }

    public void ClearItems()
    {
        foreach (var obj in _Instances)
        {
            Destroy(obj);
        }
        _Instances.Clear();
    }
}
