using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Snapshot : MonoBehaviour
{
    [SerializeField]
    protected Main_AlbumViewer _AlbumViewer;
    [SerializeField]
    protected Camera m_camera = null;
    [SerializeField]
    protected RenderTexture m_snap = null;
    [SerializeField]
    protected EggSpawnerARCore m_eggSpawner = null;

    [SerializeField]
    protected GameObject m_BackGround;

    [SerializeField]
    protected RectTransform m_SnapShotBackGround;

    [SerializeField]
    protected Image m_SnapShotImage;

    [SerializeField]
    protected float m_ReductionNeedSeconds;

    [SerializeField]
    protected Image m_EggNameImage;

    [SerializeField]
    protected GameObject m_Prefab_New;

    [SerializeField]
    protected float m_New_EnlargementNeedSeconds;

    //[SerializeField]
    //protected float m_NameImage_EnlargementNeedSeconds;

    [SerializeField]
    protected float m_New_ViewSeconds;

    [SerializeField]
    protected float m_New_ReductionNeedSeconds;

    [SerializeField]
    protected float m_New_IntervalSeconds;

    [SerializeField]
    protected float m_SnapShotDistance;

    [SerializeField]
    protected RectTransform m_PictureBookButton;

    [SerializeField]
    protected float m_SnapShot_StoreNeedSeconds;

    [SerializeField]
    protected AudioSource m_Audio_SnapShot;

    [SerializeField]
    protected AudioSource m_Audio_NewChara;

    [SerializeField]
    private List<GameObject> _TestEggList = new List<GameObject>();

#if true//UNITY_ANDROID && !UNITY_EDITOR
    protected Texture2D m_tex2d;
    protected int m_photoNum = 0;
#endif

    protected void Awake()
    {
        m_camera.targetTexture = null;
        m_BackGround.SetActive(false);
    }

    protected void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        // Texture2D を作成
        m_tex2d = new Texture2D(m_snap.width, m_snap.height, TextureFormat.RGB24, false);
#endif
    }

    public void ClickSaveButton() { StartCoroutine(SaveCamImage()); }

    protected IEnumerator SaveCamImage()
    {
        var EggObjlist = new List<KeyValuePair<GameObject, SnapShotInfo>>();

        //foreach (var egg in EggSpawnerARCore.EggList)
        //{
        //    if (egg.GetComponent<EggBehaviour>().isInCamera)
        //        int_list.Add(egg.GetComponent<EggData>()._closeID);
        //}

        RaycastHit hit;
        //foreach (var egg in EggSpawnerARCore.EggList)
        foreach (var egg in EggSpawnerARCore.EggList)
        {
            if (egg == null) continue;
            //範囲外なら棄却
            if ((Camera.main.transform.position - egg.transform.position).sqrMagnitude >= m_SnapShotDistance * m_SnapShotDistance)
            {
                Debug.Log("遠い");
                continue;
            }

            var eggbhaviour = egg.GetComponent<EggBehaviour>();
            var nchara = egg.GetComponent<NavMeshCharacter>();
            if (eggbhaviour.isInCameraForSnap && (nchara.CharaState == NavMeshCharacter.eCharaState.isItemPlaying || eggbhaviour.isFaceToCamera))
            {
                var vector = (egg.transform.position - Camera.main.transform.position);
                Ray ray = new Ray(Camera.main.transform.position, vector.normalized);
                if (Physics.Raycast(ray, out hit, m_SnapShotDistance, 1 << 8))
                {
                    if (hit.collider.gameObject != egg)
                    {
                        Debug.Log("他のにRay当たった");
                        continue;
                    }
                    Debug.Log("成功");
                    var info = new SnapShotInfo();
                    info.CharaCloseIndex = egg.GetComponent<EggData>()._closeID;
                    info.CharaState = nchara.CharaState;
                    info.ItemCloseIndex = nchara.PlayingItemIndex;
                    info._Animator = egg.GetComponent<Animator>();
                    eggbhaviour._isTaken = true;
                    EggObjlist.Add(new KeyValuePair<GameObject, SnapShotInfo>(egg, info));
                }
                else
                {
                    Debug.Log("RayCast失敗");
                }
            }
            else
            {
                Debug.Log("カメラ外");
            }
        }

        var NewEggList = Main_PictureBookManager.GetNewCharacters(EggObjlist);

#if true//UNITY_ANDROID && !UNITY_EDITOR
        // アクティブなレンダーテクスチャを一時保管
        var curRT = RenderTexture.active;
        // m_snap をアクティブなレンダーテクスチャに設定
        RenderTexture.active = m_snap;
        // m_snap を ターゲットテクスチャに設定
        m_camera.targetTexture = m_snap;
        // レンダリング
        m_camera.Render();
        // レンダリングされているものを m_tex2d に読み込む 透明度を消す？
        Texture2D m_tex2d = new Texture2D(m_snap.width, m_snap.height, TextureFormat.RGB24, false);
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
        
        //初期化
        {
            m_EggNameImage.gameObject.SetActive(false);
            m_SnapShotBackGround.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0.0f);
            m_SnapShotBackGround.transform.localScale = Vector3.one;
            m_SnapShotBackGround.transform.rotation = Quaternion.identity;
        }

        List<GameObject> NewImageList = new List<GameObject>();
        //新しいEggがいたらNewを生成しておく
        foreach (var Egg in NewEggList)
        {
            var obj = Instantiate(m_Prefab_New);
            obj.transform.SetParent(m_SnapShotBackGround.transform);

            var renderer = Egg.Key.GetComponent<NavMeshCharacter>().BodyRenderer;

            Vector3 BottomPos = Camera.main.WorldToScreenPoint(Egg.Key.transform.position);
            Vector3 RightPos = Camera.main.WorldToScreenPoint(Egg.Key.transform.position + renderer.bounds.extents);
            Vector3 InScreenHalfSize = RightPos - BottomPos;
            InScreenHalfSize.z = 0.0f;
            Vector3 CenterPos = BottomPos + Vector3.up * InScreenHalfSize.y;

            Vector3 ViewPos = CenterPos;

            //どこに表示するか
            if (CenterPos.x < Screen.width * 0.5f) ViewPos.x += InScreenHalfSize.x + 80.0f;
            else ViewPos.x -= InScreenHalfSize.x + 80.0f;

            if (CenterPos.y < Screen.height * 0.5f) ViewPos.y += InScreenHalfSize.y + 80.0f;
            else ViewPos.y -= InScreenHalfSize.y + 80.0f;

            obj.transform.position = ViewPos;
            obj.SetActive(false);

            NewImageList.Add(obj);
        }

        //写真を表示
        {
            m_BackGround.SetActive(true);
            m_SnapShotImage.material.mainTexture = m_tex2d;

            m_Audio_SnapShot.Play();

            yield return StartCoroutine(Routine_ImageReduction());
        }

        //新しいEggがいたら表示
        if (NewImageList.Count > 0) {
            yield return StartCoroutine(Routine_NewEgg(NewImageList, NewEggList));
        }
        else
        {
            yield return new WaitForSeconds(1.0f);
        }

        //写真を格納
        {
            yield return StartCoroutine(Routine_StoreImage());
        }

        //終了
        {
            m_BackGround.SetActive(false);
        }

        //アルバムを更新
        Main_PictureBookManager.UpdateAlbum(EggObjlist);
        _AlbumViewer.SnapShot(m_tex2d);


        ////チャレンジ判定に使うリストを作成
        //List<SnapShotInfo> SnapShots = new List<SnapShotInfo>();
        //{
        //    foreach (var egg in EggObjlist)
        //    {
        //        SnapShotInfo s = new SnapShotInfo();
        //    }
        //}
        Main_ChallengeManager.CheckChallenges(EggObjlist);
        Main_ItemManager.CheckUpdateItems();

        //// テクスチャを PNG に変換
        //byte[] bytes = m_tex2d.EncodeToPNG();

        //// 保存するファイル名
        //string fileName = "Image" + m_photoNum.ToString("D5");

        //string filePath = Application.persistentDataPath;

        //// ディレクトリ名を上の階層から順に格納
        //string[] ourDirList = {
        //    "Pictures",
        //    ""
        //};

        //// 階層ごとに検索し、ディレクトリが存在しないときはその都度作成
        //foreach (var str in ourDirList)
        //{
        //    var directory = new DirectoryInfo(filePath);
        //    if (!directory.Exists) directory.Create();
        //    filePath += "/" + str;
        //}

        //Debug.Log("Save " + filePath);

        //// PNGデータをファイルとして保存
        //File.WriteAllBytes(Path.Combine(filePath, fileName) + ".png", bytes);

        //m_photoNum++;
