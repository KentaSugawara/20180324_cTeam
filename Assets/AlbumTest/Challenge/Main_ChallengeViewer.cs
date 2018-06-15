using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Main_ChallengeViewer : MonoBehaviour {

    [SerializeField]
    private GameObject _Prefab_Node;

    [SerializeField]
    private Assets_ChallengeList _Asset_ChallengeList;

    [SerializeField]
    private RectTransform _BackGround;

    [SerializeField]
    private GameObject _Obj_New;
    public void SetNew(bool value)
    {
        if (_Obj_New != null) _Obj_New.SetActive(value);
    }

    [SerializeField]
    private Text _Text_NumOf;

    [SerializeField]
    private ScrollRect _ScrollView;

    [SerializeField]
    private ContentSizeFitter _ContentSizeFitter;

    [SerializeField]
    private float _ToOpenNeedSeconds;

    private Vector3 _ViewPosition;

    private bool _isMoving = false;

    private List<Main_ChallengeViewerNode> _ScrollViewNodes = new List<Main_ChallengeViewerNode>();

    private void Awake()
    {
        _ViewPosition = _BackGround.anchoredPosition;
    }

    public void ListUpChallenges()
    {
        for (int i = 0; i < _ScrollViewNodes.Count; ++i)
        {
            Destroy(_ScrollViewNodes[i].gameObject);
        }
        _ScrollViewNodes.Clear();

        int NumOfClear = 0;
        var list = _Asset_ChallengeList.ChallengeList;
        for (int i = 0, size = list.Count; i < size; ++i)
        {
            var obj = Instantiate(_Prefab_Node);
            obj.transform.SetParent(_ContentSizeFitter.transform, false);
            var node = obj.GetComponent<Main_ChallengeViewerNode>();
            var SaveData = Main_ChallengeManager.ChallengeSaveData.Data.Find(c => c.CloseID == list[i].CloseID);
            node.Init(this, SaveData, list[i]);
            if (SaveData.isCleard) ++NumOfClear;
            _ScrollViewNodes.Add(node);
        }

        SortChallengesInstance();

        _Text_NumOf.text = NumOfClear + "/" + _ScrollViewNodes.Count;
    }

    public void SortChallengesInstance()
    {
        Debug.Log("Sort");
        //_ScrollViewNodes.OrderByDescending(x => x.name);//.ThenBy(x => x.SaveData.isCleard ? 0 : 1).ThenBy(x => x.SaveData.isNewCleard ? 0 : 1)
        _ScrollViewNodes.Sort((a, b) => a.Data.CloseID - b.Data.CloseID);
        for (int i = _ScrollViewNodes.Count - 1; i >= 0; --i)
        {
            if (_ScrollViewNodes[i].SaveData.isNewCleard)
            {
                //先頭に代入
                var target = _ScrollViewNodes[i];
                _ScrollViewNodes.RemoveAt(i);
                _ScrollViewNodes.Insert(0, target);
            }
        }

        for (int i = 0; i < _ScrollViewNodes.Count; ++i)
        {
            if (_ScrollViewNodes[i].SaveData.isCleard && !_ScrollViewNodes[i].SaveData.isNewCleard)
            {
                //最後尾に代入
                var target = _ScrollViewNodes[i];
                _ScrollViewNodes.RemoveAt(i);
                _ScrollViewNodes.Add(target);
            }
        }
        //_ScrollViewNodes.Sort((a, b) => (b.SaveData.isCleard ? 0 : 1) - (a.SaveData.isCleard ? 0 : 1));
        //_ScrollViewNodes.Sort((a, b) => a.SaveData.isNewCleard ? 1 : 0/*(a.SaveData.isNewCleard ? 0 : 1) - (b.SaveData.isNewCleard ? 0 : 1)*/);
        for (int i = 0; i < _ScrollViewNodes.Count; ++i)
        {
            _ScrollViewNodes[i].transform.SetSiblingIndex(0);
        }

        for (int i = 0; i < _ScrollViewNodes.Count; ++i)
        {
            _ScrollViewNodes[i].transform.SetSiblingIndex(i);
            _ScrollViewNodes[i].SaveData.isNewCleard = false;
        }
    }

    public void Open()
    {
        if (!_isMoving)
        {
            gameObject.SetActive(true);
            ListUpChallenges();

            StopAllCoroutines();
            StartCoroutine(Routine_Open());
        }
    }

    public void Close()
    {
        if (!_isMoving)
        {
            StopAllCoroutines();
            StartCoroutine(Routine_Close());
        }
    }

    private IEnumerator Routine_Open()
    {
        _BackGround.anchoredPosition = new Vector3(-_BackGround.sizeDelta.x * 0.6f, _ViewPosition.y, _ViewPosition.z);
        yield return null;
        var deltaSize = Vector2.Scale(_BackGround.sizeDelta, new Vector2(_BackGround.lossyScale.x, _BackGround.lossyScale.y));
        var HidePosition = new Vector3(-deltaSize.x * 0.6f, _ViewPosition.y, _ViewPosition.z);
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
        var HidePosition = new Vector3(-deltaSize.x * 0.6f, _ViewPosition.y, _ViewPosition.z);
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
}
