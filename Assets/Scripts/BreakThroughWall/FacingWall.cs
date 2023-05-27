using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tea.BreakThroughWall
{
   public class FacingWall : MonoBehaviour
   {
      [SerializeField]
      private MoveDirection myDirection;
      [SerializeField]
      float durability;
      [SerializeField]
      float armor;

      private void Start()
      {
         InterControl.I.AddWall(this, myDirection);
         if (durability <= 0)
         {
            durability = 1;
         }
      }

      /// <summary>
      /// 尝试攻击墙
      /// </summary>
      /// <returns>是否击碎</returns>
      public bool TryHitWall(float atk)
      {
         var newAtk = atk - armor;
         if (newAtk <= 0 && atk > 0)
         {
            newAtk = 1;
         }
         durability -= newAtk;
         Debug.Log($"方位{myDirection}的墙收到{newAtk}攻击 目前剩余{durability}");
         if (durability <= 0)
         {
            durability = 0;
            return true;
         }
         return false;
      }
   }
}
