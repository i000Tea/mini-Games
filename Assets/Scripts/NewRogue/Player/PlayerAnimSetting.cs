using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tea.NewRouge
{
	public class PlayerAnimSetting : MonoBehaviour
	{
		[SerializeField]
		Transform RHand;
		[SerializeField]
		Transform Weapons;
		PlayerMove move;

		private void Awake()
		{
			if (!move)
			{
				if (TryGetComponent(out PlayerMove m))
				{
					move = m;
				}
			}
		}
		private void Start()
		{
			SetWeaponParent();
		}
		private void Update()
		{
			var a = move.moveDirection.AngleTransfor(-transform.eulerAngles.y);
			//Debug.Log(a);
			Player_Control.I.animControl.SetFloat("move", a.z * 0.5f);
		}
		/// <summary>
		/// 设置武器父集(到手中)
		/// </summary>
		void SetWeaponParent()
		{
			Weapons.SetParent(RHand);
			Weapons.localPosition = Vector3.zero;
			Weapons.localEulerAngles = Vector3.zero;
		}
	}
}
