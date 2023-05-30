using System.Collections;
using System.Collections.Generic;
using Tea.PolygonHit;
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
   public enum WallType
   {
      //空
      none,
      // 面对阻碍
      Hinder,
      // 面对奖励
      Award,
   }
   public class InterControl : Singleton<InterControl>
   {
      #region V

      WallType nowState;
      private bool isShoot = true;

      public static int Difficulty => difficulty;
      public int KillWallNum => difficulty;
      static int difficulty = 0;
      #endregion

      private void Start()
      {
         difficulty = 0;
         nowState = WallType.Hinder;
      }
      private void Update()
      {
         if (!isShoot) { return; }
         DetectKeyInput();
      }
      public void JoyStickInput(Vector2 joyStick)
      {
         // 获取长度
         var _length = joyStick.magnitude;
         // 获取方差
         var devOfDir = joyStick.x * joyStick.y;
         // 打印
         //Debug.Log($"x {joyStick.x} y {joyStick.y} x*y {devOfDir}");
         // 当方差小(指向性明确) 且距离大于0.85 确认为可以发射
         if (Mathf.Abs(devOfDir) < 0.4f && _length > 0.85f)
         {
            MoveDirection inputDir;
            //横向小于0.5 只能是纵向上下俩
            if (Mathf.Abs(joyStick.x) < 0.5f)
            {
               if (joyStick.y > 0) { inputDir = MoveDirection.up; }
               else { inputDir = MoveDirection.down; }
            }
            else
            {
               if (joyStick.x < 0) { inputDir = MoveDirection.left; }
               else { inputDir = MoveDirection.right; }
            }
            OnShoot(inputDir);
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
         if (inputDir == default) { return; }
         OnShoot(inputDir);
      }

      /// <summary>
      /// 朝着某个方向发射
      /// </summary>
      /// <param name="inputDir"></param>
      private void OnShoot(MoveDirection inputDir)
      {
         if (!isShoot) { return; }
         switch (nowState)
         {
            case WallType.none:
               break;

            case WallType.Hinder:
               var targetHWall = WallManager.I.GetHWall(inputDir);   // 获取对应的墙壁
               if (targetHWall == null) { return; }                  // 若脚本为空 返回
               else 
               { 
                  _ = StartCoroutine(HitToHinder(inputDir, targetHWall));
               }
               break;

            case WallType.Award:
               var targetAWall = WallManager.I.GetAWall(inputDir);   // 获取对应的墙壁
               if (targetAWall == null) { return; }                  // 若脚本为空 返回
               else { _ = StartCoroutine(GetSomeAward(inputDir, targetAWall)); }
               break;

            default:
               break;
         }

      }
      /// <summary>
      /// 朝着墙壁撞击
      /// </summary>
      /// <returns></returns>
      private IEnumerator HitToHinder(MoveDirection dir, HinderWall wall)
      {
         isShoot = false;
         //尝试对墙壁进行攻击 若成功 则执行动画并返回动画长度
         if (wall.TryHitWall(FightControl.I.Attack, FightControl.I.AtkNum))
         {
            difficulty++;
            MovementControl.I.SuccessMove(dir);
            FightControl.I.CostAdd();
            yield return WallManager.I.BackSomeWalls(WallType.Hinder, wall);

            if (TryCreateAward())
            {
               yield return WallManager.I.ShowSomeWalls(WallType.Award);
               nowState = WallType.Award;
            }
            else
            {
               yield return WallManager.I.ShowSomeWalls(WallType.Hinder);
            }
         }
         // 否则 播放失败移动的动画 扣除费用
         else
         {
            FightControl.I.MinusCost();
            float time = MovementControl.I.FailingMove(dir);
            yield return new WaitForSeconds(time);
         }

         isShoot = true;
      }

      private IEnumerator GetSomeAward(MoveDirection dir, AwardWall wall)
      {
         isShoot = false;
         FightControl.I.GetAward(wall.getData);
         yield return WallManager.I.BackSomeWalls(WallType.Award, wall);

         nowState = WallType.Hinder;

         yield return WallManager.I.ShowSomeWalls(WallType.Hinder);
         isShoot = true;
      }

      /// <summary>
      /// 检测是否可以生成奖励
      /// </summary>
      /// <returns></returns>
      bool TryCreateAward()
      {
         var r = Random.Range(0f, 1f);
         if (r < WallManager.I.probability)
         {
            return true;
         }
         else
         {
            return false;
         }
      }
   }
}
