using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

[CustomEditor(typeof(DamageAnouncer))]public class CreateCardButton : Editor
{
    public override void OnInspectorGUI()
    { 
        base.OnInspectorGUI();

        DamageAnouncer debuger = (DamageAnouncer)target;
        if (GUILayout.Button("해당 카드를 생성합니다"))
        {
            debuger.CardMaker();
        } 
    }
}
