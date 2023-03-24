using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM.Controllers;
using Cinemachine;
namespace Tea.NewRouge
{
	public class EnemyMove : BaseAgentController
	{
		Enemy_Control my;
		private void Start()
		{
			my = GetComponent<Enemy_Control>();
		}
		protected override void HandleInput()
		{
			base.HandleInput();

			agent.SetDestination(Player_Control.I.transform.position);
			var length = Vector3.Distance(transform.position, Player_Control.I.transform.position);

			my.ReadyAtk(length);
		}
	}
}
