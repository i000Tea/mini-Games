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
		private CinemachineVirtualCamera MainCine;
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

		public void CameraDistance(Slider slider)
		{
			//MainCine.
			//slider.value
		}
	}
}
