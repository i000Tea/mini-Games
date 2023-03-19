using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM.Controllers;
using Cinemachine;
namespace Tea.NewRouge
{
	public class EnemyMove : BaseAgentController
	{
		protected override void HandleInput()
		{
			base.HandleInput();

			agent.SetDestination(Player_Control.I.PlayerPoint);
		}
	}
}
