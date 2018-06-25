using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Main_ItemViewerNode : MonoBehaviour {
    [SerializeField]
    private Text _Text;

    [SerializeField]
    private Image _Image;

    [SerializeField]
    private Image _Image_New;

    [SerializeField]
    private EventTrigger _myEventTrigger;
    public EventTrigger myEventTrigger
    {
        get { return _myEventTrigger; }
    }

    private Main_ItemViewer _ParentComponent;
    private Json_Item_ListNode _mySaveData;
    public Json_Item_ListNode SaveData
    {
        get { return _mySaveData; }
    }
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

    private void Awake()
    {
        EventTrigger currentTrigger = gameObject.AddComponent<EventTrigger>();
        currentTrigger.triggers = new List<EventTrigger.Entry>();

        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((x) => PointerDown());

            currentTrigger.triggers.Add(entry);
        }

        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerUp;
            entry.callback.AddListener((x) => PointerUp());

            currentTrigger.triggers.Add(entry);
        }
        _myEventTrigger = currentTrigger;
    }

    public void PointerDown()
    {
        StopAllCoroutines();
        //StartCoroutine(Routine_Drag());
        _ParentComponent.CreateDragObj(_Image.sprite, _myData.CloseID, this);
    }

    public void PointerUp()
    {
        _ParentComponent.ReleaseDragObj(this);
        StopAllCoroutines();
    }

    public void OnBeginDrag(BaseEventData eventData)
    {
        _ParentComponent.OnBeginDrag((PointerEventData)eventData);
    }

    public void OnEndDrag(BaseEventData eventData)
    {
        _ParentComponent.OnEndDrag((PointerEventData)eventData);
    }

    public void OnDrag(BaseEventData eventData)
    {
        _ParentComponent.OnDrag((PointerEventData)eventData);
    }
}
