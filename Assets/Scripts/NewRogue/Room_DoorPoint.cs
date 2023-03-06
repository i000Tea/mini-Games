using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Tea.NewRouge
{
	/// <summary>
	/// 开门的点
	/// </summary>
	public class Room_DoorPoint : MonoBehaviour
	{
		/// <summary>
		/// 此点是否已被使用
		/// </summary>
		public bool unUse;
		public Room_Control nextRoom;
		Collider trggerColl;
		private void Awake()
		{
			if (TryGetComponent(out Collider coll))
			{
				trggerColl = coll;
			}
			else
				trggerColl = gameObject.AddComponent<BoxCollider>();

			if (nextRoom)
			{
				nextRoom.RoomState = false;
				trggerColl.isTrigger = true;
			}
			else
			{
				trggerColl.isTrigger = false;
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.tag == "Player" && nextRoom)
			{
				nextRoom.RoomState = true;
			}
		}
	}
}
