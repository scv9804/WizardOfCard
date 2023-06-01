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

                //요소를 추가
                prop.arraySize++;

                //마지막 요소를 선택상태로 만들기
                list.index = prop.arraySize - 1;

                //추가한 요소에 문자열을 추가하기(배열이 string[]일 것을 전제로 합니다）
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
                //어떤 배열 프로퍼티에 얽힌 요소의 갱신
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