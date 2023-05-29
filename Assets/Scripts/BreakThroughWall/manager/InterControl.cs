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
   public class InterControl : Singleton<InterControl>
   {
      #region V

      #region Fight
      public float Attack
      {
         get => attack;
         set
         {
            attack = value;
            CanvasManaver.I.Atk.text = value.ToString();
         }
      }
      [SerializeField] private float attack;
      public float AtkNum
      {
         get => atkNum;
         set
         {
            atkNum = value;
            CanvasManaver.I.AtkNum.text = value.ToString();

         }
      }
      [SerializeField] private float atkNum;
      public float Cost
      {
         get => cost;
         set
         {
            cost = value;
            CanvasManaver.I.Cost.text = value.ToString();

         }
      }
      [SerializeField] private float cost;

      private bool animing;
      #endregion

      #endregion

      private void Start()
      {
         Attack = Attack;
         AtkNum = AtkNum;
         Cost = Cost;
      }
      private void Update()
      {
         if (animing) { return; }
         DetectKeyInput();
      }
      public void JoyStickInput(Vector2 joyStick)
      {
         if (animing) { return; }
         var _length = joyStick.magnitude;
         var devOfDir = joyStick.x * joyStick.y;
         Debug.Log($"x {joyStick.x} y {joyStick.y} x*y {devOfDir}");
         if (Mathf.Abs(devOfDir) < 0.4f && _length > 0.85f)
         {
            //横向小于0.5 只能是纵向上下俩
            if (Mathf.Abs(joyStick.x) < 0.5f)
            {
               if (joyStick.y > 0) { OnShoot(MoveDirection.up); }
               else { OnShoot(MoveDirection.down); }
            }
            else
            {
               if (joyStick.x < 0) { OnShoot(MoveDirection.left); }
               else { OnShoot(MoveDirection.right); }
            }
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

      private void OnShoot(MoveDirection inputDir)
      {
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
            _ = StartCoroutine(ProcessOfMovingToPoint(inputDir, targetWall));
         }
      }


      /// <summary>
      /// 移动到点的过程
      /// </summary>
      /// <returns></returns>
      IEnumerator ProcessOfMovingToPoint(MoveDirection dir, HinderWall wall)
      {
         animing = true;
         float time;
         //尝试对墙壁进行攻击 若成功 则执行动画并返回动画长度
         if (wall.TryHitWall(attack))
         {
            MovementControl.I.SuccessMove(dir);
            yield return WallManager.I.HWallsReturn(wall);
         }
         // 否则 播放失败移动的动画
         else
         {
            time = MovementControl.I.FailingMove(dir);
            yield return new WaitForSeconds(time);
         }
         animing = false;
      }
   }
}
