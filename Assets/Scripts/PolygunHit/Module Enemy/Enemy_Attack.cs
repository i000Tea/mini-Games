using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Tea.PolygonHit.Enemy
{
   public class Enemy_Attack : Enemy_AddMode
   {
      [SerializeField]
      private int Atk;

      bool MeleeAtk;
      private void OnTriggerEnter2D(Collider2D collision)
      {
         if (collision.transform == Base.TargetTransform)
         {
            switch (Base.AtkMode)
            {
               case EnemyAttackMode.Melee:
                  Base.Movement = false;
                  MeleeAtk = true;
                  break;
               case EnemyAttackMode.speedUp:
                  break;
               case EnemyAttackMode.Charge:
                  break;
            }
         }
      }
      private void FixedUpdate()
      {
         switch (Base.AtkMode)
         {
            case EnemyAttackMode.Melee:
               if (MeleeAtk)
               {
                  Base.TargetPlayer.Injury(Atk);
               }
               break;
            case EnemyAttackMode.speedUp:
               break;
            case EnemyAttackMode.Charge:
               break;
         }

      }
      private void OnTriggerExit2D(Collider2D collision)
      {
         if (collision.transform == Base.TargetTransform)
         {
            switch (Base.AtkMode)
            {
               case EnemyAttackMode.Melee:
                  Base.Movement = true;
                  MeleeAtk = false;
                  break;
               case EnemyAttackMode.speedUp:
                  break;
               case EnemyAttackMode.Charge:
                  break;
            }
         }
      }
   }
}