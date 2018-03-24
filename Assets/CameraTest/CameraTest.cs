using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraTest : MonoBehaviour
{
    [SerializeField]
    private Text _Text;

    [SerializeField]
    private int m_width = 1920;
    [SerializeField]
    private int m_height = 1080;
    [SerializeField]
    private RawImage m_displayUI = null;

    private WebCamTexture m_webCamTexture = null;

    private IEnumerator Start()
    {
        if (WebCamTexture.devices.Length == 0)
        {
            _Text.text += "カメラのデバイスが無い様だ。撮影は諦めよう。\n";
            yield break;
        }

        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            _Text.text += "カメラを使うことが許可されていないようだ。市役所に届けでてくれ！\n";
            yield break;
        }

        foreach (var device in WebCamTexture.devices)
        {
            _Text.text += device.name + "\n";
        }

        // とりあえず最初に取得されたデバイスを使ってテクスチャを作りますよ。
        WebCamDevice userCameraDevice = WebCamTexture.devices[0];
        m_webCamTexture = new WebCamTexture(userCameraDevice.name, m_width, m_height);

        m_displayUI.texture = m_webCamTexture;

        // さあ、撮影開始だ！
        m_webCamTexture.Play();
    }
}