using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tea.PolygonHit
{
   /// <summary>
   /// 通用增益效果
   /// </summary>
   public class BuffGroup_General : IBuff { }
   /// <summary>
   /// 修改伤害
   /// </summary>
   public class Buff_AlterDamage : IBuff
   {
      protected float AlterFactor;
      protected ValueAlterEffectPhase effectPhase = ValueAlterEffectPhase.none;
      protected Buff_AlterDamage() { }
      protected Buff_AlterDamage(float inputAlterFactor) { AlterFactor = inputAlterFactor; }
      public Buff_AlterDamage(float inputAlterFactor, ValueAlterEffectPhase phase)
      {
         AlterFactor = inputAlterFactor;
         effectPhase = phase;
      }

      /// <summary>
      /// 修改伤害的过程 
      /// </summary>
      /// <param name="damage"></param>
      /// <param name="nowPhase"></param>
      public virtual void AlterDamageIng(ref float damage, ValueAlterEffectPhase nowPhase)
      {
         if (nowPhase == effectPhase)
         {
            if (effectPhase == ValueAlterEffectPhase.StackMulti)
            {
               damage *= AlterFactor;
            }
            else
            {
               damage += AlterFactor;
            }
         }
      }
      /// <summary>
      /// 更改自己的因子
      /// </summary>
      /// <param name="factor"></param>
      public virtual void ChangeMyFactor(float factor)
      {
         AlterFactor = factor;
      }
   }
   /// <summary>
   /// 基础伤害修改
   /// </summary>
   public class AlterDamageInBase : Buff_AlterDamage
   {
      public AlterDamageInBase(float inputAlterFactor)
      {
         AlterFactor = inputAlterFactor;
         effectPhase = ValueAlterEffectPhase.BaseAdd;
      }
   }
   /// <summary>
   /// 对基础伤害乘算
   /// </summary>
   public class AlterDamageInStack : Buff_AlterDamage
   {
      public AlterDamageInStack(float inputAlterFactor)
      {
         AlterFactor = inputAlterFactor;
         effectPhase = ValueAlterEffectPhase.StackMulti;
      }
   }
   /// <summary>
   /// 加成在最终伤害
   /// </summary>
   public class AlterDamageInFinal : Buff_AlterDamage
   {
      public AlterDamageInFinal(float inputAlterFactor)
      {
         AlterFactor = inputAlterFactor;
         effectPhase = ValueAlterEffectPhase.FinalAdd;
      }
   }
}
