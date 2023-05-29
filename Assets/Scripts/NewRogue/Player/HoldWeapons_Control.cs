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
		/// 拿起武器
		/// </summary>
		public bool TakeWep;
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

		/// <summary>
		/// 基础子弹预制件
		/// </summary>
		public GameObject baseBullet;
		/// <summary>
		/// 射击-冷却需求
		/// </summary>
		[SerializeField]
		float shootNeedCD
		{
			get
			{
				return 1 / weapons[nowWep].RatePerS;
			}
		}
		/// <summary>
		/// 射击-当前冷却
		/// </summary>
		float shootCDNow;

		#endregion

		#region unity
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
		#endregion

		#region Weapon
		protected void Start()
		{
			Game2Manager.I.OnPlayerDead += () =>
			{
				enabled = false; 
			};
		}
		/// <summary>
		/// 武器的每帧计算
		/// </summary>
		void WeaponUpdate()
		{
			if (shootCDNow < shootNeedCD)
			{
				shootCDNow += Time.deltaTime;
			}
			if (PlayerAnim_Control.I)
			{
				if (!PlayerAnim_Control.I.aimOver)
					return;
			}
			//Debug.Log(shootCDNow + " " + shootNeedCD);
			if (control.selectEnemy && shootCDNow >= shootNeedCD)
			{
				//transform.LookAt(Player_Control.I.TargetEnemy.GetUnHitPoint());

				shootCDNow = 0;
				IsShoot(weapons[nowWep]);
				PlayerAnim_Control.I.Shoot(weapons[nowWep].recoil);
				Player_Control.I.FindTargetEnemy();

			}
		}
		/// <summary>
		/// 获取武器
		/// </summary>
		/// <param name="WeaponSN"></param>
		public void GetWeapon(int WeaponSN)
		{
			// 获取武器
			// 只有数据
			weapons[WeaponSN].GetMy();
			// 切换武器
			// 包含动画
			SwitchWeapon(WeaponSN);
		}

		/// <summary>
		/// 切换武器
		/// </summary>
		/// <param name="weaponNum">切换出的武器序号</param>
		public void SwitchWeapon(int weaponNum)
		{
			TakeWep = true;
			// 当切换出的武器不是当前武器且在列表内 
			if (nowWep != weaponNum && weaponNum < weapons.Count)
			{
				// 将其他武器隐藏
				for (int i = 0; i < weapons.Count; i++)
				{
					if (i != weaponNum)
						weapons[i].gameObject.SetActive(false);
				}
				// 动画更换为拿起武器
				PlayerAnim_Control.I.AnimTakeWeapon(true);
				// 激活该武器且链接ik
				weapons[weaponNum].ShowMy();
				// 更新当前武器标记
				nowWep = weaponNum;
			}
		}
		/// <summary>
		/// 收回武器
		/// </summary>
		public void TakeBackWeapon()
		{
			PlayerAnim_Control.I.interSystem.ResumeAll();
			for (int i = 0; i < weapons.Count; i++)
			{
				weapons[i].gameObject.SetActive(false);
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
				Instantiate(bullet).GetComponent<Bullet>().Create(item);
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
