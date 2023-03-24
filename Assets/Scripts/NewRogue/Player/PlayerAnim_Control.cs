using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
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

		/// <summary>
		/// 当没有敌人时的ik目标
		/// </summary>
		Transform aimIKBaseTarget;
		[SerializeField] Transform ShakeObj;

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
			aimIKBaseTarget = aimIK.solver.target;
		}

		private void Update()
		{
			var a = move.moveDirection.AngleTransfor(-transform.eulerAngles.y);
			//Debug.Log(a);
			anim.SetFloat("move", a.z * 0.5f);
		}
		public void BeHit()
		{
			ShakeObj.DOShakePosition(0.3f, 0.15f, fadeOut: true, randomnessMode: ShakeRandomnessMode.Harmonic);
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
		/// <param name="inputTarget"> 输入的新的目标 </param>
		/// <returns></returns>
		public IEnumerator TargetLink(Transform inputTarget)
		{
			// 如果当前目标和更新目标距离小于1 则直接更新 返回
			if (Vector3.Distance(aimIK.solver.target.position, inputTarget.position) < 3)
			{
				aimIK.solver.target = inputTarget;
				yield break;
			}
			Debug.Log("重新校准");
			// 否则 开始 重新校准 过渡动画

			// 先把这个关闭
			aimOver = false;

			// 当前目标 获取为aimIK中的目标

			// 若当前位置为基础(即当前没有锁定的敌人)
			if (aimIK.solver.target == aimIKBaseTarget)
			{
				//把基准位置更新到玩家和新的点位的中间
				aimIKBaseTarget.position = Vector3.Lerp(Player_Control.I.transform.position, inputTarget.position, 0.5f);
			}
			else
			{
				// 否则 更新到旧点位
				aimIKBaseTarget.position = aimIK.solver.target.position;
			}
			// 把目标重新锁回空对象
			aimIK.solver.target = aimIKBaseTarget;

			var nowTarget = aimIK.solver.target;
			while (Vector3.Distance(nowTarget.position, inputTarget.position) > 0.2f)
			{
				nowTarget.position = Vector3.Lerp(nowTarget.position, inputTarget.position, 0.5f);
				if (aimIK.solver.IKPositionWeight < 1)
					aimIK.solver.IKPositionWeight += Time.deltaTime * 4;
				yield return new WaitForFixedUpdate();
			}
			aimIK.solver.target = inputTarget;
			aimIK.solver.IKPositionWeight = 1;
			aimOver = true;
		}
	}
}
