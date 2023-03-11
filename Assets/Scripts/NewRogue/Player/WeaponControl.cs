using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tea.NewRouge
{
	public class WeaponControl : MonoBehaviour
	{
		public Transform muzzle;
		public WapeonItem item;

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
	}
	[System.Serializable]
	public class WapeonItem
	{
		/// <summary>
		/// 基础子弹
		/// </summary>
		public GameObject BaseBullet;
		/// <summary>
		/// 伤害
		/// </summary>
		public float damage = 1;
		/// <summary>
		/// 射速(/s)
		/// </summary>
		public float RatePerS = 4;
		/// <summary>
		/// 弹速
		/// </summary>
		public float velocity = 1;
		/// <summary>
		/// 大小
		/// </summary>
		public float scale = 1;

	}
}
