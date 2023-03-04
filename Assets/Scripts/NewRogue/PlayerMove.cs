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
		public CinemachineVirtualCamera virCamera;
		protected override void UpdateRotation()
		{
			//base.UpdateRotation();
			movement.Rotate((a.position - transform.position).normalized, angularSpeed);
		}
		protected override void HandleInput()
		{
			//base.HandleInput();

			moveDirection = new Vector3
			{
				x = Input.GetAxisRaw("Horizontal"),
				y = 0.0f,
				z = Input.GetAxisRaw("Vertical")
			};
		}
		public override void Update()
		{
			base.Update();
			if (Input.GetKey(KeyCode.Q))
			{
				virCamera.transform.eulerAngles += new Vector3(0, 1, 0);
			}
			if (Input.GetKey(KeyCode.E))
			{
				virCamera.transform.eulerAngles += new Vector3(0, -1, 0);
			}

		}
	}
}
