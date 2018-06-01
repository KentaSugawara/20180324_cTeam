using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    protected static Singleton instance;

    protected Singleton() { }

    public static Singleton Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("Singleton");
                instance = obj.AddComponent<Singleton>();
            }
            return instance;
        }
    }

    protected void Awake()
    {
        if (instance != null) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        instance = this;

    }

    public void Show()
    {
    }
}
