using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace Tea.PolygonHit
{
   /// <summary>
   /// 游戏管理器
   /// </summary>
   public class GameManager : Singleton<GameManager>
   {
      #region State 游戏状态

      /// <summary>
      /// 暂停状态
      /// </summary>
      public static bool Pause;

      /// <summary>
      /// 游戏速度
      /// </summary>
      private float gameSpeed;
      #endregion

      #region Data 数据

      //private List<SkillData> SkillData;

      private int score;

      public Transform GameCanvas => gameCanvas;
      [SerializeField]
      private Transform gameCanvas;

      public Transform PlayerParent => GameCanvas;
      #endregion

      #region unity void
      protected override void Awake()
      {
         base.Awake();
         Time.timeScale = 1;
         gameSpeed = Time.timeScale;
         // 每次游戏开始前 进行一次GC回收
         GC.Collect();
      }

      #endregion

      /// <summary>
      /// 游戏进程开始
      /// </summary>
      private void GameStart()
      {

      }

      /// <summary>
      /// 跳回0场景
      /// </summary>
      private void ExitGame()
      {
         SceneManager.LoadScene(0);
      }

      #region 广播

      protected override void AddDelegate()
      {
         EventControl.OnAddButtonList(ButtonType.Menu_StartGame, GameStart);
         EventControl.OnAddButtonList(ButtonType.Exit, ExitGame);
         EventControl.OnAddGameStateList(SetState);
      }
      protected override void RemoveDelegate()
      {
         EventControl.OnRemoveButtonList(ButtonType.Menu_StartGame, GameStart);
         EventControl.OnRemoveButtonList(ButtonType.Exit, ExitGame);
         EventControl.OnRemoveGameStateList(SetState);
      }

      /// <summary>
      /// 加分
      /// </summary>
      /// <param name="add"></param>
      public void ScoreAdd(int add = 1)
      {
         score += add;
         GUIManager.I.CalculationScore(score);
      }

      /// <summary>
      /// 设置游戏状态
      /// </summary>
      public void SetState(GameState State)
      {
         //Debug.Log($"状态切换至{State}");
         if (gameSpeed == 0 && Time.timeScale != 0)
         {
            gameSpeed = Time.timeScale;
         }
         switch (State)
         {
            case GameState.Gameing:
               Time.timeScale = gameSpeed;
               break;
            case GameState.Over:
               break;

            default:
               Time.timeScale = 0;
               break;
         }
      }

      #endregion

      public GameObject TeaInstantiate(GameObject instObj, Vector3 position, float Scale = 1, Transform Parent = null)
      {
         return TeaInstantiate(instObj, position, new Quaternion(0, 0, 0, 0), Scale, Parent);
      }
      public GameObject TeaInstantiate(GameObject instObj, Transform trans, float Scale = 1, Transform Parent = null)
      {
         return TeaInstantiate(instObj, trans.position, trans.rotation, Scale, Parent);
      }
      /// <summary>
      /// 重写的方法 生成物体 生成父集 生成位置 初始大小
      /// </summary>
      /// <param name="instObj"></param>
      /// <param name="Parent"></param>
      /// <param name="setPosition"></param>
      /// <param name="Scale"></param>
      /// <returns></returns>
      public GameObject TeaInstantiate(GameObject instObj, Vector3 setPosition, Quaternion setRotation, float Scale = 1, Transform Parent = null)
      {
         GameObject obj = Instantiate(instObj);

         if (Parent)
            obj.transform.SetParent(Parent);
         else
            obj.transform.SetParent(GameCanvas);

         obj.transform.localScale = Vector3.one * Scale;
         obj.transform.position = setPosition;
         obj.transform.rotation = setRotation;

         return obj;
      }
   }
}
