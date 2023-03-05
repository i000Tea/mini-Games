using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM.Controllers;
using Cinemachine;
namespace Tea.NewRouge
{
	public class PlayerMove : BaseCharacterController
	{
		public Transform a;
		float virRotate;
		public CinemachineVirtualCamera virCamera;
		protected override void UpdateRotation()
		{
			//base.UpdateRotation();
			movement.Rotate((a.position - transform.position).normalized, angularSpeed);
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
			moveDirection = new Vector3
			{
				x = Input.GetAxisRaw("Horizontal") * Mathf.Cos(virRotate * Mathf.Deg2Rad) +
					Input.GetAxisRaw("Vertical") * Mathf.Sin(virRotate * Mathf.Deg2Rad),
				y = 0.0f,
				z = Input.GetAxisRaw("Horizontal") * -Mathf.Sin(virRotate * Mathf.Deg2Rad) +
					Input.GetAxisRaw("Vertical") * Mathf.Cos(virRotate * Mathf.Deg2Rad)
			};
			var x = new Vector3();

		}
		public override void Update()
		{
			base.Update();

			if (Input.GetKey(KeyCode.Q))
			{
				VirRotate(1);
			}
			if (Input.GetKey(KeyCode.E))
			{
				VirRotate(-1);
			}

		}
		void VirRotate(float rotate)
		{
			virCamera.transform.eulerAngles += new Vector3(0, rotate, 0);
			virRotate = virCamera.transform.eulerAngles.y;
		}
	}
}
