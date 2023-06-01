using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using UnityEditorInternal;

namespace WIP
{
    [CustomEditor(typeof(Tester))]
    public class AreaCustomEditor : Editor
    {
        ReorderableList reorderableList;

        void OnEnable()
        {
            var prop = serializedObject.FindProperty("texts");

            reorderableList = new ReorderableList(serializedObject, prop);

            reorderableList.drawHeaderCallback = (rect) =>
                                 EditorGUI.LabelField(rect, prop.displayName);

            reorderableList.drawElementCallback = (rect, index, isActive, isFocused) => {
                var element = prop.GetArrayElementAtIndex(index);
                rect.height -= 4;
                rect.y += 2;
                EditorGUI.PropertyField(rect, element);
            };

            reorderableList.onAddCallback += (list) => {

                //��Ҹ� �߰�
                prop.arraySize++;

                //������ ��Ҹ� ���û��·� �����
                list.index = prop.arraySize - 1;

                //�߰��� ��ҿ� ���ڿ��� �߰��ϱ�(�迭�� string[]�� ���� ������ �մϴ٣�
                var element = prop.GetArrayElementAtIndex(list.index);
                element.stringValue = "New String " + list.index;
            };

            reorderableList.onAddDropdownCallback = (Rect buttonRect, ReorderableList list) =>
            {

                var menu = new GenericMenu();

                menu.AddItem(new GUIContent("Example 1"), false, () =>
                {
                    ReorderableList.defaultBehaviours.DoAddButton(list);
                });
                menu.AddSeparator("");

                menu.AddDisabledItem(new GUIContent("Example 2"));

                menu.DropDown(buttonRect);
            };

            reorderableList.onReorderCallback = (list) => {
                //� �迭 ������Ƽ�� ���� ����� ����
                Debug.Log("onReorderCallback");
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            reorderableList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }
    }
}