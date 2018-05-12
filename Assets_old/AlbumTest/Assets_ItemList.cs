using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemList", menuName = "ItemList")]
public class Assets_ItemList : MonoBehaviour {
    public List<ItemData> ItemList = new List<ItemData>();
}

[System.Serializable]
public class ItemData
{
    public int CloseID; // 内部ID
    public int OpenID;  // 表示ID
    public string ViewName; //表示名
    public GameObject Prefab;
    public Sprite sprite;
    public Color Color = Color.white;
    public string Text;
}