using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tea.PolygonHit
{
	/// <summary>
	/// 本脚本可考虑融合到技能中去
	/// </summary>
	public class Skill3003_CombatDrone : MonoBehaviour
	{
		[SerializeField]
		GameObject enemyTarget;


		#region Move
		float _long;
		float move;
		public Vector3 MoveTarget
		{
			get
			{
				return PlayerBase.Player.position +
					new Vector3(
					Mathf.Cos(move),
					Mathf.Sin(move),
					0) * 0.8f;
			}
		}
		#endregion

		#region lookat

		[Tooltip("旋转速度")]
		public float rotateSpeed;
		[Tooltip("与目标之间的旋转角度")]
		float rotateDiff;

		/// <summary>
		/// 旋转参数
		/// </summary>
		Vector3 dir;
		/// <summary>
		/// 旋转目标Z值
		/// </summary>
		float angle;
		/// <summary>
		/// 旋转目标
		/// </summary>
		Quaternion rotateTarget;
		#endregion

		#region Shoot
		[SerializeField]
		Transform m_shootTarget;
		[SerializeField]
		GameObject Bullet;
		#endregion

		private void FixedUpdate()
		{
			if (!enemyTarget)
			{
				SetTarget();
			}
			UpdateMove();
			UpdateRotate();
		}
		/// <summary>
		/// 射击
		/// </summary>
		/// <returns></returns>
		public bool IsShoot()
		{
			if (rotateDiff > 2)
				return false;
			else
			{
				//Debug.Log("射击");
				var a = GameManager.inst.TeaInstantiate(Bullet, m_shootTarget);
				a.GetComponent<Rigidbody2D>().velocity = transform.up * 16;
				Destroy(a, 3);
				SetTarget();
				return true;
			}
		}
		public void SetTarget()
		{
			enemyTarget = AddVoids.ListMin(EnemyManager.nowEnemys, transform.position);
			//Debug.Log(enemyTarget);
		}
		void UpdateMove()
		{
			move += Time.deltaTime;
			_long = Vector3.Distance(transform.position, MoveTarget);
			if (_long > 0.08f)
				transform.position = Vector3.Lerp
					(transform.position, MoveTarget, _long * 0.05f);
		}
		void UpdateRotate()
		{
			//Debug.Log(enemyTarget);

			if (!enemyTarget)
				return;
			//Debug.Log("2233 旋转");
			// 计算目标与自身的向量差
			dir = enemyTarget.transform.position - transform.position;
			// 向量Z归零
			dir.z = 0;
			// 返回有符号的角度
			angle = Vector3.SignedAngle(Vector3.up, dir, Vector3.forward);
			// 角度转四元数
			rotateTarget = Quaternion.Euler(0, 0, angle);

			// 设置 旋转角度差
			rotateDiff = Mathf.Abs(rotateTarget.eulerAngles.z - transform.localRotation.eulerAngles.z);

			// 定义旋转sudu
			var speed = rotateSpeed * Time.deltaTime;
			//if (rotateDiff > 10)
			//{
			//    speed += rotateDiff;
			//}

			// 当旋转角度差极小时 直接赋值 否则 插值计算
			if (rotateDiff < 0.025f)
				transform.localRotation = rotateTarget;
			else
				transform.localRotation = Quaternion.Slerp(transform.localRotation, rotateTarget, speed);
		}
	}
}
