using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling.Memory.Experimental;
using UnityEngine.UI;

namespace Tea.BreakThroughWall
{
   public class AwardWall : BaseWall
   {
      [SerializeField] private Text NameText;
      [SerializeField] private Text introductionText;
      [SerializeField] private Text propertyText;
      public AwardWallData getData { get; private set; }
      public string SetPropertyText
      {
         get
         {
            if (getData == null)
            {
               return "";
            }
            string setPropertyText = default;
            if (getData.atk != 0)
            {
               setPropertyText += $"攻击力提升{getData.atk} ";
            }
            if (getData.atkNum != 0)
            {
               setPropertyText += $"攻击频率提升{getData.atkNum} ";
            }
            if (getData.cost != 0)
            {
               setPropertyText += $"费用回复{getData.cost} ";
            }
            return setPropertyText;
         }
      }
      public void SetData(AwardWallData data)
      {
         getData = data;
         NameText.text = getData.Name;
         introductionText.text = getData.introduction;
         propertyText.text = SetPropertyText;
      }
      protected override void OnEnter()
      {
         base.OnEnter();
         CanvasManaver.I.ShowAwardState(getData.Name, SetPropertyText,getData.introduction);
      }
      protected override void OnExit()
      {
         base.OnExit();
         CanvasManaver.I.BackAward();
      }
   }
}