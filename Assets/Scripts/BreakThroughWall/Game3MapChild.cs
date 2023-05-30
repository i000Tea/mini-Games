using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tea.BreakThroughWall
{
   public class Game3MapChild : MonoBehaviour
   {
      public float left;
      public float right;
      public float XdiffValue;
      public List<RectTransform> Index => index;
      [SerializeField] List<RectTransform> index;
      //private void OnValidate()
      //{
      //   index = new List<RectTransform>();
      //   for (int i = 0; i < transform.childCount; i++)
      //   {
      //      index.Add(transform.GetChild(i) as RectTransform);
      //   }
      //}
      private void Start()
      {
         XdiffValue = Mathf.Abs(index[0].position.x - index[1].position.x);
      }
      public void MoveLeft()
      {
         index[index.Count - 1].transform.position = index[0].transform.position + Vector3.left * XdiffValue;
         index = Game3Map.I.MoveLastToFirst(index);
      }

      public void MoveRight()
      {
         index[0].transform.position = index[index.Count - 1].transform.position + Vector3.right * XdiffValue;
         index = Game3Map.I.MoveFirstToLast(index);
      }
   }
}