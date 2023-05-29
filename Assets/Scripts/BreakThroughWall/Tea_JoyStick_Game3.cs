using System.Collections;
using System.Collections.Generic;
using Tea;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tea.BreakThroughWall
{
   public class Tea_JoyStick_Game3 : Tea_JoyStick
   {
      public override void OnEndDrag(PointerEventData eventData)
      {
         base.OnEndDrag(eventData);
         InterControl.I.JoyStickInput(inputContent);
      }
   }
}
