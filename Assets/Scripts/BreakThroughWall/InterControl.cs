using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Tea.BreakThroughWall
{
   public enum MoveDirection
   {
      none,
      up,
      down,
      left,
      right,
   }
   public class InterControl : Singleton<InterControl>
   {
      #region V

      #region Fight
      [SerializeField]
      float attack;
      [SerializeField]
      float health;
      #endregion

      #endregion

      private void Update()
      {
         if (!MovementControl.I.IsAniming)
         {
            DetectKeyInput();
         }
      }

      /// <summary>
      /// 检测鼠标按键
      /// </summary>
      void DetectKeyInput()
      {
         MoveDirection inputDir = default;
         // 四个方向
         if (Input.GetKeyUp(KeyCode.W))
         {
            inputDir = MoveDirection.up;
         }
         else if (Input.GetKeyUp(KeyCode.S))
         {
            inputDir = MoveDirection.down;
         }
         else if (Input.GetKeyUp(KeyCode.A))
         {
            inputDir = MoveDirection.left;
         }
         else if (Input.GetKeyUp(KeyCode.D))
         {
            inputDir = MoveDirection.right;
         }
         // 若方向为空 则返回
         if (inputDir == default)
         {
            Debug.LogWarning("方向为空");
            return;
         }
         // 获取对应的墙壁
         var targetWall = WallManager.I.GetWall(inputDir);
         // 若脚本为空 返回
         if (targetWall == null)
         {
            Debug.LogWarning("未获取到墙");
            return;
         }
         else
         {
            StartCoroutine(ProcessOfMovingToPoint(inputDir, targetWall));
         }
      }

      /// <summary>
      /// 移动到点的过程
      /// </summary>
      /// <returns></returns>
      IEnumerator ProcessOfMovingToPoint(MoveDirection dir, FacingWall wall)
      {
         float time;
         //尝试对墙壁进行攻击 若成功 则执行动画并返回动画长度
         if (wall.TryHitWall(attack))
         {
            time = MovementControl.I.SuccessMove(dir);
            yield return WallManager.I.WallsReturn(time);
         }
         // 否则 播放失败移动的动画
         else
         {
            time = MovementControl.I.FailingMove(dir);
            yield return new WaitForSeconds(time);
         }
      }
   }
}
