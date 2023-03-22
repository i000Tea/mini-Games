using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tea.NewRouge
{
	public class HoldWeapons_Control : Singleton<HoldWeapons_Control>
	{
		#region 变量
		/// <summary>
		/// 玩家控制器
		/// </summary>
		[SerializeField]
		Player_Control control;

		/// <summary>
		/// 存放武器的父对象
		/// </summary>
		public Transform weaponsParent;
		/// <summary>
		/// 当前使用的武器序号
		/// </summary>
		int nowWep;
		/// <summary>
		/// 库存武器列表
		/// </summary>
		public List<HoldWeaponItem> weapons;

		public GameObject baseBullet;
		[SerializeField]
		float shootNeedCD
		{
			get
			{
				return 1 / weapons[nowWep].RatePerS;
			}
		}
		float shootCDNow;

		#endregion

		private void OnValidate()
		{
			if (weaponsParent)
			{
				if (weapons.Count != weaponsParent.childCount)
				{
					weapons = new List<HoldWeaponItem>();

					for (int i = 0; i < weaponsParent.childCount; i++)
					{
						HoldWeaponItem A;
						if (weaponsParent.GetChild(i).TryGetComponent(out HoldWeaponItem wCtrl))
						{
							A = wCtrl;
						}
						else
						{
							A = weaponsParent.GetChild(i).gameObject.AddComponent<HoldWeaponItem>();
						}
						weapons.Add(A);
					}
				}

			}
		}
		private IEnumerator Start()
		{
			yield return new WaitForSeconds(0.1f);
			GetWeapon(20);
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
			if (control.TargetEnemy)
			{
				//transform.LookAt(Player_Control.I.TargetEnemy.GetUnHitPoint());
				if (shootCDNow >= shootNeedCD)
				{
					shootCDNow = 0;
					IsShoot(weapons[nowWep]);
					Player_Control.I.FindTargetEnemy();
				}
			}
			if (shootCDNow < shootNeedCD)
			{
				shootCDNow += Time.deltaTime;
			}
		}
		public void GetWeapon(int WeaponSN)
		{
			weapons[WeaponSN].GetMy();
			SwitchWeapon(WeaponSN);
		}
		public void SwitchWeapon(int weaponNum)
		{
			Player_Control.I.interSystem.ResumeAll();
			if (nowWep != weaponNum && weaponNum < weapons.Count)
			{
				for (int i = 0; i < weapons.Count; i++)
				{
					if (i != weaponNum)
						weapons[i].gameObject.SetActive(false);
				}
				weapons[weaponNum].ShowMy();
				nowWep = weaponNum;
			}
		}
		/// <summary>
		/// 射击
		/// </summary>
		/// <returns></returns>
		bool IsShoot(HoldWeaponItem item)
		{
			// 获取子弹 若武器信息中没有子弹 则使用基础子弹
			var bullet = baseBullet;
			if (item.weaponBullet)
				bullet = item.weaponBullet;

			//Debug.Log("射击");

			// 实例化子弹
			try
			{
				var a = bullet.InstantiateBullet(item.muzzle, item.damage, HorizOffset: item.Offset,
						velocity: item.velocity, vertiOffset: item.Offset, Scale: item.scale);
				return true;
			}
			catch (System.Exception)
			{
				return false;
			}
		}
		#endregion
	}
}
