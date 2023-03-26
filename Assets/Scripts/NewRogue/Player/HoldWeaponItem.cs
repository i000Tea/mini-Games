using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;
namespace Tea.NewRouge
{
	/// <summary>
	/// 持有武器的信息参数
	/// </summary>
	public class HoldWeaponItem : MonoBehaviour
	{
		public bool IsStartToGet;
		public bool IsUse;
		/// <summary>
		/// 枪口位置
		/// </summary>
		public Transform muzzle;

		#region  parameter 射击参数
		[Header("射击参数")]
		/// <summary>
		/// 基础子弹
		/// </summary>
		public GameObject weaponBullet;
		/// <summary>
		/// 伤害
		/// </summary>
		public float damage = 1;
		/// <summary>
		/// 射速(/s)
		/// </summary>
		public float RatePerS = 4;
		/// <summary>
		/// 子弹偏移
		/// </summary>
		[Range(0, 0.5f)]
		public float Offset = 0.01f;
		/// <summary>
		/// 弹速
		/// </summary>
		public float velocity = 1;
		/// <summary>
		/// 弹速
		/// </summary>
		public float recoil = 1;
		/// <summary>
		/// 大小
		/// </summary>
		public float scale = 0.5f;

		#endregion

		/// <summary>
		/// 当前持有数量
		/// </summary>
		int holding = 0;

		private void OnValidate()
		{
			if (!muzzle)
			{
				for (int i = 0; i < transform.childCount; i++)
				{
					if (transform.GetChild(i).tag == "Point")
					{
						muzzle = transform.GetChild(i);
						break;
					}
				}
			}
		}

		/// <summary>
		/// 获得自身(当前只有增加数量)
		/// </summary>
		public void GetMy()
		{
			holding++;
		}
		/// <summary>
		/// 掏出自身 有ik的时候把手绑定上去
		/// </summary>
		public void ShowMy()
		{
			//Debug.Log("?");
			gameObject.SetActive(true);
			if (TryGetComponent(out InteractionObject interObj))
			{
				StartCoroutine(PlayerAnim_Control.I.ReturnLinkWeaponIK(interObj));
			}
		}
	}
}
