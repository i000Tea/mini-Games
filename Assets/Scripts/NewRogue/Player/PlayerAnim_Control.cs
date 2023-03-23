using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

namespace Tea.NewRouge
{
	public class PlayerAnim_Control : Singleton<PlayerAnim_Control>
	{
		[SerializeField]
		Animator anim;
		[SerializeField]
		private InteractionSystem interSystem;
		private AimIK aimIK;

		[SerializeField]
		Transform RHand;
		[SerializeField]
		Transform Weapons;
		PlayerMove move;
		private void OnValidate()
		{
			if (!interSystem)
			{
				if (transform.GetChild(0).TryGetComponent(out InteractionSystem interS))
				{
					interSystem = interS;
				}
			}
			if (!aimIK)
			{
				if (transform.GetChild(0).TryGetComponent(out AimIK aim))
				{
					aimIK = aim;
				}
			}
		}
		protected override void Awake()
		{
			base.Awake();
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
			anim.SetFloat("move", a.z * 0.5f);
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

		public void ReturnLinkWeapon()
		{

		}
		public void FindTargetEnemy()
		{
			if (EnemyManager.I)
			{
				TargetEnemy = EnemyManager.I.FindEnemy();
				if (TargetEnemy)
					aimIK.solver.target = TargetEnemy.GetUnHitPoint();
			}
		}
	}
}
