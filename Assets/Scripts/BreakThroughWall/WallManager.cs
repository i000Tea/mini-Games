using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Tea.BreakThroughWall
{
   public class WallManager : Singleton<WallManager>
   {

      #region Wall
      [SerializeField] private Transform parent;
      [SerializeField] private WallDataList dataList;
      private List<WallData> data => dataList.WallDatas;
      private List<FacingWall> walls;
      private List<MoveDirection> wallDirs;
      [SerializeField] private float backWaitTime = 0.3f;
      [SerializeField] private float backTime = 0.5f;
      [SerializeField] private Ease backEase = Ease.OutExpo;
      [SerializeField] private float returnWaitTime = 1f;
      [SerializeField] private float returnTime = 0.45f;
      [SerializeField] private Ease returnEase = Ease.OutExpo;
      #endregion

      private IEnumerator Start()
      {
         yield return new WaitForFixedUpdate();
         SetWallData();
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
      /// 墙壁退下
      /// </summary>
      /// <returns></returns>
      public IEnumerator WallsReturn()
      {
         yield return new WaitForSeconds(backWaitTime);
         for (int i = 0; i < walls.Count; i++)
         {
            var target = walls[i].Axis;
            if (target)
            {
               target.DOLocalMove(wallDirs[i].DirToPoint() * 250, backTime).SetEase(backEase);
            }
         }
         yield return new WaitForSeconds(backTime + returnWaitTime);

         SetWallData();

         parent.localPosition = MovementControl.I.pLocalPosition;

         for (int i = 0; i < walls.Count; i++)
         {
            var target = walls[i].Axis;
            if (target)
            {
               target.DOLocalMove(Vector3.zero, returnTime).SetEase(returnEase);
            }
         }
         yield return new WaitForSeconds(returnTime);
      }
      private void SetWallData()
      {
         for (int i = 0; i < walls.Count; i++)
         {
            walls[i].SetData(dataList.RandomData());
         }
      }

   }
}
