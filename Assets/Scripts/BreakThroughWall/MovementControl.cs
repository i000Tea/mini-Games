using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Tea.BreakThroughWall
{
   public class MovementControl : Singleton<MovementControl>
   {
      public Vector3 pLocalPosition => Player.localPosition;
      [SerializeField]
      private Transform Player;
      public Tween TwAnim { get; private set; }
      /// <summary>
      /// 动画是否进行中
      /// </summary>
      public bool IsAniming
      {
         get
         {
            if (TwAnim == null)
            {
               return false;
            }
            else
            {
               return TwAnim.active;
            }
         }
      }

      [SerializeField]
      float moveScale = 100;
      [SerializeField]
      [Range(0.1f, 2)]
      private float moveTime = 1;

      public void Movement(MoveDirection dir, bool isSuccess)
      {
         if (isSuccess)
         {
         }
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="dir"></param>
      /// <returns></returns>
      public float SuccessMove(MoveDirection dir)
      {
         var AddPoint = dir.DirToPoint() * moveScale + Player.localPosition;
         //Debug.Log(AddPoint);
         TwAnim = Player.DOLocalMove(AddPoint, moveTime).SetEase(Ease.OutExpo);
         return moveTime;
      }
      public float FailingMove(MoveDirection dir)
      {
         return moveTime;
      }
   }
}
