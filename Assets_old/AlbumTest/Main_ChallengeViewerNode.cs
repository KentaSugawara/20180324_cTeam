using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main_ChallengeViewerNode : MonoBehaviour {
    [SerializeField]
    private Text _Text;

    [SerializeField]
    private Image _Image_Clear;

    [SerializeField]
    private Image _Image_NotClear;

    [SerializeField]
    private Image _Image_New;

    private Main_ChallengeViewer _ParentComponent;
    private Json_Challenge_ListNode _mySaveData;
    private ChallengeData _myData;

    public void Init(Main_ChallengeViewer parent, Json_Challenge_ListNode mySaveData, ChallengeData myData)
    {
        _ParentComponent = parent;
        _mySaveData = mySaveData;
        _myData = myData;
        UpdateView();
    }

    public void UpdateView()
    {
        _Text.text = _myData.Text;
        if (_mySaveData.isCleard)
        {
            _Image_Clear.gameObject.SetActive(true);
            _Image_NotClear.gameObject.SetActive(false);
            _Image_New.gameObject.SetActive(_mySaveData.isNewCleard);
        }
        else
        {
            _Image_Clear.gameObject.SetActive(false);
            _Image_NotClear.gameObject.SetActive(true);
            _Image_New.gameObject.SetActive(false);
        }
    }
}
