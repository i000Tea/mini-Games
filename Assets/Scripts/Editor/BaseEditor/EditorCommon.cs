#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq.Expressions;

namespace Tea.tEditor
{
    /// <summary>
    /// 继承自Editor的类 写有部分编辑器重写的方法
    /// </summary>
    public class EditorCommon : Editor
    {
        /// <summary>
        /// 初始化事件
        /// </summary>
        protected virtual void OnEnable() {}

        /// <summary>
        /// 编辑器UI更新方法
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
        }

        /// <summary>
        /// 绘制头文字
        /// </summary>
        /// <param name="label"></param>
        protected static void Header(string label)
        {
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
        }

        /// <summary>
        /// 以字符串的形式 绘制带有缩进的抬头
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        protected bool HeaderFoldable(string label)
        {
            return HeaderFoldable(new GUIContent(label));
        }

        /// <summary>
        /// 以GUI元素的形式绘制带有缩进的抬头
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        protected bool HeaderFoldable(GUIContent label)
        {
            if (ms_StyleHeaderFoldable == null)
            {
                ms_StyleHeaderFoldable = new GUIStyle(EditorStyles.foldout);
                ms_StyleHeaderFoldable.fontStyle = FontStyle.Bold;
            }

            var uniqueString = this.ToString() + label.text;
            bool folded = ms_FoldedHeaders.Contains(uniqueString);

#if UNITY_5_5_OR_NEWER
            folded = !EditorGUILayout.Foldout(!folded, label, toggleOnLabelClick: true, style: ms_StyleHeaderFoldable);
#else
            folded = !EditorGUILayout.Foldout(!folded, label, ms_StyleHeaderFoldable);
#endif

            if (folded) ms_FoldedHeaders.Add(uniqueString);
            else ms_FoldedHeaders.Remove(uniqueString);

            return !folded;
        }

        /// <summary>
        /// 多倍空行
        /// </summary>
        /// <param name="num"></param>
        protected void SpaceMult(int num = 1)
        {
            EditorGUILayout.Space(18f * num);
            if (num > 1) 
                EditorGUILayout.Space(num * 3);
        }

        /// <summary>
        /// 绘制默认分割线
        /// </summary>
        protected static void DrawLineDefaultSeparator(int thickness = 2)
        {
            DrawLineSeparator(Color.grey, thickness, 30);
        } 
        /// <summary>
        /// 绘制小型分割线
        /// </summary>
        protected static void DrawLineLittleSeparators()
        {
            DrawLineSeparator(Color.grey, 1, 15, 26, 20);
        }

        /// <summary>
        /// 绘制分割线
        /// </summary>
        /// <param name="color"></param>
        /// <param name="thickness"></param>
        /// <param name="padding"></param>
        static void DrawLineSeparator(Color color, int thickness = 2, int padding = 10,int flanksL = 0, int flanksR = 0)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding/1.5f + thickness));

            r.x = flanksL;
            if (flanksR == 0 && flanksL != 0) 
                flanksR = flanksL;
            r.width = EditorGUIUtility.currentViewWidth - flanksR * 2;

            r.y += padding / 2;
            r.height = thickness;

            EditorGUI.DrawRect(r, color);
        }
        protected SerializedProperty FindProperty<T, TValue>(Expression<Func<T, TValue>> expr)
        {
            Debug.Assert(serializedObject != null);
            return serializedObject.FindProperty(ReflectionUtils.GetFieldPath(expr));
        }
        #region ButtonMode
        /// <summary>
        /// 仅提供bool
        /// </summary>
        /// <param name="modeType"></param>
        /// <returns></returns>
        protected bool ButtonMode(SerializedProperty modeType)
        {
           return ButtonMode(modeType, EditorStyles.radioButton, EditorStrings.ButtonUseModule, EditorStrings.ButtonCloseModule);
        }
        /// <summary>
        /// 提供bool和按键类型
        /// </summary>
        /// <param name="modeType"></param>
        /// <param name="buttonType"></param>
        /// <returns></returns>
        protected bool ButtonMode(SerializedProperty modeType, GUIStyle buttonType)
        {
           return ButtonMode(modeType, buttonType, EditorStrings.ButtonUseModule, EditorStrings.ButtonCloseModule);
        }
        /// <summary>
        /// 提供bool和一致的名字
        /// </summary>
        /// <param name="modeType"></param>
        /// <param name="ButtonName"></param>
        /// <returns></returns>
        protected bool ButtonMode(SerializedProperty modeType, GUIContent ButtonName)
        {
           return ButtonMode(modeType, EditorStyles.radioButton, ButtonName, ButtonName);
        } 
        /// <summary>
        /// 提供bool和开关不一致的名字
        /// </summary>
        /// <param name="modeType"></param>
        /// <param name="ButtonOpenName">开启时显示的名字</param>
        /// <param name="ButtonCloseName">关闭时显示的名字</param>
        /// <returns></returns>
        protected bool ButtonMode(SerializedProperty modeType, GUIContent ButtonOpenName,GUIContent ButtonCloseName)
        {
           return ButtonMode(modeType, EditorStyles.radioButton, ButtonOpenName, ButtonCloseName);
        }
        /// <summary>
        /// 提供全部信息
        /// </summary>
        /// <param name="modeType">bool</param>
        /// <param name="buttonType">按钮类型</param>
        /// <param name="ButtonOpenName">开启名字</param>
        /// <param name="ButtonCloseName">关闭名字</param>
        /// <returns></returns>
        protected bool ButtonMode(SerializedProperty modeType, GUIStyle buttonType,GUIContent ButtonOpenName,GUIContent ButtonCloseName)
        {
            if (!modeType.boolValue)
            {
                if (GUILayout.Button(ButtonOpenName, buttonType))
                    modeType.boolValue = true;
                return false;
            }
            else
            {
                if (GUILayout.Button(ButtonCloseName, buttonType))
                    modeType.boolValue = false;
                return true;
            }
        }
        #endregion
        /// <summary>
        /// 按钮
        /// </summary>
        protected void ButtonOpenConfig()
        {
            if (GUILayout.Button(EditorStrings.ButtonTest, EditorStyles.miniButton)) { }
            if (GUILayout.Button(EditorStrings.ButtonBIgTest, EditorStyles.radioButton)) { }
            if (GUILayout.Button(EditorStrings.ButtonBIgTest, EditorStyles.toolbarButton)) { }
            if (GUILayout.Button(EditorStrings.ButtonOpenGlobalConfig, EditorStyles.miniButton)) { }
            //Config.EditorSelectInstance();
        }

        static HashSet<String> ms_FoldedHeaders = new HashSet<String>();
        static GUIStyle ms_StyleHeaderFoldable = null;
    }
}
#endif
