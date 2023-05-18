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
   public delegate void ValueAlter(ref float damage, ValueAlterEffectPhase vaep);
   /// <summary>
   /// 增减益效果
   /// </summary>
   public class IBuff
   {
      /// <summary>
      /// 更新伤害数值
      /// </summary>
      public static ValueAlter AlterDamage;
      /// <summary>
      /// 更新受到伤害的数值
      /// </summary>
      public static ValueAlter AlterInjuried;
   }
}
