using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Tea.BreakThroughWall
{
   public class MovementControl : Singleton<MovementControl>
   {
      [SerializeField]
      private Transform Player;
      private Tween twAnim;
      /// <summary>
      /// 动画是否进行中
      /// </summary>
      public bool IsAniming
      {
         get
         {
            if (twAnim == null)
            {
               return false;
            }
            else
            {
               return twAnim.active;
            }
         }
      }

      [SerializeField]
      float moveScale = 100;
      [SerializeField]
      [Range(0.1f, 2)]
      float moveTime = 1;

      public void Movement(MoveDirection dir, bool isSuccess)
      {
         if (isSuccess)
         {
            var AddPoint = DirToPoint(dir) * moveScale + Player.localPosition;
            //Debug.Log(AddPoint);
            twAnim = Player.DOLocalMove(AddPoint, moveTime).SetEase(Ease.OutExpo);
         }
      }
      Vector3 DirToPoint(MoveDirection dir)
      {
         switch (dir)
         {
            case MoveDirection.up:
               return Vector3.up;
            case MoveDirection.down:
               return Vector3.down;
            case MoveDirection.left:
               return Vector3.left;
            case MoveDirection.right:
               return Vector3.right;
            default:
               break;
         }
         return default;
      }
   }
}
