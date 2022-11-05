using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

[CustomEditor(typeof(DebugManager))]public class CreateCardButton : Editor
{
    public override void OnInspectorGUI()
    { 
        base.OnInspectorGUI();

        DebugManager debuger = (DebugManager)target;

        if (GUILayout.Button("해당 카드를 생성합니다"))
        {
            debuger.CardMaker();
        } 
    }
}
