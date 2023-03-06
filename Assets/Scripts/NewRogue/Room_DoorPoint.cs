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
		public bool UnUse;
		public Room_Control nextRoom;

		private void OnCollisionEnter(Collision collision)
		{
			if (collision.gameObject.tag == "Player" && nextRoom)
			{
				ShowRoom();
			}
		}
		private void OnTriggerEnter(Collider other)
		{
			if (other.tag == "Player" && nextRoom)
			{
				ShowRoom();
			}
		}
		void ShowRoom()
		{
			if (TryGetComponent(out Collider collider))
			{
				collider.isTrigger = true;
			}
			nextRoom.ShowRoom();
			nextRoom = null;
		}
	}
}
