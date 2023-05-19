using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tea.PolygonHit
{
   public class SkillGruop_Base : ISkill
   {
   }
   /// <summary>
   /// 强化伤害
   /// </summary>
   public class Skill_1001 : ISkill
   {
      float AddDamageNum = 0.2f;
      /// <summary>
      /// 
      /// </summary>
      Buff_AlterDamage alterDamage;
      protected override void SkillInitialize()
      {
         // 初始化buff
         alterDamage = new AlterDamageInBase(AddDamageNum);
         // buff加入buff列表
         PlayerBase.I.buffList.Add(alterDamage);
         // 加入委托
         PlayerBase.I.valueAlterDamage += alterDamage.AlterDamageIng;
      }
      protected override void SkillDelete()
      {
         PlayerBase.I.buffList.Remove(alterDamage);
         PlayerBase.I.valueAlterDamage -= alterDamage.AlterDamageIng;
      }
   }
   public class Skill_1002 : ISkill
   {
   }
   public class Skill_1003 : ISkill
   {
   }
   public class Skill_1004 : ISkill
   {
   }
   public class Skill_1005 : ISkill
   {
   }

}