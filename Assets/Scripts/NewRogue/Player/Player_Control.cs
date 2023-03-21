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
		public Vector3 PlayerPoint
		{
			get { return transform.position; }
		}

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

		public Enemy_Control targetEnemy;

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

		public int Health
		{
			get { return health; }
			set
			{
				if (value < 0)
					health = 0;
				else
					health = value;
				GUIManager.I.SetHealth(health);
			}
		}
		int health;

		private void Start()
		{
			Keycord = 0;
		}
		private void Update()
		{
			if (EnemyManager.inst)
			{
				if (!targetEnemy)
				{
					targetEnemy = EnemyManager.inst.FindEnemy();
				}
				else if (targetEnemy.health <= 0)
					targetEnemy = EnemyManager.inst.FindEnemy();
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
