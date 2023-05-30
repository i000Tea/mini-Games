using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tea.BreakThroughWall
{
   public class Game3Map : Singleton<Game3Map>
   {
      Camera mainCamera => Camera.main;
      Vector3 cameraPosition => Camera.main.transform.position;
      float YdiffValue;

      [SerializeField] List<Game3MapChild> index;

      private void OnValidate()
      {
         index = new List<Game3MapChild>();
         for (int i = 0; i < transform.childCount; i++)
         {
            if (transform.GetChild(i).TryGetComponent(out Game3MapChild child))
               index.Add(child);
         }
      }
      float cameraSize, aspectRatio;

      Vector3 Map_topLeft, Map_bottomRight;
      Vector3 Camera_topLeft, Camera_bottomRight;
      private void Start()
      {
         // 获取正交摄像机的位置和尺寸
         cameraSize = mainCamera.orthographicSize;

         // 获取屏幕的宽高比
         aspectRatio = Screen.width / (float)Screen.height;

         YdiffValue = Mathf.Abs(index[0].transform.position.y - index[1].transform.position.y);
      }
      private void GetCamera()
      {
         // 计算相机的四周边界点位置
         Camera_topLeft =
            new Vector3(cameraPosition.x - cameraSize * aspectRatio, cameraPosition.y + cameraSize, cameraPosition.z);
         Camera_bottomRight =
            new Vector3(cameraPosition.x + cameraSize * aspectRatio, cameraPosition.y - cameraSize, cameraPosition.z);

      }
      private void Update()
      {
         GetCamera();
         Map_topLeft = index[0].Index[0].position;
         var maxCould = index[0].Index.Count - 1;
         Map_bottomRight = index[index.Count - 1].Index[maxCould].position;

         if (Camera_topLeft.y > Map_topLeft.y)
         {
            MoveUP();
         }
         else if (Camera_bottomRight.y < Map_bottomRight.y)
         {
            MoveDown();
         }
         if (Camera_topLeft.x < Map_topLeft.x)
         {
            MoveLeft();
         }
         else if (Camera_bottomRight.x > Map_bottomRight.x)
         {
            MoveRight();
         }
      }

      void MoveUP()
      {
         //Debug.Log("up");
         index[index.Count - 1].transform.position = index[0].transform.position + Vector3.up * YdiffValue;
         index = MoveLastToFirst(index);
      }
      void MoveDown()
      {
         //Debug.Log("down");
         index[0].transform.position = index[index.Count - 1].transform.position + Vector3.down * YdiffValue;
         index = MoveFirstToLast(index);
      }
      public void MoveLeft()
      {
         //Debug.Log("left");
         for (int i = 0; i < index.Count; i++)
         {
            index[i].MoveLeft();
         }
      }

      public void MoveRight()
      {
         //Debug.Log("right");
         for (int i = 0; i < index.Count; i++)
         {
            index[i].MoveRight();
         }
      }
      /// <summary>
      /// 最后一个到第一个
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="list"></param>
      /// <returns></returns>
      public List<T> MoveLastToFirst<T>(List<T> list)
      {
         if (list.Count <= 1)
         {
            // 列表为空或只有一个元素时不需要移动
            return list;
         }

         T lastElement = list[list.Count - 1];
         list.RemoveAt(list.Count - 1);
         list.Insert(0, lastElement);

         return list;
      }
      /// <summary>
      /// 第一个到最后一个
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="list"></param>
      /// <returns></returns>
      public List<T> MoveFirstToLast<T>(List<T> list)
      {
         if (list.Count <= 1)
         {
            // 列表为空或只有一个元素时不需要移动
            return list;
         }

         T firstElement = list[0];
         list.RemoveAt(0);
         list.Add(firstElement);

         return list;
      }
   }
}