using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationTest : MonoBehaviour {

    [SerializeField]
    private List<Text> _Text = new List<Text>();


	// Use this for initialization
	void Start () {
        //// サービスが有効かチェック
        //if (!Input.location.isEnabledByUser)
        //{
        //    return;
        //}


        StartCoroutine(Routine_WaitEnabled());
    }

    private IEnumerator Routine_WaitEnabled()
    {
        while (!Input.location.isEnabledByUser)
        {
            yield return null;
        }

        Debug.Log("isEnabledByUser --> OK");
        // ロケーションを問合せる前にInput.locationをスタート
        Input.location.Start();
        Debug.Log("location.Start()");

        while (Input.location.status != LocationServiceStatus.Running)
        {
            yield return null;
        }
        StartCoroutine(Routine_Update());
    }

    private double lastTimeStamp = 0.0f;
    private IEnumerator Routine_Update()
    {
        yield return null;


        while (lastTimeStamp >= Input.location.lastData.timestamp)
        {
            yield return null;
        }

        _Text[0].text = "緯度 : " + Input.location.lastData.latitude;
        _Text[1].text = "経度 : " + Input.location.lastData.longitude;
        _Text[2].text = "標高 : " + Input.location.lastData.altitude;
        _Text[3].text = "水平精度 : " + Input.location.lastData.horizontalAccuracy;
        _Text[4].text = "垂直精度 : " + Input.location.lastData.verticalAccuracy;
        _Text[5].text = "タイムスタンプ : " + Input.location.lastData.timestamp;
        lastTimeStamp = Input.location.lastData.timestamp;

        //Input.location.Stop(); 
        StartCoroutine(Routine_Update());
    }
	
	// Update is called once per frame
	//void Update () {


 //       //// 初期化できるまで待つ
 //       //var maxWait : int = 20;
 //       //while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
 //       //{
 //       //    maxWait--;
 //       //}

 //       //// Service didn't initialize in 20 seconds
 //       //if (maxWait < 1)
 //       //{
 //       //    print("Timed out");
 //       //    return;
 //       //}

 //       Debug.Log(Input.location.status);

 //       // 接続状況
 //       if (Input.location.status == LocationServiceStatus.Failed)
 //       {
 //           print("Unable to determine device location");
 //           return;
 //       }
 //       else
 //       {
 //           //Debug.Log("service initializes --> success");
 //           _Text[0].text = "緯度 : " + Input.location.lastData.latitude;
 //           _Text[1].text = "経度 : " + Input.location.lastData.longitude;
 //           _Text[2].text = "標高 : " + Input.location.lastData.altitude;
 //           _Text[3].text = "水平精度 : " + Input.location.lastData.horizontalAccuracy;
 //           _Text[4].text = "垂直精度 : " + Input.location.lastData.verticalAccuracy;
 //           _Text[5].text = "タイムスタンプ : " + Input.location.lastData.timestamp;
 //       }

 //       if (!Input.location.isEnabledByUser)
 //       {
 //           _Text[0].text = "待機中";
 //       }
 //       //Debug.Log("Location: " + Input.location.lastData.latitude + " " + // 緯度
 //       //       Input.location.lastData.longitude + " " +                  // 経度
 //       //       Input.location.lastData.altitude + " " +                   // 標高
 //       //       Input.location.lastData.horizontalAccuracy + " " +         // 水平精度
 //       //       Input.location.lastData.verticalAccuracy + " " +           // 垂直精度
 //       //       Input.location.lastData.timestamp);                        // タイムスタンプ
 //   }
}
