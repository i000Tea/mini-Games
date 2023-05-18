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
      protected override void SkillInitialize()
      {
      }
      protected override void ModifyingDamage(ref float damage, EffectivePhase phase)
      {
         base.ModifyingDamage(ref damage, phase);
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