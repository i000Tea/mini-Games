using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


namespace Tea.PolygonHit
{
	/// <summary>
	/// 敌人基类
	/// </summary>
	public class EnemyBase : MonoBehaviour
	{
		#region variable 变量

		#region Base 基础组件
		[Header("Base")]

		[SerializeField]
		[Tooltip("自身小球图标")]
		Image ShowImage;

		/// <summary>
		/// 自身刚体
		/// </summary>
		private Rigidbody2D m_Rig => GetComponent<Rigidbody2D>() ?? gameObject.AddComponent<Rigidbody2D>();
		private Collider2D m_Collider => GetComponent<Collider2D>() ?? gameObject.AddComponent<CircleCollider2D>();

		/// <summary>
		/// 正在运行
		/// </summary>
		bool isAct = true;

		EnemyState myState;
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
		private float NowHealth
		{
			get => nowHealth;
			set
			{
				if (value > nowHealthMax)
				{
					nowHealth = nowHealthMax;
				}
				else if (value < 0)
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

		#region ATK 攻击相关
		[Header("Attack")]

		[SerializeField]
		[Tooltip("攻击蓄力图标")]
		private Transform atcImage;

		/// <summary>
		/// 蓄力量
		/// </summary>
		private float Charge
		{
			get
			{
				if (charge < 0)
					return 0;
				else if (charge > 1)
					return 1;
				return charge;
			}
			set
			{
				charge = value;
			}
		}
		[Tooltip("蓄力量")]
		private float charge;


		[Tooltip("蓄力距离")]
		public float minAtkDist = 0.5f;

		[Tooltip("蓄力速度")]
		public float ChargeTime = 1;

		private bool isCD;
		[SerializeField]
		private float atkCD = 2;
		private float nowCD;
		#endregion

		#region Move and Rotate 移动旋转相关

		[Header("Move＆Rotate")]
		[SerializeField]
		/// <summary>
		/// 移动和旋转目标(玩家)
		/// </summary>
		private Vector3 target;
		private Vector3 Target
		{
			get
			{
				return PlayerBase.Player.position;
			}
		}

		[Tooltip("移动速度")]
		public float moveSpeed;
		//[Tooltip("与玩家之间的距离")]
		/// <summary>
		/// 与玩家之间的距离
		/// </summary>
		private float DistanceFromPlayer => Vector3.Distance(transform.position, PlayerBase.I.Point);

		/// <summary>
		/// 带有方向的基础速度
		/// </summary>
		private Vector2 BaseDirSpeed => 0.1f * moveSpeed * transform.up;
		/// <summary>
		/// 带有方向的输出速度
		/// </summary>
		private Vector2 OutPutDirSpeed
		{
			get
			{
				var power = BaseDirSpeed;
				// 当与玩家的角度差大于15 算法降低移动速度
				if (DiffOfAngle > 15)
					power *= Mathf.Cos(DiffOfAngle / 75 * 1.57f);

				if (myState == EnemyState.Charge)
					power *= 0.75f;
				if (myState == EnemyState.Atk)
					power *= 1.6f;
				return power;
			}
		}


		[Tooltip("旋转速度")]
		public float rotateSpeed;
		[Tooltip("与玩家之间的旋转角度")]
		float DiffOfAngle;

		/// <summary>
		/// 旋转参数
		/// </summary>
		Vector3 dir;
		/// <summary>
		/// 旋转目标Z值
		/// </summary>
		float angle;
		/// <summary>
		/// 旋转目标
		/// </summary>
		Quaternion rotateTarget;
		#endregion

		#endregion

		#region unity void
		private void Awake()
		{
			eBuffs = new List<IBuff>();
		}
		private void OnDestroy()
		{
			RemoveEvent();
		}
		private void FixedUpdate()
		{
			if (NowHealth > 0 && isAct)
			{
				UpdateRotate();
				UpdateMove();
				UpdateAttack();
			}
		}
		private void OnCollisionEnter2D(Collision2D collision)
		{
			if (collision.gameObject.TryGetComponent(out PlayerBase _))
			{
				if (myState == EnemyState.Atk)
				{
					UseAtk();
					myState = EnemyState.CD;
				}
			}
		}
		#endregion

		#region Event		广播/事件

		void AddEvent()
		{
			EventController.AddListener(EventType.PlayerDestory, Stop);
		}
		void RemoveEvent()
		{
			EventController.RemoveListener(EventType.PlayerDestory, Stop);
		}
		/// <summary>
		/// 玩家死亡时 敌人暂停
		/// </summary>
		/// <param name=""></param>
		public void Stop()
		{
			enabled = false;
		}

		/// <summary>
		/// 被击杀
		/// </summary>
		public void BeDestroy()
		{
			GetComponent<Collider2D>().enabled = false;
			RemoveEvent();
		}

		#endregion

		#region Anther		其他的
		/// <summary>
		/// 初始化	
		/// </summary>
		public void Initialize(float setHealthMulti = 1)
		{
			// 开启自身
			gameObject.SetActive(true);
			// 设置生命上限
			nowHealthMax = baseHealthMax * setHealthMulti;
			NowHealth = nowHealthMax;
			SetAct(true);
			m_Collider.enabled = true;
			AddEvent();
		}
		/// <summary>
		/// 死亡事件
		/// </summary>
		/// <returns></returns>
		private IEnumerator IsDeath()
		{
			//关闭碰撞
			m_Collider.enabled = false;
			yield return 1;

			// 死亡动画:闪烁3次
			var _image = ShowImage;
			var _color = _image.color;
			Color a = ShowImage.color;
			for (int i = 0; i < 3; i++)
			{
				ShowImage.ColorFlicker(_color, a);
				yield return new WaitForSeconds(0.3f);
			}

			EventControl.InvokeSomething(ActionType.ExpAdd, 1);
			// 玩家经验增加 分数增加 后续看看能不能整理成广播
			PlayerBase.I.ExpAdd(1);
			GameManager.I.ScoreAdd();

			// 实例化粒子 粒子位置为自身 6秒后删除粒子
			var particle = Instantiate(Particle);
			particle.transform.position = transform.position + new Vector3(0, 0, -1);
			Destroy(particle, 6);

			transform.parent.GetComponent<EnemyManager>().EnemyDestory(gameObject);
		}

		#endregion

		#region health 生命值相关
		#endregion

		#region Buff


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

		#region Atk UnAtk 攻击与被攻击相关

		/// <summary>
		/// 攻击的每帧动作
		/// </summary>
		private void UpdateAttack()
		{
			//Debug.Log(myState);
			switch (myState)
			{
				case EnemyState.Default:
					if (DistanceFromPlayer < minAtkDist)
						myState = EnemyState.Charge;
					Charge -= Time.deltaTime * ChargeTime / 1.5f;
					break;

				case EnemyState.Charge:
					if (DistanceFromPlayer > minAtkDist * 1.2f)
						myState = EnemyState.Default;

					Charge += Time.deltaTime * ChargeTime;

					if (Charge >= 1)
						myState = EnemyState.Atk;
					break;

				case EnemyState.Atk:
					Charge -= Time.deltaTime * ChargeTime / 2f;

					if (Charge <= 0)
						myState = EnemyState.CD;

					break;

				case EnemyState.CD:
					Charge -= Time.deltaTime * ChargeTime * 10;
					nowCD += Time.deltaTime;
					if (nowCD >= atkCD)
					{
						myState = EnemyState.Default;
						nowCD = 0;
					}
					break;

				default:
					break;
			}
			atcImage.localScale = Vector3.one * Charge;
		}

		/// <summary>
		/// 受到攻击 执行的事件
		/// </summary>
		/// <param name="atk">受到伤害</param>
		/// <returns>是否死亡</returns>
		public bool UnHit(float atk)
		{
			NowHealth -= atk;
			if (nowHealth <= 0)
			{
				return true;
			}

			//StartCoroutine(WaitForMoveAgain());
			return false;
		}

		/// <summary>
		/// 受到额外撞击
		/// </summary>
		/// <param name="Power"></param>
		/// <param name="collPoint"></param>
		/// <param name="dizzinessTime">撞击后的冷却</param>
		public void UnCollision(UnCollision unC)
		{
			// 计算的是从本身到目标点的方向
			Vector2 _look = (unC.Target - transform.position).normalized;

			m_Rig.velocity -= _look * unC.Power;
		}

		/// <summary>
		/// 使出攻击
		/// </summary>
		private void UseAtk()
		{
			PlayerBase.I.UnHit(1);
		}

		/// <summary>
		/// 攻击模式
		/// </summary>
		enum AttackMode
		{
			speedUp,
			/// <summary>
			/// 蓄力发射
			/// </summary>
			Charge,

		}
		#endregion

		#region Movement
		/// <summary>
		/// 移动方法
		/// </summary>
		void UpdateMove()
		{
			// 计算与玩家之间的距离
			//DistanceFromPlayer = Vector3.Distance(transform.position, PlayerBase.Player.position);


			// 当与玩家间的距离大于视线外 倍速移动靠近玩家
			if (DistanceFromPlayer > CameraController.inst.MaxEdgeDistance)
			{
				transform.localPosition += (Vector3)BaseDirSpeed * Mathf.Pow(DistanceFromPlayer, 2.4f);
			}

			//将速度应用到刚体上
			m_Rig.velocity += OutPutDirSpeed;
		}
		/// <summary>
		/// 旋转方法
		/// </summary>
		void UpdateRotate()
		{
			// 计算目标与自身的向量差
			dir = Target - transform.position;
			// 向量Z归零
			dir.z = 0;
			// 返回有符号的角度
			angle = Vector3.SignedAngle(Vector3.up, dir, Vector3.forward);
			// 角度转四元数
			rotateTarget = Quaternion.Euler(0, 0, angle);

			// 设置 旋转角度差
			DiffOfAngle = Mathf.Abs(rotateTarget.eulerAngles.z - transform.localRotation.eulerAngles.z);

			// 定义旋转速度
			var speed = rotateSpeed * Time.deltaTime;

			//if (rotateDiff > 10)
			//{
			//    speed += rotateDiff;
			//}

			// 当旋转角度差极小时 直接赋值 否则 插值计算
			if (DiffOfAngle < 0.025f)
				transform.localRotation = rotateTarget;
			else
				transform.localRotation = Quaternion.Slerp(transform.localRotation, rotateTarget, speed);
		}
		/// <summary>
		/// 获取玩家位置
		/// </summary>
		/// <returns></returns>
		IEnumerator GetPlayer()
		{
			target = PlayerBase.Player.position;

			yield return new WaitForSeconds(Random.Range(3, 5));
			StartCoroutine(GetPlayer());
		}

		public void SetAct(bool act)
		{
			isAct = act;
			if (act)
				gameObject.layer = 7;
			else
				gameObject.layer = 8;
		}
		#endregion
	}
}
