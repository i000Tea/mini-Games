using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ECM.Controllers;
using Cinemachine;
namespace Tea.NewRouge
{
	public class Player_Control : MonoBehaviour
	{
		public static Vector3 PlayerPoint
		{
			get { return inst.transform.position; }
		}
		public static Player_Control inst;

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
			get { return (int)(originalKeycord / 1); }
			set
			{
				if (value == 0)
					originalKeycord = 0;
				else
					originalKeycord = value + originalKeycord % 1;

				gui.UpdateKeycord((int)originalKeycord);
			}
		}

		float originalKeycord;
		[SerializeField]
		GUI_Control gui;

		private void Awake()
		{
			inst = this;
		}
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
