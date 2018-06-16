using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore;

public class Main_ItemViewer : MonoBehaviour {

    [SerializeField]
    private GameObject _Prefab_Node;

    [SerializeField]
    private Assets_ItemList _Asset_ItemList;

    [SerializeField]
    private RectTransform _BackGround;

    [SerializeField]
    private GameObject _Obj_New;

    [SerializeField]
    private Text _Text_NumOfNew;

    [SerializeField]
    private Text _Text_NumOf;

    [SerializeField]
    private ScrollRect _ScrollView;

    [SerializeField]
    private ContentSizeFitter _ContentSizeFitter;

    [SerializeField]
    private float _ToOpenNeedSeconds;

    [SerializeField]
    private List<GameObject> _Prefab_Items;

    [SerializeField]
    private AudioSource _Audio_ItemCreate;

    [SerializeField]
    private AudioSource _Audio_ItemRelease;

    private Vector3 _ViewPosition;

    private bool _isMoving = false;

    private List<Main_ItemViewerNode> _ScrollViewNodes = new List<Main_ItemViewerNode>();

    private void Awake()
    {
        _ViewPosition = _BackGround.anchoredPosition;
    }

    public void ListUpItems()
    {
        for (int i = 0; i < _ScrollViewNodes.Count; ++i)
        {
            Destroy(_ScrollViewNodes[i].gameObject);
        }
        _ScrollViewNodes.Clear();

        int NumOfActive = 0;
        var list = _Asset_ItemList.ItemList;
        for (int i = 0, size = list.Count; i < size; ++i)
        {
            //所持していたならば
            var obj = Instantiate(_Prefab_Node);
            obj.transform.SetParent(_ContentSizeFitter.transform, false);
            var node = obj.GetComponent<Main_ItemViewerNode>();
            var SaveData = Main_ItemManager.ItemSaveData.Data.Find(c => c.CloseID == list[i].CloseID);
            node.Init(this, SaveData, list[i]);
            if (SaveData.isActive) ++NumOfActive;
            _ScrollViewNodes.Add(node);
        }

        _Text_NumOf.text = NumOfActive + "/" + _ScrollViewNodes.Count;
    }

    public void Open()
    {
        if (!_isMoving)
        {
            gameObject.SetActive(true);
            ListUpItems();
            StopAllCoroutines();
            StartCoroutine(Routine_Open());
        }
    }

    public void Close()
    {
        if (!_isMoving)
        {
            StopAllCoroutines();
            Main_ItemManager.UpdateisNew();
            StartCoroutine(Routine_Close());
        }
    }

    private IEnumerator Routine_Open()
    {
        _BackGround.anchoredPosition = new Vector3(_BackGround.sizeDelta.x * 0.6f, _ViewPosition.y, _ViewPosition.z);
        yield return null;
        var deltaSize = Vector2.Scale(_BackGround.sizeDelta, new Vector2(_BackGround.lossyScale.x, _BackGround.lossyScale.y));
        var HidePosition = new Vector3(deltaSize.x * 0.6f, _ViewPosition.y, _ViewPosition.z);
        Vector3 b1;

        _isMoving = true;
        _BackGround.anchoredPosition = HidePosition;


        yield return null;
        _ContentSizeFitter.SetLayoutVertical();
        _ScrollView.verticalNormalizedPosition = 1.0f;

        for (float t = 0.0f; t < _ToOpenNeedSeconds; t += Time.deltaTime)
        {
            float e = t / _ToOpenNeedSeconds;
            b1 = Vector3.Lerp(HidePosition, _ViewPosition, e);
            _BackGround.anchoredPosition = Vector3.Lerp(b1, _ViewPosition, e);

            yield return null;
        }
        _BackGround.anchoredPosition = _ViewPosition;
        _isMoving = false;
    }

    private IEnumerator Routine_Close()
    {
        var deltaSize = Vector2.Scale(_BackGround.sizeDelta, new Vector2(_BackGround.lossyScale.x, _BackGround.lossyScale.y));
        var HidePosition = new Vector3(deltaSize.x * 0.6f, _ViewPosition.y, _ViewPosition.z);
        Vector3 b1;

        _isMoving = true;
        for (float t = 0.0f; t < _ToOpenNeedSeconds; t += Time.deltaTime)
        {
            float e = t / _ToOpenNeedSeconds;
            b1 = Vector3.Lerp(_ViewPosition, HidePosition, e);
            _BackGround.anchoredPosition = Vector3.Lerp(_ViewPosition, b1, e);

            yield return null;
        }
        _BackGround.anchoredPosition = HidePosition;
        gameObject.SetActive(false);
        _isMoving = false;
    }

    private List<DetectedPlane> _AllPlaneList = new List<DetectedPlane>();

    public bool SpawnItem(int CloseID)
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.2f, Screen.height * 0.5f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000.0f, 1 << 12))
        {
            var pose = new Pose(hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));

            var item = Main_ItemManager.ItemList.ItemList.Find(i => i.CloseID == CloseID);
            if (item == null || item.Prefab == null) return false;

            var obj = Instantiate(item.Prefab, pose.position, pose.rotation);

            var plane = hit.collider.gameObject.GetComponent<DetectedPlane>();

            if (plane != null)
            {
                var anchor = plane.CreateAnchor(pose);

                // Make Andy model a child of the anchor.
                obj.transform.parent = anchor.transform;

                Debug.Log("ItemSpawn");
                return true;
            }
        }
        return false;
    }

    [SerializeField]
    private GameObject _Prefab_ItemDragObj;

    private int _ItemIndex;
    private Main_ItemDragObj _DragObj;
    private Main_ItemViewerNode _DragObjChild;

    [SerializeField]
    private Transform _Canvas;

    [SerializeField]
    private RectTransform _Left;

    public void CreateDragObj(Sprite sprite, int ItemIndex, Main_ItemViewerNode child)
    {
        if (_DragObj != null) return;

        _Audio_ItemCreate.Play();
        var obj = Instantiate(_Prefab_ItemDragObj);
        var component = obj.GetComponent<Main_ItemDragObj>();
        component.Init(sprite, ItemIndex);
        obj.transform.SetParent(_Canvas);
        _ItemIndex = ItemIndex;
        _DragObj = component;
        _DragObjChild = child;
    }

    public void ReleaseDragObj(Main_ItemViewerNode child)
    {
        if (_DragObjChild != child || _DragObj == null) return;

        _Audio_ItemRelease.Play();
        Debug.Log(Input.mousePosition + " " + _Left.position);
        if (Input.mousePosition.x < _Left.position.x)
        {
            Debug.Log("Spawn");
            if (SpawnItem(_ItemIndex)) child.SaveData.isNewActive = false;
        }

        Destroy(_DragObj.gameObject);
        _DragObjChild = null;
    }

    public void SetNew(int NumOfNew)
    {
        if (NumOfNew > 0)
        {
            _Obj_New.SetActive(true);
            _Text_NumOfNew.text = NumOfNew.ToString();
        }
        else
        {
            _Obj_New.SetActive(false);
        }
    }
}
