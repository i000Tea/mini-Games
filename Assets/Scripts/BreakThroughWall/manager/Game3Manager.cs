using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

      public void SceneBase()
      {
         SceneManager.LoadScene(0);
      }
      public void SceneReturn()
      {
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
      }
   }
}
