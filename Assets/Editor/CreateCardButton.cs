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
        if (GUILayout.Button("�ش� ī�带 �����մϴ�"))
        {
            debuger.CardMaker();
        } 
    }
}
