using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterList", menuName = "CharacterList")]
public class Assets_CharacterList : ScriptableObject {
    public List<CharacterData> CharacterList = new List<CharacterData>();
}

[System.Serializable]
public class CharacterData
{
    public int CloseID; // 内部ID
    public int OpenID;  // 表示ID
    public string ViewName; //表示名
    public GameObject Prefab;
    public Sprite sprite;
    public Color Color = Color.white;
    public string Text;
}
