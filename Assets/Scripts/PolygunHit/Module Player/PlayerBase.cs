using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tea.PolygonHit
{
   /// <summary>
   /// 玩家基类
   /// </summary>
   public class PlayerBase : Singleton<PlayerBase>
   {
      #region variable     变量

      #region 基础组件
      public Vector3 Point => transform.position;
      public Vector3 localPoint => transform.localPosition;

      [SerializeField]
      private Image m_ShowImage;
      Color baseColor;

      [SerializeField]
      private Rigidbody2D m_Rig => GetComponent<Rigidbody2D>();

      /// <summary>
      /// 玩家身上挂载的buff列表
      /// </summary>
      private List<IBuff> m_playerBuffs;

      #endregion

      #region PlayerData 玩家信息
      /// <summary>
      /// 当前等级
      /// </summary>
      [SerializeField]
      [Range(1, 50)]
      private int nowLevel = 1;

      /// <summary>
      /// 算法获得经验值上限
      /// </summary>
      private int MaxExp
      {
         get
         {
            if (nowLevel == 1) return 3;
            return (int)(Mathf.Pow(nowLevel, 3) / 6 + 15 * nowLevel)
                // 测试模式 经验减少
                / 2;
         }
      }

      /// <summary>
      /// 当前经验
      /// </summary>
      public int NowExp
      {
         get => nowExp;
         set
         {
            if (value >= MaxExp)
            {
               nowExp = value - MaxExp;
               LevelUp();
            }
            else
            {
               nowExp = value;
            }

            GUIManager.I.ExpUpdate(nowExp, MaxExp);
         }
      }
      public int nowExp;

      private int Health
      {
         get => nowHP;
         set
         {
            if (value <= 0)
            {
               value = 0;
               StartCoroutine(IsDestroy());
            }
            nowHP = value;
            GUIManager.I.HealthUpdate(health: nowHP);
         }
      }
      private int nowHP;
      /// <summary>
      /// 生命值上限
      /// </summary>
      public int HealthMax = 3;

      /// <summary>
      /// 撞击伤害
      /// </summary>
      public float m_HitDmg = 3;
      #endregion

      #region Fight 战斗相关
      /// <summary>
      /// 受到伤害时的动画
      /// </summary>
      private Tween unAtkColorTween;
      public bool IsInjury => !protectAfterInjury && !protectFromShoot;

      /// <summary>
      /// 弹射保护
      /// </summary>
      private bool protectFromShoot;
      Coroutine protectShoot;

      /// <summary>
      /// 受伤后保护
      /// </summary>
      private bool protectAfterInjury;
      /// <summary>
      /// 无敌时间
      /// </summary>
      [SerializeField]
      private float protectInjuryTime = 1.5f;
      #endregion

      #region Anther
      /// <summary>
      /// 死亡时播放的粒子
      /// </summary>
      public GameObject destroyParticle;

      private Vector2 nowMoveForworld;
      private float NowMovePower
      {
         get => nmp;
         set
         {
            if (value > 1)
            {
               nmp = 1;
            }
            else
            {
               nmp = value;
            }
         }
      }
      private float nmp;
      #endregion

      #endregion

      #region unity void   基础方法
      private void OnValidate()
      {
         // 获取图片
         if (!m_ShowImage)
            m_ShowImage = transform.GetChild(0).GetComponent<Image>();
         // 获取刚体
      }

      protected override void Awake()
      {
         base.Awake();
         m_playerBuffs = new List<IBuff>();
         baseColor = m_ShowImage.color;
      }
      protected virtual void Start()
      {
         // 更新血量
         Health = HealthMax;
         GUIManager.I.GUIUpdate(nowLevel, 0, MaxExp, Health);
      }

      private void FixedUpdate()
      {
         UpdateMovement();
      }
      /// <summary>
      /// 碰撞时
      /// </summary>
      /// <param name="collision"></param>
      private void OnCollisionEnter2D(Collision2D collision)
      {
         // 撞击的是敌人
         if (collision.gameObject.tag == "Enemy")
            IsStrike(collision);
      }

      #endregion

      #region Event        事件
      protected override void AddDelegate()
      {
         EventControl.OnAddAntherList<ActionType, int>(ActionType.ExpAdd, ExpAdd);
      }
      protected override void RemoveDelegate()
      {
         EventControl.OnRemoveAhtnerList<ActionType, int>(ActionType.ExpAdd, ExpAdd);
      }
      #endregion

      #region Movement     移动
      /// <summary>
      /// 每帧移动
      /// </summary>
      private void UpdateMovement()
      {
         if (nowMoveForworld != default)
         {
            NowMovePower += Time.deltaTime;
            (transform as RectTransform).anchoredPosition += nowMoveForworld * NowMovePower * 10;
         }

      }
      /// <summary>
      /// 修正移动方向
      /// </summary>
      public void FixMovement(Vector2 moveForworld)
      {
         nowMoveForworld = moveForworld;
      }

      /// <summary>
      /// 发射移动 
      /// </summary>
      public void ShootMovement(Vector2 moveForworld, float powerBaseScale)
      {
         nowMoveForworld = default;
         NowMovePower = 0;

         m_Rig.velocity += moveForworld * powerBaseScale;

         if (protectShoot != null)
         {
            StopCoroutine(protectShoot);
         }
         protectShoot = StartCoroutine(ShootProtect());
      }
      #endregion

      #region Fight        攻击与被攻击

      /// <summary>
      /// 撞击敌人
      /// </summary>
      public virtual void IsStrike(Collision2D collision)
      {
         // 当撞击时 速度大于1 则判定为撞击成功
         if (m_Rig.velocity.magnitude > 1f && collision.gameObject.TryGetComponent(out EnemyBase enemyBase))
         {
            // 放出撞击事件
            EventControl.InvokeSomething(ActionType.Strike, enemyBase);

            // 新建伤害值
            float dmg = m_HitDmg;

            //遍历buff列表 更新伤害值
            IBuff.AlterDamage?.Invoke(ref dmg,ValueAlterEffectPhase.BaseAdd);
            IBuff.AlterDamage?.Invoke(ref dmg,ValueAlterEffectPhase.StackMulti);
            IBuff.AlterDamage?.Invoke(ref dmg,ValueAlterEffectPhase.FinalAdd);

            int particlePower = 0;
            if (dmg > m_HitDmg)
            {
               particlePower = 1;
            }

            ParticleManager.InstStrikeParticle(particlePower,
                (Vector3)collision.collider.ClosestPoint(transform.position) +
                new Vector3(0, 0, 10));

            // 对撞击到的地人打出伤害
            enemyBase.UnAtk(dmg);
         }
      }

      /// <summary>
      /// 受到攻击
      /// </summary>
      /// <param name="atk"></param>
      /// <returns>是否成功击中</returns>
      public bool Injury(int atk)
      {
         //Debug.Log($"是否可以攻击{IsInjury} 受伤防护{protectAfterInjury} 弹射防护{protectFromShoot}");
         if (!IsInjury)
         {
            return false;
         }
         else
         {
            var _ = StartCoroutine(InjuryProtect());
            Health -= atk;
            return true;
         }
      }
      /// <summary>
      /// 受到攻击后防护
      /// </summary>
      /// <returns></returns>
      private IEnumerator InjuryProtect()
      {
         // 开启无敌
         protectAfterInjury = true;
         // 循环无敌帧动画
         unAtkColorTween = m_ShowImage.DOColor(Color.black, 0.15f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Flash);
         // 等待无敌时间
         yield return new WaitForSeconds(protectInjuryTime - 0.2f);
         // 杀死动画
         unAtkColorTween.Kill();
         // 返回原色
         m_ShowImage.DOColor(baseColor, 0.2f);
         yield return new WaitForSeconds(0.4f);
         // 结束无敌
         protectAfterInjury = false;
      }
      /// <summary>
      /// 弹射防护
      /// </summary>
      private IEnumerator ShootProtect()
      {
         protectFromShoot = true;
         while (true)
         {
            yield return new WaitUntil(() => m_Rig.velocity.magnitude < 1f);
            yield return new WaitForSeconds(0.1f);
            if (m_Rig.velocity.magnitude < 1f)
            {
               break;
            }
            else
            {
               continue;
            }
         }
         protectFromShoot = false;
      }
      /// <summary>
      /// 死亡
      /// </summary>
      /// <returns></returns>
      private IEnumerator IsDestroy()
      {
         // 放出死亡事件
         EventControl.InvokeSomething(ActionType.PlayerDestory);

         // 循环无敌帧动画
         var tweenAnim = m_ShowImage.DOColor(Color.black, 0.15f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Flash);
         yield return new WaitForSeconds(1);
         tweenAnim.Kill();

         // 关闭自身图片显示
         m_ShowImage.enabled = false;

         // 生成一个死亡粒子
         ParticleManager.InstParticle(destroyParticle, transform.position + new Vector3(0, 0, -1));

         // 等待1秒后
         yield return new WaitForSeconds(1);
         // 执行游戏结束事件
         EventControl.SetGameState(GameState.Over);
      }
      #endregion

      #region Level        等级相关

      /// <summary>
      /// 经验增加
      /// </summary>
      /// <param name="exp"></param>
      private void ExpAdd(int exp)
      {
         NowExp += exp;
      }
      /// <summary>
      /// 升级
      /// </summary>
      private void LevelUp()
      {
         // 等级数字提升
         nowLevel++;
         // 升级动作
         EventControl.InvokeSomething(ActionType.LevelUp);
         // 升级状态
         EventControl.SetGameState(GameState.LevelUp);
         // 等级UI更新
         GUIManager.I.LevelUpdate(nowLevel);
      }

      #endregion

      #region Buff         增益
      /// <summary>
      /// 传入一个新的buff 若列表中存在相同类 则返回列表中的 否则 添加到列表中
      /// </summary>
      /// <param name="newBuff"></param>
      /// <returns></returns>
      public IBuff AddBuff(IBuff newBuff)
      {
         for (int i = 0; i < m_playerBuffs.Count; i++)
         {
            //Debug.Log(m_playerBuffs[i].ToString() + "  " + newBuff.ToString());
            if (m_playerBuffs[i].ToString() == newBuff.ToString())
            {
               Debug.Log("相同");
               return m_playerBuffs[i];
            }
         }

         m_playerBuffs.Add(newBuff);
         return newBuff;
      }
      #endregion

      #region Special systems

      int m_MaxAmmo;
      public int m_Ammo;
      public void UseAmmo(int useNum)
      {
         // 当余量 大等于 需求 成功使用
         if (m_Ammo >= useNum)
         {
            m_Ammo -= useNum;
            GUISpecialSystems.inst.Ammo(m_Ammo, m_MaxAmmo);
         }
      }
      public void AddAmmo(int num)
      {
         m_Ammo += num;
         if (m_Ammo > m_MaxAmmo)
            m_Ammo = m_MaxAmmo;
         GUISpecialSystems.inst.Ammo(m_Ammo, m_MaxAmmo);
      }
      public void AmmoMax(int max)
      {
         m_MaxAmmo += max;
      }

      #endregion
   }
}
