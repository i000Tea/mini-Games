using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.Events;

namespace Tea.PolygonHit
{
   public delegate void BaseEvent();
   public delegate void SwitchAtkMode(EnemyAttackMode atk);
   /// <summary>
   /// 敌人基类
   /// </summary>
   public class EnemyBase : MonoBehaviour
   {
      #region variable     变量

      #region Base 基础组件
      [SerializeField]
      private Image ShowImage;

      private Collider2D m_Collider => GetComponent<Collider2D>() ?? gameObject.AddComponent<CircleCollider2D>();

      private Collider2D[] m_Colliders
      {
         get
         {
            if (m_colls == null)
            {
               m_colls = GetComponentsInChildren<Collider2D>();
            }
            return m_colls;
         }
      }
      private Collider2D[] m_colls;

      public EnemyState State => myState;
      [SerializeField]
      private EnemyState myState;

      /// <summary>
      /// 目标
      /// </summary>
      public Transform TargetTransform => m_TargetTransform;
      private Transform m_TargetTransform;
      public PlayerBase TargetPlayer => m_TargetPlayer;
      private PlayerBase m_TargetPlayer;

      public bool Movement
      {
         get => move;
         set => move = value;
      }
      private bool move = true;

      #endregion

      #region Health 生命值相关
      [Header("Health")]
      [SerializeField]
      [Tooltip("最大生命值")]
      private float baseHealthMax = 10;
      private float nowHealthMax;
      /// <summary>
      /// 当前生命值
      /// </summary>
      public float NowHealth
      {
         get => nowHealth;
         private set
         {
            if (value > nowHealthMax)
            {
               nowHealth = nowHealthMax;
            }
            else if (value <= 0)
            {
               nowHealth = 0;
               StartCoroutine(IsDeath());
            }
            else
            {
               nowHealth = value;
            }
            healthImage.fillAmount = nowHealth / nowHealthMax;
         }
      }
      private float nowHealth;

      [SerializeField]
      GameObject Particle;

      /// <summary>
      /// 图片
      /// </summary>
      [SerializeField]
      [Tooltip("生命值图标")]
      private Image healthImage;

      #endregion

      #region Buff
      List<IBuff> eBuffs;
      #endregion

      #region Anther
      public EnemyAttackMode AtkMode { get; private set; }

      public BaseEvent ModeInitialize;
      /// <summary>
      /// 模组结束
      /// </summary>
      public BaseEvent ModeDestroy;
      #endregion

      public SwitchAtkMode switchMode;
      #endregion

      #region unity void   原生方法
      private void Awake()
      {
         eBuffs = new List<IBuff>();
      }
      private void OnDestroy()
      {
         RemoveEvent();
      }
      #endregion

      #region Event        广播/事件

      void AddEvent()
      {
         EventControl.OnAddAntherList(ActionType.PlayerDestory, Stop);
      }
      void RemoveEvent()
      {
         EventControl.OnRemoveAhtnerList(ActionType.PlayerDestory, Stop);
      }
      /// <summary>
      /// 玩家死亡时 敌人暂停
      /// </summary>
      /// <param name=""></param>
      private void Stop()
      {
         //关闭碰撞
         for (int i = 0; i < m_Colliders.Length; i++)
         {
            m_Colliders[i].enabled = false;
         }
         Movement = false;
         //Debug.Log("玩家死亡");
         ModeDestroy?.Invoke();
      }
      #endregion

      #region LifeCycle    生命周期
      /// <summary>
      /// 初始化	
      /// </summary>
      public void Initialize(float setHealthMulti = 1, EnemyAttackMode newMode = EnemyAttackMode.Melee)
      {
         // 开启自身
         gameObject.SetActive(true);
         // 设置生命上限
         nowHealthMax = baseHealthMax * setHealthMulti;
         NowHealth = nowHealthMax;
         // 开启碰撞
         for (int i = 0; i < m_Colliders.Length; i++)
         {
            m_Colliders[i].enabled = true;
         }
         // 开启移动
         Movement = true;
         AddEvent();
         try
         {
            ModeInitialize?.Invoke();
         }
         catch (Exception)
         {

            throw;
         }
         if (!m_TargetTransform)
         {
            GetTarget();
         }
         AtkMode = newMode;
      }

      /// <summary>
      /// 死亡事件
      /// </summary>
      /// <returns></returns>
      private IEnumerator IsDeath()
      {
         //关闭碰撞
         for (int i = 0; i < m_Colliders.Length; i++)
         {
            m_Colliders[i].enabled = false;
         }
         yield return new WaitForFixedUpdate();

         // 死亡动画

         // 玩家经验增加 分数增加
         EventControl.InvokeSomething(ActionType.ExpAdd, 1);
         Game1Manager.I.ScoreAdd();

         // 实例化粒子 粒子位置为自身 6秒后删除粒子
         var particle = Instantiate(Particle);
         particle.transform.position = transform.position + new Vector3(0, 0, -1);
         Destroy(particle, 6);

         transform.parent.GetComponent<EnemyManager>().EnemyDestory(gameObject);
      }
      /// <summary>
      /// 被击杀销毁
      /// </summary>
      public void BeDestroy()
      {
         try
         {
            ModeDestroy?.Invoke();
         }
         catch (Exception)
         {

            throw;
         }
         GetComponent<Collider2D>().enabled = false;
         RemoveEvent();
      }
      #endregion

      #region health       生命值相关
      public float UnAtk(float DMG)
      {
         NowHealth -= DMG;
         return DMG;
      }
      #endregion

      #region Buff         增益减益
      public IBuff GetBuff(IBuff newBuff)
      {
         for (int i = 0; i < eBuffs.Count; i++)
         {
            if (eBuffs[i].ToString() == newBuff.ToString())
            {
               return eBuffs[i];
            }
         }
         eBuffs.Add(newBuff);
         return newBuff;
      }

      #endregion

      #region Anther 其他不好分类的内容

      /// <summary>
      /// 获取用于战斗或移动的目标
      /// </summary>
      private void GetTarget()
      {
         m_TargetPlayer = PlayerBase.I;
         m_TargetTransform = PlayerBase.I.transform;
      }
      #endregion
   }
}
