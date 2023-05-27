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

      #region Wall
      private List<FacingWall> walls;
      private List<MoveDirection> wallDirs;
      #endregion

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

      #region Wall

      public void AddWall(FacingWall wall, MoveDirection wallDir)
      {
         if (walls == null || wallDirs == null)
         {
            walls = new List<FacingWall>();
            wallDirs = new List<MoveDirection>();
         }
         walls.Add(wall);
         wallDirs.Add(wallDir);
      }
      public FacingWall GetWall(MoveDirection inputDir)
      {
         for (int i = 0; i < wallDirs.Count; i++)
         {
            if (wallDirs[i] == inputDir)
            {
               return walls[i];
            }
         }
         Debug.LogWarning("未找到相对应的墙");
         return null;
      }
      #endregion

      /// <summary>
      /// 检测鼠标按键
      /// </summary>
      void DetectKeyInput()
      {
         MoveDirection inputDir = default;
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
         if (inputDir == default) { return; }
         var targetWall = GetWall(inputDir);
         if (targetWall == null)
         {
            Debug.LogWarning("未获取到墙");
            return;
         }
         else
         {
            MovementControl.I.Movement(inputDir, targetWall.TryHitWall(attack));
         }
      }
   }
}
