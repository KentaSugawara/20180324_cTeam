using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_SetItem : TutorialMethod {
    [SerializeField]
    private Main_ItemViewer _ItemViewer;

    public override void Method(System.Action endcallback)
    {
        //StartCoroutine(Routine_Find(endcallback));
    }

    private IEnumerator Routine_Find(System.Action endcallback)
    {
        _ItemViewer.StopClose = true;
        while (_isActive)
        {
            if (EggSpawnerARCore.EggList.Count > 0)
            {
                var egg = EggSpawnerARCore.EggList[0].GetComponent<EggBehaviour>();
                if (egg != null)
                {
                    if (egg.isInCamera) break;
                }
            }
            yield return null;
        }

        //アイテム置いたら
        _ItemViewer.Close();

        _ItemViewer.StopClose = false;

        endcallback();
    }
}
