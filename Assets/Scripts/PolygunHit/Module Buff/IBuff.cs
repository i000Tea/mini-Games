using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Tea.PolygonHit
{
   /// <summary>
   /// 增减益效果
   /// </summary>
   public class IBuff
   {
      BuffTarget m_target;
      /// <summary>
      /// 初始化
      /// </summary>
      public virtual void BuffAwake()
      {

      }
      public virtual void IsTime()
      {

      }
      /// <summary>
      /// 弹射时执行
      /// </summary>
      /// <param name="dmg"></param>
      public virtual void IsShoot()
      {

      }
      /// <summary>
      /// 移动时执行
      /// </summary>
      public virtual void IsMovement()
      {

      }
      /// <summary>
      /// 撞击时执行
      /// </summary>
      /// <param name="dmg">原伤害值</param>
      public virtual void IsStrike(ref float dmg)
      {

      }
      /// <summary>
      /// 受到撞击时执行
      /// </summary>
      /// <param name="unDmg">受到伤害值</param>
      public virtual void UnStrike(ref float unDmg)
      {

      }
   }
}
