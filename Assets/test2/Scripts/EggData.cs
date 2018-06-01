using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggData : MonoBehaviour {

    [SerializeField]
    int _closeID = 0;

    public int CloseID { get { return _closeID; } }
}
