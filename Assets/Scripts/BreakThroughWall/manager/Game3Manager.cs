using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tea.BreakThroughWall
{
   public class Game3Manager : MonoBehaviour
   {
      [SerializeField]
      float setGameTime = 1;
      IEnumerator Start ()
      {
         yield return new WaitForSeconds(1);
         Time.timeScale = setGameTime;
      }
   }
}
