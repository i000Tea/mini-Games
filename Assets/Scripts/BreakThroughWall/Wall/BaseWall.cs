using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tea.BreakThroughWall
{
   public class BaseWall : ButtonControlBase
   {
      public Transform Axis => transform.GetChild(0);
      public MoveDirection MyDirection => myDirection;
      [SerializeField] protected MoveDirection myDirection;
      public virtual void OnInter() { }
      protected override void OnEnter()
      {
         base.OnEnter();
         //Debug.Log("进入");
      }
      protected override void OnExit()
      {
         base.OnExit();
         //Debug.Log("离开");
      }
   }
}