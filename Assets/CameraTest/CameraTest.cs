using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraTest : MonoBehaviour
{
    [SerializeField]
    int width = 1920;
    [SerializeField]
    int height = 1080;
    int fps = 60;
    WebCamTexture webcamTexture;

    [SerializeField]
    private Camera _MainCamera;

    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        webcamTexture = new WebCamTexture(devices[0].name, this.height, this.width, this.fps);
        GetComponent<Renderer>().material.mainTexture = webcamTexture;
        webcamTexture.Play();
    }

    //int width = 1920;
    //int height = 1080;
    //int fps = 30;
    //Texture2D texture;
    //WebCamTexture webcamTexture;
    //Color32[] colors = null;

    //IEnumerator Init()
    //{
    //    while (true)
    //    {
    //        if (webcamTexture.width > 16 && webcamTexture.height > 16)
    //        {
    //            colors = new Color32[webcamTexture.width * webcamTexture.height];
    //            texture = new Texture2D(webcamTexture.width, webcamTexture.height, TextureFormat.RGBA32, false);
    //            GetComponent<Renderer>().material.mainTexture = texture;
    //            break;
    //        }
    //        yield return null;
    //    }
    //}

    //// Use this for initialization
    //void Start()
    //{
    //    WebCamDevice[] devices = WebCamTexture.devices;
    //    webcamTexture = new WebCamTexture(devices[0].name, this.width, this.height, this.fps);
    //    webcamTexture.Play();

    //    StartCoroutine(Init());
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (colors != null)
    //    {
    //        webcamTexture.GetPixels32(colors);

    //        int width = webcamTexture.width;
    //        int height = webcamTexture.height;
    //        Color32 rc = new Color32();

    //        for (int x = 0; x < width; x++)
    //        {
    //            for (int y = 0; y < height; y++)
    //            {
    //                Color c = colors[x + y * webcamTexture.width];
    //                colors[x + y * webcamTexture.width] = new Color(c.grayscale, c.grayscale, c.grayscale);
    //            }
    //        }

    //        texture.SetPixels32(colors);
    //        texture.Apply();
    //    }
    //}

    //[SerializeField]
    //private Text _Text;

    //[SerializeField]
    //private int m_width = 1080;
    //[SerializeField]
    //private int m_height = 1920;
    //[SerializeField]
    //private RawImage m_displayUI = null;

    //private WebCamTexture m_webCamTexture = null;

    //private IEnumerator Start()
    //{
    //    yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);

    //    if (WebCamTexture.devices.Length == 0)
    //    {
    //        _Text.text += "カメラのデバイスが無い様だ。撮影は諦めよう。\n";
    //        yield break;
    //    }


    //    if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
    //    {
    //        _Text.text += "カメラを使うことが許可されていないようだ。市役所に届けでてくれ！\n";
    //        yield break;
    //    }

    //    //foreach (var device in WebCamTexture.devices)
    //    //{
    //    //    _Text.text += device.name + "\n";
    //    //}

    //    // とりあえず最初に取得されたデバイスを使ってテクスチャを作りますよ。
    //    WebCamDevice userCameraDevice = WebCamTexture.devices[0];
    //    WebCamTexture wcam = new WebCamTexture(userCameraDevice.name);
    //    wcam.Play();
    //    m_webCamTexture = new WebCamTexture(userCameraDevice.name, wcam.width, wcam.height, 60);
    //    wcam.Stop();
    //    m_displayUI.texture = m_webCamTexture;

    //    // さあ、撮影開始だ！
    //    m_webCamTexture.Play();
    //}

    //public WebCamTexture camTex { get { return m_webCamTexture; } }
}