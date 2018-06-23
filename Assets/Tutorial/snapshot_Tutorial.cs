using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snapshot_Tutorial : Snapshot {
    public new void ClickSaveButton() { StartCoroutine(SaveCamImage()); }

    [SerializeField]
    private Tutorial_SnapShotEgg _Tutorial_SnapShotEgg;

    protected new IEnumerator SaveCamImage()
    {
        var EggObjlist = new List<KeyValuePair<GameObject, SnapShotInfo>>();

        RaycastHit hit;
        foreach (var egg in EggSpawnerARCore.EggList)
        {
            //範囲外なら棄却
            //if ((Camera.main.transform.position - egg.transform.position).sqrMagnitude >= m_SnapShotDistance * m_SnapShotDistance) continue;
            var eggbhaviour = egg.GetComponent<EggBehaviour>();
            var nchara = egg.GetComponent<NavMeshCharacter>();
            if (eggbhaviour.isInCameraForSnap)
            {
                var vector = (egg.transform.position - Camera.main.transform.position);
                Ray ray = new Ray(Camera.main.transform.position, vector.normalized);
                if (Physics.Raycast(ray, out hit, m_SnapShotDistance, 1 << 8))
                {
                    if (hit.collider.gameObject != egg) continue;

                    var info = new SnapShotInfo();
                    info.CharaCloseIndex = egg.GetComponent<EggData>()._closeID;
                    info.CharaState = nchara.CharaState;
                    info.ItemCloseIndex = nchara.PlayingItemIndex;
                    info._Animator = egg.GetComponent<Animator>();
                    EggObjlist.Add(new KeyValuePair<GameObject, SnapShotInfo>(egg, info));
                }
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
        if (NewImageList.Count > 0)
        {
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

        _Tutorial_SnapShotEgg.CheckSnapShot(EggObjlist);
#endif
        yield break;
    }
}
