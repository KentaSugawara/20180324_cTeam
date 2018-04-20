using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_ChallengeViewer : MonoBehaviour {

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
