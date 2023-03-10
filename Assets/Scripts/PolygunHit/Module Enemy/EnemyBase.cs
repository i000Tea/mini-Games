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
        private Rigidbody2D rig;

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
        private float healthMax;
		/// <summary>
		/// 当前生命值
		/// </summary>
        private float healthNow;

        [SerializeField]
        GameObject Particle;

        [SerializeField]
        [Tooltip("生命值图标")]
        private Image health;

		#endregion

		#region Buff
		List<BuffBase> eBuffs;
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
        [Tooltip("与玩家之间的距离")]
        float moveDiff;

        [Tooltip("旋转速度")]
        public float rotateSpeed;
        [Tooltip("与玩家之间的旋转角度")]
        float rotateDiff;

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
            // 唤醒时获取刚体
            if (!rig)
                rig = GetComponent<Rigidbody2D>() ?? gameObject.AddComponent<Rigidbody2D>();
			eBuffs = new List<BuffBase>();
		}
        private void OnDestroy()
        {
            RemoveEvent();
        }
        private void FixedUpdate()
        {
			if (healthNow > 0 && isAct) 
			{
				UpdateRotate();
				UpdateMove();
				UpdateAttack();
			}
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out  PlayerBase _))
            {
                if (myState == EnemyState.Atk)
                {
                    UseAtk();
                    myState = EnemyState.CD;
                }
            }
        }
        #endregion

        #region Event 广播/事件

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
        /// 被创造
        /// </summary>
        public void BeCreated(int maxHP)
        {
            gameObject.SetActive(true);
            // 设置生命
            healthNow = healthMax;
            health.fillAmount = 1;
			SetAct(true);

            GetComponent<Collider2D>().enabled = true;
            AddEvent();
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

		#region health 生命值相关
		/// <summary>
		/// 死亡时执行
		/// </summary>
		/// <returns></returns>
		IEnumerator DestMe()
        {
            //关闭碰撞
            GetComponent<Collider2D>().enabled = false;

            yield return 1;

            // 死亡动画:闪烁3次
            var _image = ShowImage;
            var _color = _image.color;

            Color a = ShowImage.color;
            for (int i = 0; i < 3; i++)
            {
                ShowImage.ColorFlicker(Color.red, a);
                yield return new WaitForSeconds(0.3f);
            }

            // 玩家经验增加 分数增加 后续看看能不能整理成广播
            PlayerBase.inst.ExpAdd(1);
            GameManager.inst.ScoreAdd();

            // 实例化粒子 粒子位置为自身 6秒后删除粒子
            var particle = Instantiate(Particle);
            particle.transform.position = transform.position + new Vector3(0, 0, -1);
            Destroy(particle, 6);

            transform.parent.GetComponent<EnemyManager>().EnemyDestory(gameObject);
        }
		#endregion

		#region Buff


		public BuffBase GetBuff(BuffBase newBuff)
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
                    if (moveDiff < minAtkDist)
                        myState = EnemyState.Charge;
                    Charge -= Time.deltaTime * ChargeTime / 1.5f;
                    break;

                case EnemyState.Charge:
                    if (moveDiff > minAtkDist * 1.2f)
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
			healthNow -= atk;
			health.fillAmount = healthNow / healthMax;
			if (healthNow <= 0)
			{
				StartCoroutine(DestMe());
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

			rig.velocity -= _look * unC.Power;
		}

        /// <summary>
        /// 使出攻击
        /// </summary>
        private void UseAtk()
        {
            PlayerBase.inst.UnHit(1);
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

        #region Move and Rotate
        /// <summary>
        /// 移动方法
        /// </summary>
        void UpdateMove()
        {
            // 计算与玩家之间的距离
            moveDiff = Vector3.Distance(transform.position, PlayerBase.Player.position);

			// 速度为 正前方 乘 设置速度
			Vector2 power = transform.up * 0.1f * moveSpeed;

            // 当与玩家间的距离大于视线外 倍速移动靠近玩家
            if (moveDiff > CameraController.inst.MaxEdgeDistance)
                transform.localPosition += new Vector3(power.x, power.y, 0) * Mathf.Pow(moveDiff, 2.4f);

            // 当与玩家的角度差大于15 算法降低移动速度
            if (rotateDiff > 15)
                power *= Mathf.Cos(rotateDiff / 75 * 1.57f);

            if (myState == EnemyState.Charge)
                power *= 0.75f;
            if (myState == EnemyState.Atk)
                power *= 1.6f;
            //将速度应用到刚体上
            rig.velocity += power;
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
            rotateDiff = Mathf.Abs(rotateTarget.eulerAngles.z - transform.localRotation.eulerAngles.z);

            // 定义旋转sudu
            var speed = rotateSpeed * Time.deltaTime;
            //if (rotateDiff > 10)
            //{
            //    speed += rotateDiff;
            //}

            // 当旋转角度差极小时 直接赋值 否则 插值计算
            if (rotateDiff < 0.025f)
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