#endif
        yield break;
    }

    protected IEnumerator Routine_ImageReduction()
    {
        m_SnapShotBackGround.localScale = Vector3.one;
        Vector3 b;
        for (float t = 0.0f; t < m_ReductionNeedSeconds; t += Time.deltaTime)
        {
            float e = t / m_ReductionNeedSeconds;
            b = Vector3.Lerp(Vector3.one, Vector3.one * 0.6f, e);
            m_SnapShotBackGround.localScale = Vector3.Lerp(b, Vector3.one * 0.6f, e);
            yield return null;
        }
        m_SnapShotBackGround.localScale = Vector3.one * 0.6f;
    }

    protected IEnumerator Routine_NewEgg(List<GameObject> NewImageList, List<KeyValuePair<GameObject, Sprite>> NewEggList)
    {
        m_EggNameImage.gameObject.SetActive(true);
        m_EggNameImage.transform.localScale = Vector3.zero;

        Vector3 b;
        float e;

        //今は一つだけ
        for (int i = 0; i < NewImageList.Count; ++i)
        {
            m_EggNameImage.rectTransform.sizeDelta = new Vector2(NewEggList[i].Value.texture.width * 3.0f, NewEggList[i].Value.texture.height * 3.0f);
            m_EggNameImage.sprite = NewEggList[i].Value;
            //拡大
            NewImageList[i].gameObject.SetActive(true);
            for (float t = 0.0f; t < m_New_EnlargementNeedSeconds; t += Time.deltaTime)
            {
                e = t / m_New_EnlargementNeedSeconds;
                b = Vector3.Lerp(Vector3.zero, Vector3.one, e);
                NewImageList[i].transform.localScale = Vector3.Lerp(b, Vector3.one, e);
                m_EggNameImage.transform.localScale = Vector3.Lerp(b, Vector3.one, e);
                yield return null;
            }
            NewImageList[i].transform.localScale = Vector3.one;

            //for (float t = 0.0f; t < m_NameImage_EnlargementNeedSeconds; t += Time.deltaTime)
            //{
            //    e = t / m_NameImage_EnlargementNeedSeconds;
            //    b = Vector3.Lerp(Vector3.zero, Vector3.one, e);
            //    m_EggNameImage.transform.localScale = Vector3.Lerp(b, Vector3.one, e);
            //    yield return null;
            //}
            m_EggNameImage.transform.localScale = Vector3.one;

            m_Audio_NewChara.Play();
            yield return new WaitForSeconds(m_New_ViewSeconds);

            //縮小
            for (float t = 0.0f; t < m_New_ReductionNeedSeconds; t += Time.deltaTime)
            {
                e = t / m_ReductionNeedSeconds;
                b = Vector3.Lerp(Vector3.one, Vector3.zero, e);
                NewImageList[i].transform.localScale = Vector3.Lerp(b, Vector3.zero, e);
                m_EggNameImage.transform.localScale = NewImageList[i].transform.localScale;
                yield return null;
            }
            m_EggNameImage.transform.localScale = Vector3.zero;
            NewImageList[i].gameObject.SetActive(false);

            yield return new WaitForSeconds(m_New_IntervalSeconds);
        }


        m_EggNameImage.gameObject.SetActive(false);
    }

    protected IEnumerator Routine_StoreImage()
    {
        Vector3 StartPos = m_SnapShotBackGround.transform.position;
        Vector3 EndPos = m_PictureBookButton.position;
        Vector3 StartScale = m_SnapShotBackGround.transform.localScale;
        Quaternion StartRotation = m_SnapShotBackGround.transform.rotation;
        var vec = (StartPos - EndPos).normalized;
        Quaternion EndRotation = Quaternion.FromToRotation(Vector3.up, vec);

        Vector3 b;
        Quaternion q;
        float e;
        for (float t = 0.0f; t < m_SnapShot_StoreNeedSeconds; t += Time.deltaTime)
        {
            e = t / m_New_EnlargementNeedSeconds;
            b = Vector3.Lerp(StartPos, EndPos, e);
            m_SnapShotBackGround.transform.position = Vector3.Lerp(b, EndPos, e);

            b = Vector3.Lerp(StartScale, Vector3.zero, e);
            m_SnapShotBackGround.transform.localScale = Vector3.Lerp(b, Vector3.zero, e);

            q = Quaternion.Lerp(StartRotation, EndRotation, e);
            m_SnapShotBackGround.transform.rotation = Quaternion.Lerp(q, EndRotation, e);

            yield return null;
        }
    }
}
