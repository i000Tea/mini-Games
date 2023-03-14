using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tea.NewRouge
{
	public class Prop_GetWeapon : MonoBehaviour
	{
		[SerializeField]
		List<GameObject> Weapons;

		[SerializeField]
		Image loading;

		bool LoadingAdd;
		int mySN;

		private void OnValidate()
		{
			Transform parent = transform.GetChild(0);
			if (Weapons == null || Weapons.Count != parent.childCount)
			{
				Weapons = new List<GameObject>();
				for (int i = 0; i < parent.childCount; i++)
				{
					Weapons.Add(parent.GetChild(i).gameObject);
				}
			}
		}
		private void Start()
		{
			SetWeapon();
		}
		private void Update()
		{
			if (loading.LoadingRim(LoadingAdd))
			{
				WeaponsManager.inst.GetWeapon(mySN);
				this.enabled = false;
				gameObject.SetActive(false);
			}
		}

		/// <summary>
		/// 设置武器
		/// </summary>
		public void SetWeapon(int SN = -1)
		{
			for (int i = 0; i < Weapons.Count; i++)
			{
				Weapons[i].SetActive(false);
			}
			if (SN == -1 || SN >= Weapons.Count)
			{
				SN = Random.Range(0, Weapons.Count);
			}
			mySN = SN;
			Weapons[SN].SetActive(true);
		}

		private void OnTriggerStay(Collider other)
		{
			if (other.gameObject.tag == "Player")
			{
				LoadingAdd = true;
			}
		}
		private void OnTriggerExit(Collider other)
		{
			if (other.gameObject.tag == "Player")
			{
				LoadingAdd = false;
			}
		}

	}
}
