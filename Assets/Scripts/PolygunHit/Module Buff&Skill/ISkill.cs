using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Tea.PolygonHit
{
   /// <summary>
   /// 技能触发器委托
   /// </summary>
   public delegate void SkillTriggerHandler();

   /// <summary>
   /// 生效阶段
   /// </summary>
   public enum EffectivePhase
   {
      none,
      /// <summary> 
      /// 基础增加 <br/>
      /// 如果在这里尽可能用加法
      /// </summary>
      BaseAdd = 1,
      /// <summary> 
      /// 叠加乘算 <br/>
      /// 如果在这里尽可能用乘法
      /// </summary>
      StackMulti = 5,
      /// <summary> 
      /// 最终增加 <br/>
      /// 如果在这里尽可能用加法
      /// </summary>
      FinalAdd = 11,
   }
   /// <summary>
   /// 技能
   /// 技能主要分为两个部分<br/>
   /// 1.主动类 通过各种方式充能或释放的效果<br/>
   /// ps:范围给敌人或友军提供buff应该也算主动类？<br/>
   /// 2.增益类 通过直接在玩家或全体敌人的列表里增加一个或几个buff的一次性动作
   /// </summary>
   public abstract class ISkill
   {
      /// <summary>
      /// 获得此技能时
      /// </summary>
      public void SkillAwake()
      {
         SkillInitialize();
      }
      /// <summary>
      /// 移除技能
      /// </summary>
      public void SkillDestory()
      {
         SkillDelete();
      }
      /// <summary>
      /// 技能 初始化
      /// </summary>
      protected virtual void SkillInitialize() { }
      /// <summary>
      /// 删除技能
      /// </summary>
      protected virtual void SkillDelete() { }

      /// <summary>
      /// 修改伤害
      /// </summary>
      /// <param name="damage"></param>
      protected virtual void ModifyingDamage(ref float damage, EffectivePhase phase) { }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="Injuries"></param>
      protected virtual void ModifyingnInjuries(ref float Injuries) { }

      /// <summary>
      /// 弹射
      /// </summary>
      protected virtual void Trigger_Shoot() { }
      /// <summary>
      /// 撞击
      /// </summary>
      protected virtual void Trigger_Strike(EnemyBase enemy) { }
      /// <summary>
      /// 受击时
      /// </summary>
      protected virtual void Trigger_Injuries() { }
      /// <summary>
      /// 杀敌时
      /// </summary>
      protected virtual void Trigger_Kill() { }
      /// <summary>
      /// 升级时
      /// </summary>
      protected virtual void Trigger_LevelUp() { }

      protected virtual void SomeVoidA() { }
   }
}