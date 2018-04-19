using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class AlbumTest_Snapshot : MonoBehaviour
{
    [SerializeField]
    Camera m_camera = null;
    [SerializeField]
    RenderTexture m_snap = null;
    Texture2D m_tex2d;

    int m_photoNum = 0;

    private void Awake()
    {
        m_camera.targetTexture = null;
    }

    private void Start()
    {
        // Texture2D を作成
        m_tex2d = new Texture2D(m_snap.width, m_snap.height, TextureFormat.RGB24, false);
    }

    private void Update()
    {

    }

    public void ClickSaveButton() { StartCoroutine(SaveCamImage()); }

    IEnumerator SaveCamImage()
    {
        // アクティブなレンダーテクスチャを一時保管
        var curRT = RenderTexture.active;
        // m_snap をアクティブなレンダーテクスチャに設定
        RenderTexture.active = m_snap;
        // m_snap を ターゲットテクスチャに設定
        m_camera.targetTexture = m_snap;
        // レンダリング
        m_camera.Render();
        // レンダリングされているものを m_tex2d に読み込む
        m_tex2d.ReadPixels(new Rect(0, 0, m_snap.width, m_snap.height), 0, 0);

        // テクスチャの上下反転
        //for (int y = 0; y < m_tex2d.height / 2.0f; y++)
        //{
        //    var tmp = m_tex2d.GetPixels(0, y, m_tex2d.width, 1);
        //    m_tex2d.SetPixels(0, y, m_tex2d.width, 1, m_tex2d.GetPixels(0, m_tex2d.height - 1 - y, m_tex2d.width, 1));
        //    m_tex2d.SetPixels(0, m_tex2d.height - 1 - y, m_tex2d.width, 1, tmp);
        //}

        // m_tex2d に変更を反映
        m_tex2d.Apply();

        // アクティブなレンダーテクスチャをもとに戻す
        RenderTexture.active = curRT;
        m_camera.targetTexture = null;
        m_camera.Render();

        // テクスチャを PNG に変換
        byte[] bytes = m_tex2d.EncodeToPNG();

        // 保存するファイル名
        string fileName = "Image" + m_photoNum.ToString("D5");
        
        string filePath = Application.streamingAssetsPath;

        // ディレクトリ名を上の階層から順に格納
        string[] ourDirList = {
            "Pictures",

        };

        // 階層ごとに検索し、ディレクトリが存在しないときはその都度作成
        foreach (var str in ourDirList)
        {
            var directory = new DirectoryInfo(filePath);
            if (!directory.Exists) directory.Create();
            filePath += "/" + str;
        }

        // PNGデータをファイルとして保存
        File.WriteAllBytes(Path.Combine(filePath, fileName) + ".png", bytes);

        m_photoNum++;
        yield break;
    }
}

