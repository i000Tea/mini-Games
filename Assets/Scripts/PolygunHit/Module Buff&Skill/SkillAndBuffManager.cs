using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WeChatWASM;

namespace Tea.PolygonHit
{
   /// <summary>
   /// 技能管理器
   /// </summary>
   public class SkillAndBuffManager : Singleton<SkillAndBuffManager>
   {
      #region MyRegion
      /// <summary>
      /// 所有的技能列表
      /// </summary>
      private List<SkillData> AllSkillData;
      /// <summary>
      /// 基础的技能列表
      /// </summary>
      private List<SkillData> baseSkillData;
      /// <summary>
      /// 准备就绪可以使用的列表
      /// </summary>
      private List<SkillData> readUseSkillData;

      private List<IBuff> bufflist;

      #endregion

      #region base
      protected override void Awake()
      {
         base.Awake();
      }
      
      public void SetSkillData(AllSkillData data)
      {
         AllSkillData = data.skillList;

         baseSkillData = AllSkillData;

         readUseSkillData = baseSkillData;

         //Debug.Log(baseSkillData);
      }
      #endregion

      #region Delegate
      protected override void AddDelegate()
      {
         base.AddDelegate();
         EventControl.OnAddAntherList(ActionType.LevelUp, SetSkillThatProvidesChoice);
      }
      protected override void RemoveDelegate()
      {
         base.RemoveDelegate();
         EventControl.OnRemoveAhtnerList(ActionType.LevelUp, SetSkillThatProvidesChoice);
         
      }

      #endregion

      #region AddSkill
      /// <summary>
      /// 每当等级提升时 查找可以使用的技能并传递给GUI
      /// </summary>
      private void SetSkillThatProvidesChoice()
      {
         SkillData[] datas = new SkillData[3];
         for (int i = 0; i < 3; i++)
         {
            var newData = readUseSkillData[Random.Range(0, readUseSkillData.Count)];
            datas[i] = newData;
         }
         GUIManager.I.InputSkillData(datas);
      }
      public void AddSkill(ISkill someSkill)
      {
         if (someSkill != null)
         {
            PlayerBase.I.skillList.Add(someSkill);
            someSkill.SkillAwake();
            EventControl.SetGameState(GameState.Gameing);
         }
         else
         {
            Debug.LogWarning("技能类不存在 添加技能失败");
         }
      }
      public void AddSkill(string ClassName)
      {
         ISkill newSkill = ClassName.GetSkillFromString("Tea.PolygonHit");
         AddSkill(newSkill);
      }
      #endregion

   }
}
