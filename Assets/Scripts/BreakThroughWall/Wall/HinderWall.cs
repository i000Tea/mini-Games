using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Profiling.Memory.Experimental;
using System;

namespace Tea.BreakThroughWall
{
   /// <summary>
   /// 面朝的墙壁
   /// </summary>
   public class HinderWall : BaseWall
   {
      #region V
      public Transform CreateParticlePoint => particlePoint;
      [SerializeField] private Transform particlePoint;
      HinderWallData getData;

      #region Durability 耐久
      private float Durability
      {
         get => durability;
         set
         {
            if (value < 0.3) { value = 0; }
            durability = value;
            durText.text = String.Format("{0:N2}", durability);
            durFill.fillAmount = durability / durMax;
         }
      }
      private float durability;
      private float durMax = 1;
      [SerializeField] private Text durText;
      [SerializeField] private Image durFill;
      #endregion

      #region 护甲
      private float Armor
      {
         get => armor;
         set
         {
            armor = value;
            armorText.text = armor.ToString();
         }
      }
      private float armor;
      [SerializeField] private Text armorText;
      #endregion

      [SerializeField] private Animator animator;
      [SerializeField] private Text nameText;

      #endregion

      /// <summary>
      /// 尝试攻击墙
      /// </summary>
      /// <returns>是否击碎</returns>
      public bool TryHitWall(float atk, float num)
      {
         var newAtk = atk - Armor;
         if (newAtk <= 0 && atk > 0)
         {
            newAtk = 1;
         }
         for (int i = 0; i < num; i++)
         {
            Durability -= newAtk;
            if (Durability <= 0)
            {
               break;
            }

         }
         //Debug.Log($"方位{myDirection}的墙收到{newAtk}攻击 目前剩余{Durability}");
         if (Durability <= 0)
         {
            Durability = 0;
            return true;
         }
         return false;
      }
      /// <summary>
      /// 设置属性
      /// </summary>
      public void SetData(HinderWallData data)
      {
         if (!gameObject.activeInHierarchy)
         {
            gameObject.SetActive(true);
         }
         getData = data;
         durMax = data.durability;
         Durability = durMax;
         Armor = data.armor;
         nameText.text = data.Name;
      }

      public override void OnInter()
      {
         base.OnInter();
         var obj = Instantiate(WallManager.I.HinderCrushParticlePrefab, CreateParticlePoint.position, CreateParticlePoint.rotation);
         Destroy(obj, 5);
         gameObject.SetActive(false);
      }
      protected override void OnEnter()
      {
         base.OnEnter();
         CanvasManaver.I.ShowHinderState(getData.Name, getData.durability.ToString(), getData.armor.ToString(), getData.introduction);
      }
      protected override void OnExit()
      {
         base.OnExit();
         CanvasManaver.I.BackHinder();
      }
   }
}
