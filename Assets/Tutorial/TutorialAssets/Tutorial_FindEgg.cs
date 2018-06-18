using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_FindEgg : TutorialMethod {
    public override void Method(System.Action endcallback)
    {
        StartCoroutine(Routine_Find(endcallback));
    }

    private IEnumerator Routine_Find(System.Action endcallback)
    {
        while (_isActive)
        {
            Debug.Log("serching");
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

        endcallback();
    }
}
