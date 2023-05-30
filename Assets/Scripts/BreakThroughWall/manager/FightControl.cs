using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace Tea.BreakThroughWall
{
   public class FightControl : Singleton<FightControl>
   {
      #region Fight
      public float Attack
      {
         get
         {
            var outputATK = (baseAtk * (1 + muiltAtk) + addAtk);
            if (outputATK <= 1)
            {
               return 1;
            }
            else
            {
               return outputATK;
            }
         }
      }
      [SerializeField] private float baseAtk = 5;
      private float muiltAtk = 0;
      private float addAtk = 0;

      public float AtkNum => baseAtkNum + addAtkNum;

      [SerializeField] private float baseAtkNum = 1;
      private float addAtkNum = 0;

      public float Cost
      {
         get => cost;
         private set
         {
            Debug.Log("费用更新" + (value - cost));
            cost = value;
            CanvasManaver.I.Cost.text = string.Format("{0:N2}", Cost);
         }
      }
      [SerializeField] private float cost = 3;
      int costAward;

      #endregion
      private void Start()
      {
         UpdateGUI();
      }
      public void GetAward(AwardWallData data)
      {
         if (data == null) { return; }
         if (data.atkMulit)
         {
            muiltAtk += data.atk;
         }
         else
         {
            addAtk += data.atk;
         }
         addAtkNum += data.atkNum;
         if (data.cost != 0)
         {
            Cost += data.cost;
         }
         UpdateGUI();
      }
      private void UpdateGUI()
      {
         CanvasManaver.I.Atk.text = string.Format("{0:N2}", Attack);
         CanvasManaver.I.AtkNum.text = string.Format("{0:N2}", AtkNum);
         CanvasManaver.I.Cost.text = string.Format("{0:N2}", Cost);
      }
      public void MinusCost(int value = 1)
      {
         costAward = 0;
         Cost--;
         if (Cost < 0)
         {
            CanvasManaver.I.GameOver();
         }
      }
      public void CostAdd()
      {
         costAward++;
         int addValue;
         if (costAward > 32 + 1)
         {
            addValue = 6;
         }
         else if (costAward > 16 + 1)
         {
            addValue = 5;
         }
         else if (costAward > 8 + 1)
         {
            addValue = 4;
         }
         else if (costAward > 4 + 1)
         {
            addValue = 3;
         }
         else if (costAward > 2 + 1)
         {
            addValue = 2;
         }
         else
         {
            addValue = 1;
         }
         Cost += addValue;
      }
   }
}