using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Net;

namespace Tea.BreakThroughWall
{
   public class MovementControl : Singleton<MovementControl>
   {
      public Vector3 pLocalPosition => Player.localPosition;
      [SerializeField] private Transform Player;
      [SerializeField] private Ease successAnimEase;
      [SerializeField] private Ease failingAnimEase;
      [SerializeField] private GameObject failingPartialPrefab;

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

      [SerializeField] private float successMoveScale = 100;
      [SerializeField] private float failingMoveScale = 100;
      [SerializeField][Range(0.1f, 2)] private float successMoveTime = 1;
      [SerializeField][Range(0.1f, 2)] private float failingMoveTime = 1;

      /// <summary>
      /// 成功
      /// </summary>
      /// <param name="dir"></param>
      /// <returns></returns>
      public float SuccessMove(MoveDirection dir)
      {
         var AddPoint = dir.DirToPoint() * successMoveScale + Player.localPosition;
         //Debug.Log(AddPoint);
         TwAnim = Player.DOLocalMove(AddPoint, successMoveTime).SetEase(successAnimEase);
         return successMoveTime - 0.05f;
      }
      /// <summary>
      /// 失败
      /// </summary>
      /// <param name="dir"></param>
      /// <returns></returns>
      public float FailingMove(MoveDirection dir)
      {
         // 记录初始坐标
         var basePoint = Player.localPosition;
         // 直接赋值到新的坐标
         Player.localPosition += dir.DirToPoint() * failingMoveScale;
         // 创建一个失败粒子
         var obj = Instantiate(failingPartialPrefab);
         // 粒子位置调整 并延迟移除
         obj.transform.position = Player.position + dir.DirToPoint() * 0.35f;
         obj.transform.localScale = Vector3.one * 0.5f;
         Destroy(obj, 5);
         // 动画回到原位
         TwAnim = Player.DOLocalMove(basePoint, failingMoveTime).SetEase(failingAnimEase);
         // 返回动画时间
         return failingMoveTime - 0.05f;
      }
   }
}
