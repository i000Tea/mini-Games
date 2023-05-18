using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tea.PolygonHit
{
   /// <summary>
   /// 敌人管理器
   /// </summary>
   public class EnemyManager : Singleton<EnemyManager>
   {
      #region 变量

      #region Create 生成敌人
      /// <summary>
      /// 用于生成的预制件
      /// </summary>
      public GameObject prefab;
      /// <summary>
      /// 敌人生成开关
      /// </summary>
      private bool isCreate = false;
      /// <summary>
      /// 最大敌人数量
      /// </summary>
      public int EnemyMaxLimit = 30;
      /// <summary>
      /// 每次生成的数量
      /// </summary>
      private int BaseCreateNum => Random.Range(1, 3);
      private int BaseHPMulti = 1;

      /// <summary>
      /// 生成是否就绪
      /// </summary>
      public bool CreateReady => createCD <= 0;
      /// <summary>
      /// 生成倒计时
      /// </summary>
      public float CreateCD
      {
         get => createCD;
         set
         {
            // 数值小于0 纠正为0
            if (value < 0)
            {
               createCD = 0;
            }
            // 数值大于上限 纠正为上限
            else if (value > cdUpperLimit)
            {
               createCD = cdUpperLimit;
            }
            else
            {
               createCD = value;
            }
         }
      }
      private float createCD;
      /// <summary>
      /// CD上限
      /// </summary>
      public float cdUpperLimit;

      /// <summary>
      ///场景中的敌人的列表
      /// </summary>
      public static List<GameObject> nowEnemys;

      /// <summary>
      /// 敌人的对象池
      /// </summary>
      private List<GameObject> dieEnemyPool;
      #endregion

      #endregion

      #region unity void

      /// <summary>
      /// 初始化
      /// </summary>
      protected override void Awake()
      {
         base.Awake();
         nowEnemys = new List<GameObject>();
         dieEnemyPool = new List<GameObject>();
      }

      private void Update()
      {
         if (isCreate)
         {
            CreateUpdate();
         }
      }
      #endregion

      #region Event
      protected override void AddDelegate()
      {
         EventControl.OnAddButtonList(ButtonType.Menu_StartGame, GameStart);
         EventControl.OnAddGameStateList(SetTheState);

         EventControl.OnAddAntherList(ActionType.PlayerDestory, StopCreate);
         EventControl.OnAddAntherList(ActionType.LevelUp, PlayerLevelUp);
      }
      protected override void RemoveDelegate()
      {
         EventControl.OnRemoveButtonList(ButtonType.Menu_StartGame, GameStart);
         EventControl.OnRemoveGameStateList(SetTheState);

         EventControl.OnRemoveAhtnerList(ActionType.PlayerDestory, StopCreate);
         EventControl.OnRemoveAhtnerList(ActionType.LevelUp, PlayerLevelUp);
      }



      #endregion

      #region progress 进度
      /// <summary>
      /// 游戏开始 
      /// </summary>
      private void GameStart()
      {
         isCreate = true;
      }
      private void SetTheState(GameState state)
      {
         if (state == GameState.Gameing)
         {
            isCreate = true;
         }
         else
         {
            isCreate = false;
         }

         switch (state)
         {
            case GameState.Gameing:
               break;
            case GameState.LevelUp:
               break;
            case GameState.Pause:
               isCreate = false;
               break;
            case GameState.Over:
               StopCreate();
               break;
            default:
               break;
         }
      }
      private void PlayerLevelUp()
      {
      }
      #endregion

      #region Create 生成

      /// <summary>
      /// CD倒计时 检测能否生成
      /// </summary>
      private void CreateUpdate()
      {
         // 可以运行的时候 CD每帧减少
         CreateCD -= Time.deltaTime;
         //若CD未就绪 返回
         if (CreateCD > 0)
         {
            return;
         }
         // 可以生成 将CD设为0
         CreateCD = cdUpperLimit;
         for (int i = 0; i < BaseCreateNum; i++)
         {
            if (!TryCreateEnemy())
            {
               return;
            }
         }
      }

      /// <summary>
      /// 尝试生成敌人
      /// </summary>
      private bool TryCreateEnemy()
      {
         // 若敌人大于上限 返回否
         if (nowEnemys.Count >= EnemyMaxLimit)
         {
            return false;
         }

         // 新建对象
         GameObject newEnemy;

         // 尝试从对象池中生成一个敌人
         if (dieEnemyPool.Count > 0)
         {
            newEnemy = dieEnemyPool[0];
            dieEnemyPool.RemoveAt(0);
         }
         // 否则 创建一个敌人
         else
         {
            newEnemy = prefab.CreateObjInCanvas(transform as RectTransform);
         }
         //newEnemy = Instantiate(prefab);
         newEnemy.transform.position = RandomPoint();

         newEnemy.GetComponent<EnemyBase>().Initialize();

         nowEnemys.Add(newEnemy);
         return true;
      }

      /// <summary>
      /// 停止生成
      /// </summary>
      public void StopCreate()
      {
         isCreate = false;
         for (int i = 0; i < transform.childCount; i++)
            if (transform.GetChild(i).TryGetComponent(out EnemyBase enemy)) enemy.enabled = false;
      }

      /// <summary>
      /// 随机方向
      ///       0 
      ///       
      ///   1       2
      ///     
      ///       3
      /// </summary>
      /// <returns></returns>
      Vector3 RandomPoint()
      {
         int a = Random.Range(0, 4);
         float pointX = 0, pointY = 0;
         switch (a)
         {
            case 0:
               pointX = Random.Range(CameraController.I.MaxLeft(), CameraController.I.MaxRight());
               pointY = CameraController.I.MaxUp() + 0.2f;
               break;
            case 1:
               pointY = Random.Range(CameraController.I.MaxUp(), CameraController.I.MaxDown());
               pointX = CameraController.I.MaxLeft() - 0.2f;
               break;
            case 2:
               pointY = Random.Range(CameraController.I.MaxUp(), CameraController.I.MaxDown());
               pointX = CameraController.I.MaxRight() + 0.2f;
               break;
            case 3:
               pointX = Random.Range(CameraController.I.MaxLeft(), CameraController.I.MaxRight());
               pointY = CameraController.I.MaxDown() - 0.2f;
               break;
            default:
               break;
         }
         return new Vector3(pointX, pointY, 10);
      }
      #endregion

      #region Death 死亡

      /// <summary>
      /// 敌人被销毁
      /// </summary>
      public void EnemyDestory(GameObject target, GameObject particle = null)
      {
         // 将敌人从列表中移除
         nowEnemys.Remove(target);
         // 添加到对象池列表
         dieEnemyPool.Add(target);

         target.SetActive(false);
         target.GetComponent<EnemyBase>().BeDestroy();
         // 把死亡的敌人挪到第一个 方便开发时检查(后续可删除)
         target.transform.SetAsFirstSibling();
      }

      #endregion

   }
}
