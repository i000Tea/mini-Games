using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tea.NewRouge
{
	public class PlayerWeapon : MonoBehaviour
	{
		[SerializeField]
		Player_Control control;

		public Transform weapon;
		public GameObject bullet;
		[SerializeField]
		float bulletCD = 0.5f;
		float bulletCDNow;

		private void Update()
		{
			
		}
		#region Weapon
		void WeaponUpdate()
		{
			if (control.targetEnemy)
			{
				transform.LookAt(new Vector3()
				{
					x = control.targetEnemy.transform.position.x,
					y = transform.position.y,
					z = control.targetEnemy.transform.position.z
				});
				if (bulletCDNow <= 0)
				{
					bulletCDNow = bulletCD;
					IsShoot();
				}
				else
				{
					bulletCDNow -= Time.deltaTime;
				}
			}
		}
		/// <summary>
		/// 射击
		/// </summary>
		/// <returns></returns>
		public bool IsShoot()
		{
			//Debug.Log("射击");

			var a = Instantiate(bullet);
			a.transform.position = weapon.transform.position;
			a.transform.rotation = weapon.transform.rotation;
			a.GetComponent<Rigidbody>().velocity = weapon.transform.forward * 16;
			Destroy(a, 3);
			return true;
		}
		#endregion
	}
}
