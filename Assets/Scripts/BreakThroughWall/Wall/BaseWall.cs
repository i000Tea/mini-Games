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
   }
}