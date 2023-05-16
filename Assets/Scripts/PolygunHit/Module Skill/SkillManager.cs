using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tea.PolygonHit
{
   /// <summary>
   /// 技能管理器
   /// </summary>
   public class SkillManager : MonoBehaviour
   {
      public static SkillManager inst;
      public List<SkillBase> mySkills;
      private void Awake()
      {
         mySkills = new List<SkillBase>();
         inst = this;

      }

      private void Start()
      {
         AddEvent();
      }
      private void OnDestroy()
      {
         RemoveEvent();
      }
      private void AddEvent()
      {
         EventControl.OnAddAntherList<ActionType, EnemyBase>(ActionType.Strike, Trigger_Strike);
         EventControl.OnAddAntherList(ActionType.Shoot,Trigger_Shoot);
      }
      private void RemoveEvent()
      {
         EventControl.OnRemoveAhtnerList<ActionType,EnemyBase>(ActionType.Strike, Trigger_Strike);

         EventControl.OnRemoveAhtnerList(ActionType.Shoot, Trigger_Shoot);
      }

      #region MyRegion
      /// <summary>
      /// 选择技能
      /// </summary>
      public void SelectSkill()
      {
         List<ISkill> availableSkills = GetAvailableSkills();
         List<ISkill> randomSkills = GetRandomSkills(availableSkills, 3);
      }
      private List<ISkill> GetAvailableSkills()
      {
         List<ISkill> availableSkills = new List<ISkill>();
         // 根据玩家已拥有的技能列表，筛选出尚未拥有的技能，并添加到 availableSkills 中
         // 可以根据需要从所有技能中进行筛选，或者从一个已定义的技能池中进行筛选
         return availableSkills;
      }
      private List<ISkill> GetRandomSkills(List<ISkill> skillsList, int count)
      {
         List<ISkill> randomSkills = new List<ISkill>();
         // 从技能列表中随机选择指定数量的技能
         // 可以使用随机数生成器来实现随机选择的逻辑
         return randomSkills;
      }

      #endregion

      public void AddSkill(GameObject inst)
      {
         // 实例化技能 设置参数
         Transform @object = Instantiate(inst).transform;
         @object.SetParent(transform);
         @object.localPosition = Vector3.zero;
         @object.localScale = Vector3.one;

         // 技能脚本添加到方法
         mySkills.Add(@object.GetComponent<SkillBase>());

         // 返回游戏
         GameManager.I.SetState(GameState.Gameing);
      }
      public void AddSkill(int filePathNum)
      {
         // 获取技能位置
         string filePath = "Prefabs/Skills/Skill";
         if (filePathNum < 10)
            filePath += "0";
         filePath += filePathNum.ToString();
         //Debug.Log(filePath);

         AddSkill(Resources.Load<GameObject>(filePath));
      }

      #region Trigger 触发方式
      /// <summary>
      /// 弹射
      /// </summary>
      private void Trigger_Shoot()
      {
         for (int i = 0; i < mySkills.Count; i++)
            mySkills[i].CompareSkill(SkillUseType.IsShoot);
      }
      /// <summary>
      /// 撞击
      /// </summary>
      private void Trigger_Strike(EnemyBase enemy)
      {
         for (int i = 0; i < mySkills.Count; i++)
            mySkills[i].StrikeTrigger(enemy);
      }
      /// <summary>
      /// 受击时
      /// </summary>

      public virtual void Trigger_UnStrike()
      {
         for (int i = 0; i < mySkills.Count; i++)
            mySkills[i].EventTrigger(SkillReplyType.UnStrike);
      }
      /// <summary>
      /// 杀敌时
      /// </summary>

      public virtual void Trigger_Kill()
      {

         for (int i = 0; i < mySkills.Count; i++)
            mySkills[i].EventTrigger(SkillReplyType.Kill);
      }

      /// <summary>
      /// 升级时
      /// </summary>
      public virtual void Trigger_LevelUp()
      {

         for (int i = 0; i < mySkills.Count; i++)
            mySkills[i].EventTrigger(SkillReplyType.LevelUp);
      }

      #endregion
   }
}
