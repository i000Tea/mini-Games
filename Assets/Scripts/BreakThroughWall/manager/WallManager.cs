using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;
using static UnityEngine.Rendering.ReloadAttribute;
using UnityEngine.UIElements;

namespace Tea.BreakThroughWall
{
   public class WallManager : Singleton<WallManager>
   {
      #region  V

      /// <summary>
      /// 墙的数据
      /// </summary>
      [SerializeField] private WallDataList wallData;
      [SerializeField] private Transform Round;

      #region hinder
      /// <summary> 阻碍墙的父节点 </summary>
      [Header("Hinder 阻碍")]
      [SerializeField] private Transform hinderParent;
      /// <summary> 阻碍墙的数据集 </summary>
      private List<HinderWallData> hinderData => wallData.HinderWallDatas;
      /// <summary> 阻碍墙的脚本集 </summary>
      private List<HinderWall> hinderWalls;

      /// <summary> 阻碍墙的动画 </summary>
      [SerializeField] private float hinderBackLength = 275;
      [SerializeField] private float hinderBackWaitTime = 0.3f;
      [SerializeField] private float hinderBackTime = 0.5f;
      [SerializeField] private Ease hinderBackEase = Ease.OutExpo;
      [Space(5)]
      [SerializeField] private float hinderShowWaitTime = 1f;
      [SerializeField] private float hinderShowTime = 0.45f;
      [SerializeField] private Ease hinderShowEase = Ease.OutExpo;

      /// <summary> 粒子预制件 </summary>
      [SerializeField] private GameObject hinderCrushParticlePrefab;
      #endregion

      #region award
      [Header("award 奖励")]
      [SerializeField] private Transform awardParent;
      /// <summary> 奖励墙的数据集 </summary>
      private List<AwardWallData> awardData => wallData.AwardWallDatas;
      /// <summary> 奖励墙的脚本集 </summary>
      private List<AwardWall> awardWalls;
      [SerializeField] private float awardShowWaitTime = 0.3f;
      [SerializeField] private float awardShowTime = 0.5f;
      [SerializeField] private Ease awardShowEase = Ease.OutExpo;
      [Space(5)]
      /// <summary> 阻碍墙的动画 </summary>
      [SerializeField] private float awardBackLength = 275;
      [SerializeField] private float awardBackWaitTime = 1f;
      [SerializeField] private float awardBackTime = 0.45f;
      [SerializeField] private Ease awardBackEase = Ease.OutExpo;
      #endregion

      #endregion

      private IEnumerator Start()
      {
         yield return new WaitForFixedUpdate();
         hinderWalls ??= new List<HinderWall>();
         for (int i = 0; i < hinderParent.childCount; i++)
         {
            if (hinderParent.GetChild(i).TryGetComponent(out HinderWall hWall))
            {
               hinderWalls.Add(hWall);
            }
         }

         awardWalls ??= new List<AwardWall>();
         for (int i = 0; i < awardParent.childCount; i++)
         {
            if (awardParent.GetChild(i).TryGetComponent(out AwardWall aWall))
            {
               awardWalls.Add(aWall);
               PackagedDoMove(aWall, 0.01f, moveLength: awardBackLength);
            }
         }
         SetWallData();
      }

      #region Wall 
      public HinderWall GetWall(MoveDirection inputDir)
      {
         for (int i = 0; i < hinderWalls.Count; i++)
         {
            if (hinderWalls[i].MyDirection == inputDir)
            {
               return hinderWalls[i];
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
      public IEnumerator HWallsReturn(HinderWall BrokenWall)
      {
         // 等待
         yield return new WaitForSeconds(hinderBackWaitTime);
         // 执行
         for (int i = 0; i < hinderWalls.Count; i++)
         {
            if (hinderWalls[i] == BrokenWall)
            {
               Transform create = BrokenWall.CreateParticlePoint;
               var obj = Instantiate(hinderCrushParticlePrefab, create.position, create.rotation);
               Destroy(obj, 5);
               BrokenWall.gameObject.SetActive(false);
               //continue;
            }
            PackagedDoMove(hinderWalls[i], hinderBackTime, hinderBackEase, hinderBackLength);
         }
         // 等待
         yield return new WaitForSeconds(hinderBackTime + hinderShowWaitTime);

         // 设置
         SetWallData();

         //设置
         Round.localPosition = MovementControl.I.pLocalPosition;

         // 等待
         //yield return new WaitUntil(() => );

         // 设置
         for (int i = 0; i < hinderWalls.Count; i++)
         {
            PackagedDoMove(hinderWalls[i], hinderShowTime, hinderShowEase);
         }
         // 等待
         yield return new WaitForSeconds(hinderShowTime);
      }

      private IEnumerator ShowAWall()
      {
         // 等待
         yield return new WaitForSeconds(awardShowWaitTime);
         // 执行
         for (int i = 0; i < hinderWalls.Count; i++)
         {
            PackagedDoMove(awardWalls[i], awardShowTime, awardShowEase);
         }
         // 等待
         yield return new WaitForSeconds(hinderBackTime + hinderBackWaitTime);
      }

      void PackagedDoMove(BaseWall wall, float moveTime, Ease ease = Ease.Flash, float moveLength = 0)
      {
         var target = wall.Axis;
         if (target)
         {
            target.DOLocalMove(wall.MyDirection.DirToPoint() * moveLength, moveTime).SetEase(ease);
         }
      }

      private void SetWallData()
      {
         for (int i = 0; i < hinderWalls.Count; i++)
         {
            hinderWalls[i].SetData(wallData.RandomHData());
         }
      }
   }
}
