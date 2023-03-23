using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

namespace Tea.NewRouge
{
	/// <summary>
	/// 角色动画的处理
	/// </summary>
	public class PlayerAnim_Control : Singleton<PlayerAnim_Control>
	{
		[SerializeField]
		Animator anim;
		[SerializeField]
		public InteractionSystem interSystem;

		public AimIK aimIK;

		Transform aimIKTarget;

		/// <summary>
		/// 已锁定敌人
		/// </summary>
		public bool aimOver;

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
			aimIK.solver.IKPositionWeight = 0;
			aimIKTarget = aimIK.solver.target;
		}

		private void Update()
		{
			var a = move.moveDirection.AngleTransfor(-transform.eulerAngles.y);
			//Debug.Log(a);
			anim.SetFloat("move", a.z * 0.5f);
		}

		public void AnimTakeWeapon(bool isTake)
		{
			anim.SetBool("TakeWeapon", isTake);
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

		/// <summary>
		/// 脱离所有ik
		/// </summary>
		public void InterResumeAll()
		{
			interSystem.ResumeAll();
		}
		/// <summary>
		/// 重新链接武器IK
		/// </summary>
		public IEnumerator ReturnLinkWeaponIK(InteractionObject interObj)
		{
			interSystem.ResumeAll();
			yield return new WaitForFixedUpdate();
			interSystem.StartInteraction(FullBodyBipedEffector.LeftHand, interObj, true);
		}
		/// <summary>
		/// 链接目标
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		public IEnumerator TargetLink(Transform target)
		{
			var nowTarget = aimIK.solver.target;
			nowTarget.position = Vector3.Lerp(Player_Control.I.transform.position, target.position, 0.5f);

			while (Vector3.Distance(nowTarget.position, target.position) > 0.2f)
			{
				nowTarget.position = Vector3.Lerp(nowTarget.position, target.position, 0.5f);
				aimIK.solver.poleWeight += Time.deltaTime * 4;
				yield return new WaitForFixedUpdate();
			}
			aimIK.solver.target = target;
			aimIK.solver.poleWeight = 1;
			aimOver = true;
		}
	}
}
