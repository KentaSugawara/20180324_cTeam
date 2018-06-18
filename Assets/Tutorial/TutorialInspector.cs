using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(Main_Tutorial))]
public class Tutorial_Inspector : Editor {
    Main_Tutorial myComponent = null;
    private void OnEnable()
    {
        myComponent = (Main_Tutorial)target;
    }

    private int _Index;

    public override void OnInspectorGUI()
    {
        _Index = EditorGUILayout.IntField("操作インデックス", _Index);
        if (GUILayout.Button("削除", GUILayout.Height(30)))
        {
            myComponent.DeleteIndex(_Index);
        }

        if (GUILayout.Button("挿入", GUILayout.Height(30)))
        {
            myComponent.InsertIndex(_Index);
        }

        base.OnInspectorGUI();
    }
}
#endif
