using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

		public Enemy_Control targetEnemy;

		#region weapon
		[SerializeField]
		private PlayerWeapon nowWeapon;
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
	}
}
