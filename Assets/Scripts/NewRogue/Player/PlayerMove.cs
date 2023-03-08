using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM.Controllers;
using Cinemachine;
namespace Tea.NewRouge
{
	public class PlayerMove : BaseCharacterController
	{
		Transform targetEnemy
		{
			get
			{
				try
				{
					return Player_Control.inst.targetEnemy.transform;

				}
				catch (System.Exception)
				{
					return null;
				}
			}
		}
		float virRotate;
		public CinemachineVirtualCamera virCamera;
		[SerializeField]
		JoyStick joyStick;

		protected override void UpdateRotation()
		{
			if (targetEnemy)
				movement.Rotate((targetEnemy.position - transform.position).normalized, angularSpeed);
			else
			  base.UpdateRotation();
		}
		protected override void HandleInput()
		{
			//base.HandleInput();

			//moveDirection = new Vector3
			//{
			//	x = Input.GetAxisRaw("Horizontal"),
			//	y = 0.0f,
			//	z = Input.GetAxisRaw("Vertical")
			//};
			//Debug.Log($" {Mathf.Cos(0)} {Mathf.Cos(0.5f)} {Mathf.Cos(1)} \n {Mathf.Cos(virRotate)}");
			Vector2 move;
			if (joyStick)
				move = new Vector2
				{
					x = Input.GetAxisRaw("Horizontal") + joyStick.inputContent.x,
					y = Input.GetAxisRaw("Vertical") + joyStick.inputContent.y
				};
			else
				move = new Vector2
				{
					x = Input.GetAxisRaw("Horizontal"),
					y = Input.GetAxisRaw("Vertical")
				};

			//Debug.Log(joyStick.inputContent);
			moveDirection = new Vector3
			{
				x = move.x * Mathf.Cos(virRotate * Mathf.Deg2Rad) +
					move.y * Mathf.Sin(virRotate * Mathf.Deg2Rad),
				y = 0.0f,
				z = move.x * -Mathf.Sin(virRotate * Mathf.Deg2Rad) +
					move.y * Mathf.Cos(virRotate * Mathf.Deg2Rad)
			};
			jump = Input.GetButton("Jump");

		}
		public override void Update()
		{
			base.Update();

			if (Input.GetKey(KeyCode.Q))
			{
				VirRotate(-1);
			}
			if (Input.GetKey(KeyCode.E))
			{
				VirRotate(1);
			}
		}
		void VirRotate(float rotate)
		{
			virCamera.transform.eulerAngles += new Vector3(0, rotate, 0);
			virRotate = virCamera.transform.eulerAngles.y;
		}
	}
}
