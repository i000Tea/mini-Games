using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Tea.BreakThroughWall
{
   public class WallManager : Singleton<WallManager>
   {

      #region Wall
      [SerializeField]
      private Transform parent;
      private List<FacingWall> walls;
      private List<MoveDirection> wallDirs;
      #endregion

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
      /// 墙壁退下
      /// </summary>
      /// <returns></returns>
      public IEnumerator WallsReturn(float time)
      {
         for (int i = 0; i < walls.Count; i++)
         {
            var target = walls[i].Axis;
            if (target)
            {
               target.DOLocalMove(wallDirs[i].DirToPoint() * 200, time);
            }
         }
         yield return new WaitForSeconds(time + 0.2f);

         RandomWallData();

         parent.localPosition = MovementControl.I.pLocalPosition;

         for (int i = 0; i < walls.Count; i++)
         {
            var target = walls[i].Axis;
            if (target)
            {
               target.DOLocalMove(Vector3.zero, time * 2);
            }
         }
         yield return new WaitForSeconds(1);
      }
      private void RandomWallData()
      {
         for (int i = 0; i < walls.Count; i++)
         {
            walls[i].RandomData();
         }
      }
   }
}
