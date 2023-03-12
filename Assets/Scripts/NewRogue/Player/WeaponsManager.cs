using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tea.NewRouge
{
	public class WeaponsManager : MonoBehaviour
	{
		public static WeaponsManager inst;
		[SerializeField]
		Player_Control control;

		public Transform weapon;
		public Transform weaponsParent;

		int nowWep;
		public List<WeaponControl> weapons;


		public GameObject bullet;
		[SerializeField]
		float shootNeedCD
		{
			get
			{
				return 1 / weapons[nowWep].item.RatePerS;
			}
		}
		float shootCDNow;

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
		private void Awake()
		{
			inst = this;
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
			//Debug.Log(shootCDNow + " " + shootNeedCD);
			if (control.targetEnemy)
			{
				transform.LookAt(new Vector3()
				{
					x = control.targetEnemy.transform.position.x,
					y = transform.position.y,
					z = control.targetEnemy.transform.position.z
				});
				if (shootCDNow >= shootNeedCD)
				{
					shootCDNow = 0;
					IsShoot(weapons[nowWep].muzzle, weapons[nowWep].item);
				}
			}
			if (shootCDNow < shootNeedCD)
			{
				shootCDNow += Time.deltaTime;
			}
		}
		public void GetWeapon(int WeaponSN)
		{
			SwitchWeapon(WeaponSN);
		}
		public void SwitchWeapon(int weaponNum)
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
		bool IsShoot(Transform muzzle, WapeonItem item)
		{
			//Debug.Log("射击");
			var a = bullet.InstantiateBullet
				(muzzle, item.damage, HorizOffset: item.Offset, vertiOffset: item.Offset, Scale: item.scale);
			return true;
		}
		#endregion
	}
}
