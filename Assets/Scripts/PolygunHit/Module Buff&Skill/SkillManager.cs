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
   public class SkillManager : Singleton<SkillManager>
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

      public List<SkillBase> mySkills;
      /// <summary>
      /// 玩家技能列表
      /// </summary>
      private List<ISkill> playerSkills;

      #endregion

      #region base
      protected override void Awake()
      {
         base.Awake();
         mySkills = new List<SkillBase>();
      }

      private void Start()
      {
         playerSkills = new List<ISkill>();
      }
      public void SetSkillData(AllSkillData data)
      {
         AllSkillData = data.skillList;

         baseSkillData = AllSkillData;

         readUseSkillData = baseSkillData;

         Debug.Log(baseSkillData);
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

      #region Add
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
      public void AddSkill(int SkillSN)
      {
         ISkill newSkill = GetClassNameFromSN(SkillSN).GetClassFromString("Tea.PolygonHit");
         AddSkill(newSkill);
      }
      public void AddSkill(string ClassName)
      {
         ISkill newSkill = ClassName.GetClassFromString("Tea.PolygonHit");
         AddSkill(newSkill);
      }
      /// <summary>
      /// 添加技能
      /// </summary>
      /// <param name="someSkill"></param>
      public void AddSkill(ISkill someSkill)
      {
         if (someSkill != null)
         {
            playerSkills.Add(someSkill);
            someSkill.GetSkill();
            EventControl.SetGameState(GameState.Gameing);
         }
         else
         {
            Debug.LogWarning("技能类不存在 添加技能失败");
         }
      }
      /// <summary>
      /// 从SN中获取类名
      /// </summary>
      /// <param name="SN"></param>
      /// <returns></returns>
      private string GetClassNameFromSN(int SN)
      {
         for (int i = 0; i < AllSkillData.Count; i++)
         {
            if (AllSkillData[i].skillSN == SN)
            {
               return AllSkillData[i].skillClassName;
            }
         }
         Debug.Log($"未找到匹配{SN}序号的技能");
         return null;
      }
      #endregion
   }
}
