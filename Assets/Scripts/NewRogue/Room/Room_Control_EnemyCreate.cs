using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace Tea.NewRouge
{
	/// <summary>
	/// 房间的控制器
	/// </summary>
	public class Room_Control_EnemyCreate : Room_Control
	{
		public Transform enemyCreatePoint;

		public override void RoomAwakeSet(bool roomState = false)
		{
			base.RoomAwakeSet(roomState);
			//Debug.Log("敌人房间");
			EnemyManager.I.createPoints.Add(enemyCreatePoint);
		}
	}
}
