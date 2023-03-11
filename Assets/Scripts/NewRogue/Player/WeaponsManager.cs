using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tea.NewRouge
{
	public class WeaponsManager : MonoBehaviour
	{
		[SerializeField]
		Player_Control control;

		public Transform weapon;
		public Transform weaponsParent;

		int nowWep;
		public List<WeaponControl> weapons;


		public GameObject bullet;
		[SerializeField]
		float bulletCD = 0.5f;
		float bulletCDNow;

		private void OnValidate()
		{
			if (weaponsParent)
			{
				if (weapons.Count != weaponsParent.childCount)
				{
					weapons = new List<WeaponControl>();

					for (int i = 0; i < weaponsParent.childCount; i++)
					{
						WeaponControl A;
						if (weaponsParent.GetChild(i).TryGetComponent(out WeaponControl wCtrl))
						{
							A = wCtrl;
						}
						else
						{
							A = weaponsParent.GetChild(i).gameObject.AddComponent<WeaponControl>();
						}
						weapons.Add(A);
					}
				}

			}
		}

		private void Update()
		{
			WeaponUpdate();
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				SwitchWeapon(0);
			}
			if (Input.GetKeyDown(KeyCode.Alpha2))
			{

				SwitchWeapon(1);
			}
			if (Input.GetKeyDown(KeyCode.Alpha3))
			{

				SwitchWeapon(2);
			}
			if (Input.GetKeyDown(KeyCode.Alpha4))
			{

				SwitchWeapon(3);
			}
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
					IsShoot(weapons[nowWep].muzzle);
				}
				else
				{
					bulletCDNow -= Time.deltaTime;
				}
			}
		}
		void SwitchWeapon(int weaponNum)
		{
			if (nowWep != weaponNum && weaponNum < weapons.Count)
			{
				for (int i = 0; i < weapons.Count; i++)
				{
					weapons[i].gameObject.SetActive(false);
				}
				weapons[weaponNum].gameObject.SetActive(true);
				nowWep = weaponNum;
			}
		}
		/// <summary>
		/// 射击
		/// </summary>
		/// <returns></returns>
		bool IsShoot(Transform muzzle, float damage = 1)
		{
			//Debug.Log("射击");
			var a = bullet.InstantiateBullet(muzzle, damage);
			return true;
		}
		#endregion
	}
}
