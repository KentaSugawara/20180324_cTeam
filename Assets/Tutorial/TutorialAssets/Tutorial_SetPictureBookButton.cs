using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_SetPictureBookButton : TutorialMethod {
    [SerializeField]
    private Main_PictureBookViewer _PictureBookViewer;

    [SerializeField]
    private Main_Tutorial _Tutorial;

    [SerializeField]
    private int _TutorialIndex;

    public void Set()
    {
        _Tutorial.TutorialList[_TutorialIndex].NextButton = _PictureBookViewer.ScrollViewNodes[0].GetComponent<Button>();
    }
}
