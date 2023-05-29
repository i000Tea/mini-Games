using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Tea.BreakThroughWall
{
   /// <summary>
   /// 面朝的墙壁
   /// </summary>
   public class HinderWall : BaseWall
   {
      #region V
      public Transform CreateParticlePoint=> particlePoint;
      [SerializeField] private Transform particlePoint;

      #region Durability 耐久
      private float Durability
      {
         get => durability;
         set
         {
            if (value < 0) { value = 0; }
            durability = value;
            durText.text = durability.ToString();
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
      public bool TryHitWall(float atk)
      {
         var newAtk = atk - Armor;
         if (newAtk <= 0 && atk > 0)
         {
            newAtk = 1;
         }
         Durability -= newAtk;
         Debug.Log($"方位{myDirection}的墙收到{newAtk}攻击 目前剩余{Durability}");
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
         durMax = data.durability;
         Durability = durMax;
         Armor = data.armor;
         nameText.text = data.Name;
      }
   }
}
