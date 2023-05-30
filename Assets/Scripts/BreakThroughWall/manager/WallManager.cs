using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;
using static UnityEngine.Rendering.ReloadAttribute;
using UnityEngine.UIElements;
using System;
using System.Linq;

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

      /// <summary> 奖励墙的脚本集 </summary>
      private List<AwardWall> awardWalls;
      /// <summary> 阻碍墙的脚本集 </summary>
      private List<HinderWall> hinderWalls;
      #region hinder
      /// <summary> 阻碍墙的父节点 </summary>
      [Header("Hinder 阻碍")]
      [SerializeField] private Transform hinderParent;
      /// <summary> 阻碍墙的数据集 </summary>
      private List<HinderWallData> hinderData => wallData.HinderWallDatas;
      /// <summary> 阻碍墙的动画 </summary>
      [SerializeField] private ShowAndBackTweenDataGroup hTweenGruop;

      /// <summary> 粒子预制件 </summary>
      public GameObject HinderCrushParticlePrefab => hinderCrushParticlePrefab;
      [SerializeField] private GameObject hinderCrushParticlePrefab;
      #endregion

      #region award
      [Header("award 奖励")]
      [Range(0, 1)] public float probability = 0.3f;
      [SerializeField] private Transform awardParent;

      [SerializeField] private ShowAndBackTweenDataGroup aTweenGruop;
      /// <summary> 奖励墙的数据集 </summary>
      private List<AwardWallData> awardData => wallData.AwardWallDatas;
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
               PackagedDoMove(aWall, 0.01f, moveLength: aTweenGruop.BackAnim.AnimLength);
            }
         }
         SetHWallData();
      }

      #region Wall Get
      public HinderWall GetHWall(MoveDirection inputDir)
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
      public AwardWall GetAWall(MoveDirection inputDir)
      {
         for (int i = 0; i < awardWalls.Count; i++)
         {
            if (awardWalls[i].MyDirection == inputDir)
            {
               return awardWalls[i];
            }
         }
         Debug.LogWarning("未找到相对应的墙");
         return null;
      }
      #endregion

      #region H A Wall 
      /// <summary>
      /// 将某一列墙 刷新出现
      /// </summary>
      /// <returns></returns>
      public IEnumerator ShowSomeWalls(WallType type)
      {
         // 更新位置
         Round.localPosition = MovementControl.I.pLocalPosition;
         yield return new WaitForFixedUpdate();
         var localData = SomeWallAnimData(type).ShowAnim;
         // 显示相对应的父集
         ShowWallParent(type);
         // 设置相关数据
         SetWallData(type);
         // 等待
         yield return new WaitForSeconds(localData.WaitTime);
         var someList = GetWallList(type);
         // 设置
         for (int i = 0; i < someList.Count; i++)
         {
            PackagedDoMove(someList[i], localData);
         }
         // 等待
         yield return new WaitForSeconds(localData.AnimTime);
      }
      /// <summary>
      /// 将某一组墙 退回
      /// </summary>
      /// <param name="type"></param>
      /// <returns></returns>
      public IEnumerator BackSomeWalls(WallType type, BaseWall BrokenWall)
      {
         //Debug.Log("返回"+ type);
         var localData = SomeWallAnimData(type).BackAnim;
         yield return new WaitForSeconds(localData.WaitTime);  // 等待
         var someList = GetWallList(type);
         for (int i = 0; i < someList.Count; i++)           // 执行
         {
            if (someList[i] == BrokenWall)
            {
               BrokenWall.OnInter();
            }
            PackagedDoMove(someList[i], localData, true);
         }
         yield return new WaitForSeconds(localData.AnimTime);
      }
      #endregion

      #region other
      void PackagedDoMove(BaseWall wall, TweenDataGroup data, bool useLength = false)
      {
         var target = wall.Axis;
         if (target)
         {
            Vector3 inputLength = default;
            if (useLength)
            {
               inputLength = wall.MyDirection.DirToPoint() * data.AnimLength;
            }
            target.DOLocalMove(inputLength, data.AnimTime).SetEase(data.AnimEase);
         }
      }
      void PackagedDoMove(BaseWall wall, float moveTime, Ease ease = Ease.Flash, float moveLength = 0)
      {
         var target = wall.Axis;
         if (target)
         {
            target.DOLocalMove(wall.MyDirection.DirToPoint() * moveLength, moveTime).SetEase(ease);
         }
      }

      private void ShowWallParent(WallType state)
      {
         hinderParent.gameObject.SetActive(false);
         awardParent.gameObject.SetActive(false);
         switch (state)
         {
            case WallType.none:
               break;
            case WallType.Hinder:
               hinderParent.gameObject.SetActive(true);
               break;
            case WallType.Award:
               awardParent.gameObject.SetActive(true);
               break;
            default:
               break;
         }
      }
      List<BaseWall> GetWallList(WallType type)
      {
         switch (type)
         {
            case WallType.Hinder:
               return hinderWalls.Cast<BaseWall>().ToList();
            case WallType.Award:
               return awardWalls.Cast<BaseWall>().ToList();
            default:
               break;
         }
         return null;
      }
      ShowAndBackTweenDataGroup SomeWallAnimData(WallType type)
      {
         switch (type)
         {
            case WallType.Hinder:
               return hTweenGruop;
            case WallType.Award:
               return aTweenGruop;
            default:
               break;
         }
         return null;
      }
      private void SetWallData(WallType type)
      {
         switch (type)
         {
            case WallType.none:
               break;
            case WallType.Hinder:
               for (int i = 0; i < hinderWalls.Count; i++)
               {
                  hinderWalls[i].SetData(wallData.RandomHData());
               }
               break;
            case WallType.Award:
               for (int i = 0; i < awardWalls.Count; i++)
               {
                  awardWalls[i].SetData(wallData.RandomAData());
               }
               break;
            default:
               break;
         }
      }
      private void SetHWallData()
      {
         for (int i = 0; i < hinderWalls.Count; i++)
         {
            hinderWalls[i].SetData(wallData.RandomHData());
         }
      }
      private void SetAWallData()
      {
         for (int i = 0; i < awardWalls.Count; i++)
         {
            awardWalls[i].SetData(wallData.RandomAData());
         }
      }

      #endregion
   }
}
