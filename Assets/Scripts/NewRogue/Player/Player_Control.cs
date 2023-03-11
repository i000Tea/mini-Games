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

		[SerializeField]
		int minDist = 5, maxDist = 20;
		[SerializeField]
		int minRotate = 30, maxRotate = 45;
		[SerializeField]
		CinemachineVirtualCamera cine;

		public Enemy_Control targetEnemy;

		#region weapon
		[SerializeField]
		private WeaponsManager nowWeapon;
		#endregion

		private void Awake()
		{
			inst = this;
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

		public void UpdateDistance(Slider slider)
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
