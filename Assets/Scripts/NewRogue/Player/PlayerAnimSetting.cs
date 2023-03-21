using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimSetting : MonoBehaviour
{
	[SerializeField]
	Transform RHand;
	[SerializeField]
	Transform Weapons;

	private void Start()
	{
		SetWeaponParent();
	}
	void SetWeaponParent()
	{
		Weapons.SetParent(RHand);
	}
}
