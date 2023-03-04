#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using Tea.Sanctuary;

namespace Tea.tEditor
{
    [CanEditMultipleObjects, CustomEditor(typeof(ButtonControlSetting))]
    public class Editor_ButtonControlSetting : EditorCommon
    {
        /// <summary>
        /// 需要更新的目标脚本
        /// </summary>
        private ButtonControlSetting editorTarget;

        SerializedProperty _canClick, _canEnter, _canDraw;
        SerializedProperty _canClickAnim, _canEnterAnim;
        SerializedProperty _click_Scale, _click_Time, _clickBack_Time, _click_UseColor, _click_Color;
        SerializedProperty _enter_Scale, _enter_Time, _enterBack_Time, _enter_UseColor, _enter_Color;

        protected override void OnEnable()
        {
            base.OnEnable();
            editorTarget = (ButtonControlSetting)target;

            _canClick = FindProperty((ButtonControlSetting x) => x.CanClick);
            _canEnter = FindProperty((ButtonControlSetting x) => x.CanEnter);
            _canDraw = FindProperty((ButtonControlSetting x) => x.CanDraw);

            _canClickAnim = FindProperty((ButtonControlSetting x) => x.CanClickAnim);
            _canEnterAnim = FindProperty((ButtonControlSetting x) => x.CanEnterAnim);

            _click_Scale = FindProperty((ButtonControlSetting x) => x.click_Scale);
            _click_Time = FindProperty((ButtonControlSetting x) => x.click_Time);
            _clickBack_Time = FindProperty((ButtonControlSetting x) => x.clickBack_Time);
            _click_UseColor = FindProperty((ButtonControlSetting x) => x.clickBack_Time);
            _click_Color = FindProperty((ButtonControlSetting x) => x.clickBack_Time);

            _enter_Scale = FindProperty((ButtonControlSetting x) => x.enter_Scale);
            _enter_Time = FindProperty((ButtonControlSetting x) => x.enter_Time);
            _enterBack_Time = FindProperty((ButtonControlSetting x) => x.enterBack_Time);
            _enter_UseColor = FindProperty((ButtonControlSetting x) => x.enterBack_Time);
            _enter_Color = FindProperty((ButtonControlSetting x) => x.enterBack_Time);
        }
        public override void OnInspectorGUI()
        {
            // 更新显示
            serializedObject.Update();

            editorTarget = (ButtonControlSetting)target;
            _canClick.ToggleLeft(new GUIContent("点击时是否响应"), GUILayout.MaxWidth(170));
            if (editorTarget.CanClick)
            {
                _canClickAnim.ToggleLeft(new GUIContent("点击时是否有动画"), GUILayout.MaxWidth(170));
                EditorGUILayout.PropertyField(_click_Scale, new GUIContent("点击时倍率", ""));
                EditorGUILayout.PropertyField(_click_Time, new GUIContent("点击时时间", ""));
                EditorGUILayout.PropertyField(_clickBack_Time, new GUIContent("点击后返回的时间", ""));
            }
            SpaceMult(1);
            _canEnter.ToggleLeft(new GUIContent("进入离开时是否响应"), GUILayout.MaxWidth(170));
            if (editorTarget.CanEnter)
            {
                _canEnterAnim.ToggleLeft(new GUIContent("进入离开时是否有动画"), GUILayout.MaxWidth(170));
                EditorGUILayout.PropertyField(_enter_Scale, new GUIContent("进入时倍率", ""));
                EditorGUILayout.PropertyField(_enter_Time, new GUIContent("进入时时间", ""));
                EditorGUILayout.PropertyField(_enterBack_Time, new GUIContent("离开后时间", ""));
            }
            SpaceMult(1);
            _canDraw.ToggleLeft(new GUIContent("是否可以被拖拽"), GUILayout.MaxWidth(170));
            if (editorTarget.CanDraw)
            {
            }

            // 应用属性修改
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif