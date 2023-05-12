using RootMotion.FinalIK;
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
		#region variable	变量
		public static Transform Player;
		public Vector3 Point => transform.localPosition;

		[SerializeField]
		private Image m_ShowImage;

		[SerializeField]
		private Rigidbody2D m_Rig => GetComponent<Rigidbody2D>();

		private List<IBuff> m_playerBuffs;

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
				//if (nowLevel == 1) return 3;
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
				nowExp = value;
				if(value>= MaxExp)
				{
					value -= MaxExp;
					LevelUp();
				}
				nowExp = value;
			}
		}
		public int nowExp;

		/// <summary>
		/// 生命值上限
		/// </summary>
		public int HealthMax = 3;
		private int Health;

		/// <summary>
		/// 撞击伤害
		/// </summary>
		public float m_HitDmg = 3;
		#endregion

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

		#region unity void	基础方法
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
			// 更新血量
			Health = HealthMax;
			// 设置玩家位置
			Player = transform;

			m_playerBuffs = new List<IBuff>();
		}
		protected virtual void Start()
		{

			GUIManager.I.PlayerMessageUpdate(nowLevel, nowExp, MaxExp, Health);
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

		#region Event
		protected override void AddDelegate()
		{
			EventControl.OnAddAntherList<ActionType, int>(ActionType.ExpAdd, ExpAdd);
		}
		protected override void Removedelegate()
		{
			EventControl.OnRemoveAhtnerList<ActionType, int>(ActionType.ExpAdd, ExpAdd);
		}
		#endregion

		#region Movement	移动
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
		}
		#endregion

		#region Hitler&UnHit	攻击与被攻击
		/// <summary>
		/// 撞击敌人
		/// </summary>
		public virtual void IsStrike(Collision2D collision)
		{
			// 当撞击时 速度大于1 则判定为撞击成功
			if (m_Rig.velocity.magnitude > 1f)
			{
				// 放出撞击事件
				EventController.Broadcast(EventType.action_Strike,
					collision.gameObject.GetComponent<EnemyBase>());

				// 新建伤害值
				float dmg = m_HitDmg;

				//遍历buff列表 更新伤害值
				for (int i = 0; i < m_playerBuffs.Count; i++)
				{
					m_playerBuffs[i].IsStrike(ref dmg);
				}

				int particlePower = 0;
				if (dmg > m_HitDmg)
					particlePower = 1;
				ParticleManager.InstStrikeParticle(particlePower,
					(Vector3)
					collision.collider.ClosestPoint(transform.position) +
					new Vector3(0, 0, 10));
				// 对撞击到的地人打出伤害
				collision.transform.GetComponent<EnemyBase>().UnHit(dmg);
			}
		}

		/// <summary>
		/// 受到伤害
		/// </summary>
		public void UnHit(int atk)
		{
			if (m_Rig.velocity.magnitude < 1.5f)
			{
				Health -= atk;

				if (Health <= 0)
				{
					Health = 0;
					StartCoroutine(IsDestroy());
				}
				else
					EventController.Broadcast(EventType.action_UnStrike);

				GUIManager.I.PlayerMessageUpdate(nowLevel, nowExp, MaxExp, Health);
			}
		}
		private IEnumerator IsDestroy()
		{
			// 放出死亡事件
			EventController.Broadcast(EventType.PlayerDestory);

			// 循环闪烁4次
			for (int i = 0; i < 4; i++)
			{
				Color main = m_ShowImage.color;
				m_ShowImage.ColorFlicker(Color.black, main, 0.25f);
				yield return new WaitForSeconds(0.25f);
			}
			// 关闭自身图片显示
			m_ShowImage.enabled = false;

			// 生成一个死亡粒子
			ParticleManager.InstParticle(destroyParticle, transform.position + new Vector3(0, 0, -1));

			yield return new WaitForSeconds(1);
			// 等待1秒后
			GUIManager.I.CanvasSwitch(GameState.Over);
		}
		#endregion

		#region Level		等级相关

		/// <summary>
		/// 经验增加
		/// </summary>
		/// <param name="exp"></param>
		public void ExpAdd(int exp)
		{
			nowExp += exp;
			GUIManager.I.PlayerMessageUpdate(nowLevel, nowExp, MaxExp, Health);
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
			GUIManager.I.PlayerMessageUpdate(nowLevel, nowExp, MaxExp, Health);
		}

		#endregion

		#region Buff		增益
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
