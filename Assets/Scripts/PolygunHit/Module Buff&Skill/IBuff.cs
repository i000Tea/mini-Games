using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tea.PolygonHit
{
   /// <summary>
   /// 数值 修改
   /// </summary>
   /// <param name="damage"></param>
   /// <param name="phase"></param>
   public delegate void BuffValueAlter(ref float damage, ValueAlterEffectPhase vaep);
   /// <summary>
   /// 增减益效果
   /// </summary>
   public abstract class IBuff
   {
      /// <summary>
      /// 更新伤害数值
      /// </summary>
      public static BuffValueAlter AlterDamage;
      /// <summary>
      /// 更新受到伤害的数值
      /// </summary>
      public static BuffValueAlter AlterInjuried;
   }
}
