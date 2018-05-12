using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main_ItemViewerNode : MonoBehaviour {
    [SerializeField]
    private Text _Text;

    [SerializeField]
    private Image _Image;

    [SerializeField]
    private Image _Image_New;

    private Main_ItemViewer _ParentComponent;
    private Json_Item_ListNode _mySaveData;
    private ItemData _myData;

    public void Init(Main_ItemViewer parent, Json_Item_ListNode mySaveData, ItemData myData)
    {
        if (mySaveData.isActive)
        {
            _Image.sprite = myData.sprite;
            _ParentComponent = parent;
            _mySaveData = mySaveData;
            _myData = myData;
            UpdateView();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void UpdateView()
    {
        _Text.text = _myData.Text;
        if (_mySaveData.isActive)
        {
            _Image_New.gameObject.SetActive(_mySaveData.isNewActive);
        }
        else
        {
            _Image_New.gameObject.SetActive(false);
        }
    }
}
