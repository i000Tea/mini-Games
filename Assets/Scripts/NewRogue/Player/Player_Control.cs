using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ECM.Controllers;
using Cinemachine;
namespace Tea.NewRouge
{
	public class Player_Control : Singleton<Player_Control>
	{
		#region 变量
		#region Camera
		[Header("Camera")]
		[SerializeField]
		private int minDist = 5;
		[SerializeField]
		private int maxDist = 20;
		[SerializeField]
		private int minRotate = 30;
		[SerializeField]
		private int maxRotate = 45;
		[SerializeField]
		CinemachineVirtualCamera cine;
		#endregion

		#region Anim
		/// <summary>
		/// 交互系统
		/// </summary>
		[Header("Anim")]
		public PlayerAnim_Control animControl;
		#endregion

		/// <summary>
		/// 选择准备攻击的敌人
		/// </summary>
		public Enemy_Control selectEnemy;


		public int Keycord
		{
			get { return (int)(keycord / 1); }
			set
			{
				if (value == 0)
					keycord = 0;
				else
					keycord = value + keycord % 1;

				GUIManager.I.SetKeycord((int)keycord);
			}
		}
		float keycord;

		/// <summary>
		/// 受伤阈值
		/// </summary>
		const float beHitThreshold = 0.6f;
		float nowThreshold;
		[SerializeField]
		private int MaxHealth = 10;
		public int Health
		{
			get { return health; }
			set
			{
				if (value <= 0)
				{
					GameManager.I.Stage_GameOver();
					health = 0;
				}
				else if (value > MaxHealth)
					health = MaxHealth;
				else
					health = value;
				GUIManager.I.SetHealth(health, MaxHealth);
			}
		}
		int health;

		#endregion
		private void OnValidate()
		{
			if (!animControl)
			{
				if (transform.GetChild(0).TryGetComponent(out PlayerAnim_Control anim))
				{
					animControl = anim;
				}
			}
		}
		private void Start()
		{
			Keycord = 0;
			Health = MaxHealth;
		}
		private void Update()
		{
			if (!selectEnemy)
				FindTargetEnemy();
			else if (selectEnemy.health <= 0)
				FindTargetEnemy();
			if (nowThreshold > 0)
				nowThreshold -= Time.deltaTime;
		}

		public void BeHit(int damage)
		{
			if (nowThreshold <= 0)
			{
				Health -= damage;
				nowThreshold = beHitThreshold; 
				PlayerAnim_Control.I.BeHit();
			}
		}

		/// <summary>
		/// 查找目标敌人
		/// </summary>
		public void FindTargetEnemy()
		{
			if (EnemyManager.I)
			{
				selectEnemy = EnemyManager.I.FindEnemy();
				if (selectEnemy)
				{
					StartCoroutine(PlayerAnim_Control.I.TargetLink(selectEnemy.GetUnHitPoint()));
				}
			}
		}
		/// <summary>
		/// 更新相机旋转
		/// </summary>
		/// <param name="slider"></param>
		public void UpdateCameraDistance(Slider slider)
		{
			var a = (CinemachineFramingTransposer)cine.GetCinemachineComponent(CinemachineCore.Stage.Body);
			Debug.Log(a);
			a.m_CameraDistance = slider.value * (maxDist - minDist) + minDist;
			cine.transform.localEulerAngles = new Vector3(
				slider.value * (maxRotate - minRotate) + minRotate,
				cine.transform.localEulerAngles.y,
				0);
		}
	}
}
